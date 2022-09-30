using iText.Kernel.Pdf;

namespace System.Media.Drawing.PDF;

/// <summary>
/// 该类型代表PDF中的一页
/// </summary>
sealed class PageIText : IPDFPage
{
    #region 封装的对象
    #region PDF页
    /// <summary>
    /// 获取封装的PDF页，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal PdfPage Page { get; }
    #endregion
    #region PDF对象
    /// <summary>
    /// 获取页所在的PDF对象
    /// </summary>
    internal DocumentIText PDF { get; }
    #endregion
    #endregion
    #region PDF文档
    IPDFDocument IPDFPage.PDF => PDF;
    #endregion
    #region 构造函数
    public PageIText(PdfPage page, DocumentIText pdf)
    {
        this.Page = page;
        this.PDF = pdf;
    }
    #endregion
}
