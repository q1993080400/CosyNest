namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个针对属性的，
/// 封装可预览文件的信息，
/// 它指示该属性直接封装了可预览文件，
/// 直接封装的意思是：
/// 这个类型或属性就是可预览文件，或它的集合自身
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
    #region 是否允许空集合
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该属性是可预览文件的集合，而且它允许空集合，
    /// 不要求集合中至少有一个文件
    /// </summary>
    bool AllowEmptyCollection { get; }
    #endregion
    #region 是否直接映射
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示它直接映射到<see cref="IHasReadOnlyPreviewFile"/>或它的集合，
    /// 否则表示它是通过<see cref="MapToPreviewFileAttribute"/>特性间接映射的
    /// </summary>
    bool IsDirectMap { get; }
    #endregion
    #region 读取直接封装的可预览文件
    /// <summary>
    /// 获取这个属性所直接封装的可预览文件
    /// </summary>
    /// <param name="obj">属性依附的对象实例，
    /// 如果它为<see langword="null"/>，则返回一个空集合</param>
    /// <returns></returns>
    IReadOnlyCollection<IHasReadOnlyPreviewFile> GetPreviewFile(object? obj);
    #endregion
    #region 写入直接封装的可预览文件
    /// <summary>
    /// 写入这个属性所直接封装的可预览文件
    /// </summary>
    /// <param name="obj">属性依附的对象实例</param>
    /// <param name="files">待写入的可预览文件</param>
    void SetPreviewFile(object obj, IEnumerable<IHasReadOnlyPreviewFile> files);
    #endregion
}
