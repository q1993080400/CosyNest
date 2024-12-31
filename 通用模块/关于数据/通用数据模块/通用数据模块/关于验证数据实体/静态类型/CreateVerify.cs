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
        => entity =>
        {
            var type = entity.GetType();
            var propertys = dataVerifyInfo.GetVerifyPropertys(type);
            var previewFilePropertyNatureState = HasPreviewFilePropertyNatureState.Get(type);
            var verify = propertys.Select(property =>
            {
                var propertyName = property.Name;
                var name = property.GetCustomAttribute<RenderDataAttribute>()?.Name ?? propertyName;
                var value = property.GetValue(entity);
                var previewFilePropertyDescribe = previewFilePropertyNatureState.
                PreviewFilePropertyDescribe.GetValueOrDefault(propertyName);
                var nullabilityInfo = property.GetNullabilityInfo();
                if (propertyName is "Attachment")
                {

                }
                switch ((nullabilityInfo.ReadState, previewFilePropertyDescribe, value))
                {
                    case (NullabilityState.NotNull, { IsStrict: true }, null):
                    case (NullabilityState.NotNull, { IsStrict: true, Multiple: true }, IEnumerable<IHasPreviewFile> files) when !files.Any():
                        return (property, $"{name}没有上传任何文件");
                    case (NullabilityState.NotNull, _, null or ""):
                        return (property, $"{name}不能为空");
                    default:
                        var attribute = property.GetCustomAttribute<VerifyAttribute>();
                        if (attribute is null)
                            return (property, null);
                        var verify = attribute.Verify(value, name);
                        return (property, verify is null ? null : attribute.Message ?? verify);
                }
            }).Where(x => x.Item2 is { }).ToArray();
            return new()
            {
                Data = entity,
                FailureReason = verify!
            };
        };
    #endregion  
    #endregion
}
