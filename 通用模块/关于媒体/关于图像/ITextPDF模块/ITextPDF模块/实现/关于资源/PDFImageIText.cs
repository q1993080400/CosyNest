using System.IOFrancis;

using iText.Kernel.Pdf.Xobject;

namespace System.Media.Drawing.PDF;

/// <summary>
/// 
/// </summary>
sealed class PDFImageIText : IPDFImage
{
    #region 公开成员
    #region 保存对象
    public async Task Save(string path)
    {
        using var stream = new MemoryStream(Image.GetPdfObject().GetBytes());
        var ex = $".{Format}";
        var newPath = path.EndsWith(ex) ? path : path + ex;
        using var fileStream = CreateIO.FileStream(newPath);
        await stream.CopyToAsync(fileStream);
    }
    #endregion
    #region 图片格式
    public string Format
        => Image.IdentifyImageFileExtension().ToLower();
    #endregion
    #endregion
    #region 内部成员
    #region 封装的图片对象
    /// <summary>
    /// 获取封装的图片对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private PdfImageXObject Image { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="image">封装的图片对象，
    /// 本对象的功能就是通过它实现的</param>
    public PDFImageIText(PdfImageXObject image)
    {
        Image = image;
    }
    #endregion
}
