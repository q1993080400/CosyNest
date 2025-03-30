using System.Reflection;

namespace System.DataFrancis;

public static partial class CreateDataObj
{
    //这个部分类专门用来声明有关数据验证的API

    #region 创建数据验证默认委托
    #region 正式方法
    /// <summary>
    /// 创建一个用于验证数据的委托
    /// </summary>
    /// <param name="dataVerifyInfo">用于创建数据验证委托的参数</param>
    /// <returns></returns>
    public static DataVerify DataVerifyDefault(DataVerifyInfo dataVerifyInfo)
        => obj => DataVerifyRealize(obj, dataVerifyInfo);
    #endregion
    #region 内部方法
    /// <summary>
    /// 这个方法是用于验证的方法的实现
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="DataVerify"/>
    /// <inheritdoc cref="DataVerifyDefault(DataVerifyInfo)"/>
    private static VerificationResults DataVerifyRealize(object obj, DataVerifyInfo dataVerifyInfo)
    {
        var type = obj.GetType();
        var propertys = dataVerifyInfo.GetVerifyPropertys(type);
        var previewFileTypeInfo = GetPreviewFileTypeInfo(type);
        #region 额外验证的方法
        IEnumerable<FailureReason> Additional()
        {
            var results = (obj as IDataVerify)?.Verify();
            if (results is null)
                yield break;
            foreach (var item in results)
            {
                yield return new()
                {
                    Prompt = item,
                    MemberInfo = obj.GetType()
                };
            }
        }
        #endregion
        var verify = propertys.Select(property =>
        {
            var propertyName = property.Name;
            var renderDataAttribute = property.GetCustomAttribute<RenderDataAttribute>();
            var value = property.GetValue(obj);
            #region 用来返回验证结果的本地函数
            string? VerificationResults()
            {
                var name = renderDataAttribute?.Name ?? propertyName;
                var previewFilePropertyInfo = previewFileTypeInfo.HasPreviewFilePropertyInfo.GetValueOrDefault(propertyName);
                var nullabilityInfo = property.GetNullabilityInfo();
                #region 返回是否不存在上传文件的参数
                static bool NotPreviewFile(params IHasPreviewFile[] previewFiles)
                => !previewFiles.WhereEnable().Any();
                #endregion
                switch ((nullabilityInfo.ReadState, previewFilePropertyInfo, value))
                {
                    case (NullabilityState.Nullable, _, null):
                        return null;
                    case (_, null, null or ""):
                        return $"{name}不能不填";
                    case (_, IHasPreviewFilePropertyDirectInfo { HasPreviewFile: true, Multiple: false, IsDirectMap: false }, IProjection<IHasPreviewFile> projection) when NotPreviewFile(projection.Projection()):
                    case (_, IHasPreviewFilePropertyDirectInfo { HasPreviewFile: true, Multiple: false, IsDirectMap: true }, null):
                    case (_, IHasPreviewFilePropertyDirectInfo { HasPreviewFile: true, Multiple: true, AllowEmptyCollection: false, IsDirectMap: true }, IEnumerable<IHasReadOnlyPreviewFile> files) when !files.Any():
                    case (_, IHasPreviewFilePropertyDirectInfo { HasPreviewFile: true, Multiple: true, AllowEmptyCollection: false, IsDirectMap: false }, IEnumerable<IProjection<IHasPreviewFile>> projections) when NotPreviewFile([.. projections.Projection()]):
                        return $"{name}没有上传任何文件";
                    case (_, _, { } value):
                        var attribute = property.GetCustomAttribute<VerifyAttribute>();
                        if (attribute is null)
                            return null;
                        var verify = attribute.Verify(value, name, obj => DataVerifyRealize(obj, dataVerifyInfo));
                        return verify is null ?
                        null : attribute.Message ?? verify;
                    default:
                        throw new NotSupportedException($"无法验证这个数据");
                }
            }
            #endregion
            var prompt = VerificationResults();
            var results = prompt is null ? null :
            new FailureReason()
            {
                Prompt = prompt,
                MemberInfo = property
            };
            #region 递归验证的方法
            IEnumerable<FailureReason> Recursive()
            => (renderDataAttribute, value) switch
            {
                not ({ IsRecursion: true }, { }) => [],
                (_, IEnumerable<object> list) => list.Select(x => DataVerifyRealize(x, dataVerifyInfo).FailureReason).SelectMany(),
                (_, var value) => DataVerifyRealize(value, dataVerifyInfo).FailureReason
            };
            #endregion
            var recursive = (renderDataAttribute, value) is ({ IsRecursion: true }, { }) ?
            DataVerifyRealize(value, dataVerifyInfo).FailureReason : [];
            FailureReason?[] array = [results, .. Recursive()];
            return array;
        }).SelectMany().WhereNotNull().Concat(Additional()).ToArray().DistinctBy(x => x.MemberInfo).ToArray();
        return new()
        {
            Data = obj,
            FailureReason = verify
        };
    }
    #endregion
    #endregion
}
