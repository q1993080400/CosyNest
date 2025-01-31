namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个针对属性的，
/// 封装可预览文件的信息，
/// 它指示该属性直接封装了可预览文件
/// </summary>
public interface IHasPreviewFilePropertyDirectInfo : IHasPreviewFilePropertyInfo
{
    #region 是否为可预览文件的集合
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该属性为可预览文件的集合，
    /// 否则表示只能封装单个可预览文件
    /// </summary>
    bool Multiple { get; }
    #endregion
    #region 获取直接封装的可预览文件
    /// <summary>
    /// 获取这个属性所直接封装的可预览文件
    /// </summary>
    /// <param name="obj">属性依附的对象实例，
    /// 如果它为<see langword="null"/>，则返回一个空集合</param>
    /// <returns></returns>
    IReadOnlyCollection<IHasReadOnlyPreviewFile> AllPreviewFileDirect(object? obj);
    #endregion
}
