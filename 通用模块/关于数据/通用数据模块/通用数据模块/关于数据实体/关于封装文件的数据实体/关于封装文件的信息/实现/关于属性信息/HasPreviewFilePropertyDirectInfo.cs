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
    #region 是否是否可写入
    public required bool IsInitOnly { get; init; }
    #endregion
    #region 递归获取所有可预览文件
    public IEnumerable<PreviewFileInfo> AllPreviewFile(object? obj)
    {
        if (obj is null)
            yield break;
        #region 本地函数
        PreviewFileInfo Convert(IEnumerable<IHasReadOnlyPreviewFile?> file)
            => new()
            {
                Files = file.WhereNotNull().ToArray(),
                Property = Property,
                Target = obj,
                Multiple = Multiple,
                IsStrict = IsStrict
            };
        #endregion
        var value = Property.GetValue(obj);
        var list = Multiple ?
            (value.To<IEnumerable<IHasReadOnlyPreviewFile?>>() ?? []) :
            [value.To<IHasReadOnlyPreviewFile>()];
        yield return Convert(list);
    }
    #endregion
    #region 获取直接封装的可预览文件
    public IReadOnlyCollection<IHasReadOnlyPreviewFile> AllPreviewFileDirect(object? obj)
    {
        var value = Property.GetValue(obj);
        return (Multiple ?
            (value.To<IEnumerable<IHasReadOnlyPreviewFile?>>() ?? []) :
            [value.To<IHasReadOnlyPreviewFile>()]).WhereEnable().ToArray();
    }
    #endregion
    #region 是否为严格模式
    public required bool IsStrict { get; init; }
    #endregion
    #region 封装可预览文件的状态
    public HasPreviewFileState HasPreviewFileState
        => HasPreviewFileState.Direct;
    #endregion
}
