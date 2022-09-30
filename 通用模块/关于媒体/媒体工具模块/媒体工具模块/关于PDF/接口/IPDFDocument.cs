using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace System.Media.Drawing.PDF;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个PDF文档
/// </summary>
public interface IPDFDocument : IFromIO
{
    #region PDF的文件类型
    /// <summary>
    /// 返回PDF文件类型
    /// </summary>
    public static IFileType FileTypePDF { get; }
    = CreateIO.FileType("PDF文件格式", "pdf");
    #endregion
    #region 合并文档
    /// <summary>
    /// 将另一个文档与本文档合并，
    /// 它的所有页会加入本文档
    /// </summary>
    /// <param name="document">待合并的另一个文档</param>
    /// <param name="pos">指定将<paramref name="document"/>插入本文档的位置，
    /// 单位是页数，从0开始，如果为<see langword="null"/>，默认放在最后</param>
    void Merge(IPDFDocument document, Index? pos = null)
        => document.Pages.CopyTo(Pages, null, pos);
    #endregion
    #region 枚举所有页面
    /// <summary>
    /// 枚举PDF文档中的所有页面
    /// </summary>
    IPDFCollect Pages { get; }
    #endregion
}
