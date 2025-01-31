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
        var verify = propertys.Select(property =>
        {
            var propertyName = property.Name;
            var name = property.GetCustomAttribute<RenderDataAttribute>()?.Name ?? propertyName;
            var previewFilePropertyInfo = previewFileTypeInfo.HasPreviewFilePropertyInfo.GetValueOrDefault(propertyName);
            var nullabilityInfo = property.GetNullabilityInfo();
            switch ((nullabilityInfo.ReadState, previewFilePropertyInfo, property.GetValue(obj)))
            {
                case (NullabilityState.Nullable, _, null):
                    return null;
                case (_, null, null or ""):
                    return (property, $"{name}不能为空");
                case (_, IHasPreviewFilePropertyDirectInfo { HasPreviewFile: true, Multiple: false }, null):
                case (_, IHasPreviewFilePropertyDirectInfo { HasPreviewFile: true, Multiple: true }, IEnumerable<IHasReadOnlyPreviewFile> files) when !files.Any():
                    return (property, $"{name}没有上传任何文件");
                case (_, _, { } value):
                    var attribute = property.GetCustomAttribute<VerifyAttribute>();
                    if (attribute is null)
                        return ((PropertyInfo, string)?)null;
                    var verify = attribute.Verify(value, name, obj => DataVerifyRealize(obj, dataVerifyInfo));
                    return verify is null ?
                    null : (property, attribute.Message ?? verify);
                default:
                    throw new NotSupportedException($"无法验证这个数据");
            }
        }).WhereNotNull().ToArray();
        return new()
        {
            Data = obj,
            FailureReason = verify!
        };
    }
    #endregion
    #endregion
}
