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
    /// 指示这个类型或属性就是可预览文件，
    /// 或它的集合自身
    /// </summary>
    PreviewFile,
    /// <summary>
    /// 直接封装可预览文件，
    /// 直接的定义是：它的子属性封装了<see cref="IHasReadOnlyPreviewFile"/>或它的集合
    /// </summary>
    Direct,
    /// <summary>
    /// 递归封装可预览文件，
    /// 递归的定义是：它本身不封装，
    /// 但是它的子属性封装了<see cref="IHasReadOnlyPreviewFile"/>或它的集合
    /// </summary>
    Recursion,
    /// <summary>
    /// 指示这个类型本身不封装可预览文件，
    /// 但是它的已知派生类型封装了可预览文件
    /// </summary>
    Offspring,
    /// <summary>
    /// 指示这个类型本身不封装可预览文件，
    /// 但是它是一个集合，集合的元素类型直接封装了可预览文件，
    /// 注意，它指的不是可预览文件的集合
    /// </summary>
    Collections
}
