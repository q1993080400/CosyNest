namespace System.DataFrancis;

/// <summary>
/// 这个枚举指示类型封装可预览文件的状态，
/// 可预览文件指的是<see cref="IHasPreviewFile"/>或它的集合
/// </summary>
public enum HasPreviewFileState
{
    /// <summary>
    /// 不封装可预览文件
    /// </summary>
    None,
    /// <summary>
    /// 直接封装可预览文件
    /// </summary>
    Direct,
    /// <summary>
    /// 递归封装可预览文件
    /// </summary>
    Recursion
}
