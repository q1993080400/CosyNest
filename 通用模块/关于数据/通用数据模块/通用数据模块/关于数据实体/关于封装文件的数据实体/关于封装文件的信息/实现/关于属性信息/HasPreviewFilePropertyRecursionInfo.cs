using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是对递归封装可预览文件的属性的描述，
/// 它指示该属性递归封装了可预览文件
/// </summary>
sealed class HasPreviewFilePropertyRecursionInfo : IHasPreviewFilePropertyRecursionInfo
{
    #region 对属性类型的描述
    public required IHasPreviewFileTypeInfo PropertyTypeInfo { get; init; }
    #endregion
    #region 封装的属性
    public required PropertyInfo Property { get; init; }
    #endregion
    #region 是否可写入
    public required bool IsInitOnly { get; init; }
    #endregion
    #region 递归获取所有可预览文件
    public IEnumerable<PreviewFileInfo> AllPreviewFile(object? obj)
    {
        var typeInfo = PropertyTypeInfo;
        foreach (var propertyInfo in typeInfo.HasPreviewFilePropertyInfo.Values)
        {
            var value = propertyInfo.Property.GetValue(obj);
            foreach (var previewFileInfo in propertyInfo.AllPreviewFile(value))
            {
                yield return previewFileInfo;
            }
        }
    }
    #endregion
    #region 是否为严格模式
    public bool IsStrict
        => PropertyTypeInfo.IsStrict;
    #endregion
    #region 封装可预览文件的状态
    public HasPreviewFileState HasPreviewFileState
        => HasPreviewFileState.Recursion;
    #endregion 
}
