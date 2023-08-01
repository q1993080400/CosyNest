using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;

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
    #region 枚举所有图片
    public IEnumerable<IPDFImage> Images
    {
        get
        {
            var xObjects = Page.GetPdfObject().GetAsDictionary(PdfName.Resources).GetAsDictionary(PdfName.XObject);
            var keys = xObjects.KeySet();
            foreach (var item in keys)
            {
                var stream = xObjects.GetAsStream(item);
                if (stream is null)
                    continue;
                var subType = stream.Get(PdfName.Subtype);
                if (subType is null || subType.ToString() != PdfName.Image.ToString())
                    continue;
                var image = new PdfImageXObject(stream);
                if (image.GetHeight() is <= 100 || image.GetWidth() is <= 100)
                    continue;
                yield return new PDFImageIText(image);

            }
        }
    }
    #endregion
    #region 构造函数
    public PageIText(PdfPage page, DocumentIText pdf)
    {
        this.Page = page;
        this.PDF = pdf;
    }
    #endregion
}
