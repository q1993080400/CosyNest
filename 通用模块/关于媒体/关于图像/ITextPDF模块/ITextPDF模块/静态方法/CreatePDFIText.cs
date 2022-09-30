using System.IOFrancis;
using System.IOFrancis.FileSystem;

using iText.Kernel.Pdf;

namespace System.Media.Drawing.PDF;

/// <summary>
/// 该静态类可以用来创建底层使用IText实现的PDF
/// </summary>
public static class CreatePDFIText
{
    #region 创建PDF文档
    #region 通过路径
    /// <summary>
    /// 通过路径，创建一个PDF文档
    /// </summary>
    /// <inheritdoc cref="DocumentIText(PathText)"/>
    public static IPDFDocument PDF(PathText path)
    {
        if (!File.Exists(path))
        {
            using var create = CreateIO.FileStream(path);
            new PdfDocument(new PdfWriter(create)).Close();
        }
        using var file = new FileStream(path, FileMode.Open);
        return new DocumentIText(file, path);
    }
    #endregion
    #region 通过流
    /// <summary>
    /// 通过流，创建一个PDF文档
    /// </summary>
    /// <inheritdoc cref="DocumentIText(Stream)"/>
    public static IPDFDocument PDF(Stream stream)
         => new DocumentIText(stream, null);
    #endregion
    #endregion
}
