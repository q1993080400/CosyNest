using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是对于某个封装可预览文件属性的描述，
/// 它指示该属性直接封装了可预览文件
/// </summary>
sealed class HasPreviewFilePropertyDirectInfo : IHasPreviewFilePropertyDirectInfo
{
    #region 封装可预览文件的属性
    public required PropertyInfo Property { get; init; }
    #endregion 
    #region 是否为可预览文件的集合
    public required bool Multiple { get; init; }
    #endregion
    #region 是否允许空集合
    public required bool AllowEmptyCollection { get; init; }
    #endregion
    #region 是否直接映射
    public bool IsDirectMap
        => CreateDataObj.GetPreviewFileTypeInfo(Property.PropertyType).IsDirect;
    #endregion
    #region 是否是否可写入
    public required bool IsInitOnly { get; init; }
    #endregion
    #region 递归获取所有可预览文件
    public IEnumerable<PreviewFileInfo> AllPreviewFile(object? obj, bool isStrict)
    {
        if (obj is null)
            yield break;
        var propertyType = Property.PropertyType;
        var previewFileTypeInfo = CreateDataObj.GetPreviewFileTypeInfo(propertyType);
        if (previewFileTypeInfo.HasPreviewFileState is HasPreviewFileState.PreviewFile)
        {
            yield return new()
            {
                PreviewFilePropertyInfo = this,
                Target = obj
            };
            yield break;
        }
        var previewFilePropertyInfos = previewFileTypeInfo.
            HasPreviewFilePropertyInfo.Values.OfType<IHasPreviewFilePropertyDirectInfo>().
            Where(x => x.IsStrict || !isStrict).ToArray();
        if (previewFilePropertyInfos.Length is 0)
            yield break;
        var value = Property.GetValue(obj);
        foreach (var previewFilePropertyInfo in previewFilePropertyInfos)
        {
            var previewFileProperty = previewFilePropertyInfo.Property;
            var previewFilePropertyValue = previewFileProperty.GetValue(value);
            if (previewFilePropertyValue is null)
                continue;
            var previewFileInfo = new PreviewFileInfo()
            {
                PreviewFilePropertyInfo = previewFilePropertyInfo,
                Target = previewFilePropertyValue
            };
            yield return previewFileInfo;
        }
    }
    #endregion
    #region 读取直接封装的可预览文件
    public IReadOnlyCollection<IHasReadOnlyPreviewFile> GetPreviewFile(object? obj)
    {
        if (obj is null)
            return [];
        var value = Property.GetValue(obj);
        if (value is null)
            return [];
        object[] files = Multiple ? [.. value.To<IEnumerable<object>>()] : [value];
        #region 返回用来转换的本地函数
        Func<object, IHasReadOnlyPreviewFile?> Convert()
        {
            if (IsDirectMap)
                return x => (IHasReadOnlyPreviewFile)x;
            var projectionType = typeof(IProjection<>).MakeGenericType(typeof(IHasReadOnlyPreviewFile));
            var method = projectionType.GetMethod(nameof(IProjection<>.Projection))!;
            return x => x is null ? null : method.Invoke<IHasReadOnlyPreviewFile>(x);
        }
        #endregion
        var convert = Convert();
        return [.. files.Select(convert).WhereNotNull().WhereEnable()];
    }
    #endregion
    #region 写入直接封装的可预览文件
    public void SetPreviewFile(object obj, IEnumerable<IHasReadOnlyPreviewFile> files)
    {
        var fileArray = files.WhereEnable().ToArray();
        if ((Multiple, fileArray) is (false, { Length: > 1 }))
            throw new NotSupportedException($"{Property}只接受单个可预览文件，但是写入了可预览文件的集合");
        var propertyType = Property.PropertyType;
        #region 返回用来转换的本地函数
        Func<IHasReadOnlyPreviewFile, object> Convert()
        {
            if (IsDirectMap)
                return x => x;
            var previewFileType = propertyType.GetCollectionElementType() ?? propertyType;
            var method = previewFileType.GetMethods(BindingFlags.Static | BindingFlags.Public).
                Where(x =>
                {
                    if (x.Name is not nameof(ICreate<,>.Create))
                        return false;
                    var parameterTypes = x.GetParameterTypes();
                    return parameterTypes.Length is 1 &&
                    typeof(IHasReadOnlyPreviewFile).IsAssignableFrom(parameterTypes[0]) &&
                    previewFileType.IsAssignableFrom(x.ReturnType);
                }).Single();
            return x => method.Invoke(null, [x])!;
        }
        #endregion
        var convert = Convert();
        var convertFiles = files.Select(convert).ToArray();
        var setValue = Multiple ?
            propertyType.CreateCollection(convertFiles) :
            convertFiles.SingleOrDefault();
        Property.SetValue(obj, setValue);
    }
    #endregion
    #region 是否为严格模式
    public required bool IsStrict { get; init; }
    #endregion
    #region 封装可预览文件的状态
    public required HasPreviewFileState HasPreviewFileState { get; init; }
    #endregion
}
