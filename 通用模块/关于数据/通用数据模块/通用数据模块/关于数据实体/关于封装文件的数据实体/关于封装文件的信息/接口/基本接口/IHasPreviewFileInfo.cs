namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为封装可预览文件的信息
/// </summary>
public interface IHasPreviewFileInfo
{
    #region 递归获取所有可预览文件
    /// <summary>
    /// 递归获取某一类型的对象所引用的可预览文件
    /// </summary>
    /// <param name="obj">属性依附的对象实例，
    /// 如果它为<see langword="null"/>，则返回一个空集合</param>
    /// <returns></returns>
    IEnumerable<PreviewFileInfo> AllPreviewFile(object? obj);
    #endregion
    #region 是否为严格模式
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该对象为严格模式，它表示必然直接或间接封装了<see cref="IHasPreviewFile"/>或它的集合，
    /// 否则只表示封装了<see cref="IHasReadOnlyPreviewFile"/>或它的集合，
    /// 或者根本没有封装可预览属性
    /// </summary>
    bool IsStrict { get; }
    #endregion
    #region 封装可预览文件的状态
    /// <summary>
    /// 返回封装可预览文件的状态
    /// </summary>
    HasPreviewFileState HasPreviewFileState { get; }
    #endregion
    #region 是否封装了可上传文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个对象封装了可上传文件
    /// </summary>
    bool HasPreviewFile
        => (HasPreviewFileState, IsStrict) is (not HasPreviewFileState.None, true);
    #endregion
}
