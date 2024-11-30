namespace System.DataFrancis;

/// <summary>
/// 这个枚举指示一个属性是否封装了可预览文件
/// </summary>
public enum HasPreviewFilePropertyNature
{
    /// <summary>
    /// 该属性不是封装可预览文件的属性
    /// </summary>
    NotPreviewFile,
    /// <summary>
    /// 该属性是一个封装单个可预览文件的属性
    /// </summary>
    HasPreviewFile,
    /// <summary>
    /// 该属性是一个封装可预览文件集合的属性
    /// </summary>
    HasPreviewFileCollections
}
