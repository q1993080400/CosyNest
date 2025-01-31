
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
    #region 递归获取所有可预览文件
    public IEnumerable<PreviewFileInfo> AllPreviewFile(object? obj)
    {
        if (obj is null)
            yield break;
        foreach (var propertyInfo in HasPreviewFilePropertyInfo.Values)
        {
            foreach (var item in propertyInfo.AllPreviewFile(obj))
            {
                yield return item;
            }
        }
    }
    #endregion
    #region 是否为严格模式
    public required bool IsStrict { get; init; }
    #endregion
    #region 封装可预览文件的状态
    public required HasPreviewFileState HasPreviewFileState { get; init; }
    #endregion 
}