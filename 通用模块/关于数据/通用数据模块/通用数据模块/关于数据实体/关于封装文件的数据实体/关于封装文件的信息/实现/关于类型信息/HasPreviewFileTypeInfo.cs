
namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasPreviewFileTypeInfo"/>的实现，
/// 可以作为一个针对类型的，封装可预览文件的信息
/// </summary>
sealed class HasPreviewFileTypeInfo : IHasPreviewFileTypeInfo
{
    #region 封装可预览文件的类型
    public required Type Type { get; init; }
    #endregion
    #region 索引对可预览文件属性的描述
    public required IReadOnlyDictionary<string, IHasPreviewFilePropertyInfo> HasPreviewFilePropertyInfo { get; init; }
    #endregion
    #region 集合元素的类型信息
    public required IHasPreviewFileTypeInfo? ElementPreviewFileTypeInfo { get; init; }
    #endregion
    #region 递归获取所有可预览文件
    public IEnumerable<PreviewFileInfo> AllPreviewFile(object? obj, bool isStrict)
    {
        if (obj is null)
            yield break;
        var previewFilePropertyInfos = HasPreviewFilePropertyInfo.Values.
            Where(x => x.IsStrict || !isStrict);
        foreach (var propertyInfo in previewFilePropertyInfos)
        {
            foreach (var item in propertyInfo.AllPreviewFile(obj, isStrict))
            {
                yield return item;
            }
        }
    }
    #endregion
    #region 是否为严格模式
    public required bool IsStrict { get; init; }
    #endregion
    #region 是否直接映射
    public required bool IsDirect { get; init; }
    #endregion
    #region 封装可预览文件的状态
    #region 正式成员
    public required HasPreviewFileState HasPreviewFileState { get; init; }
    #endregion
    #endregion
    #region 已知派生类型
    public required IReadOnlyDictionary<Type, IHasPreviewFileTypeInfo> KnownDerivedTypes { get; init; }
    #endregion
}