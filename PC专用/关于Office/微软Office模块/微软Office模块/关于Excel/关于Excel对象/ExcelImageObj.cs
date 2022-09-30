
using System.Media.Drawing;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个对象代表一个Excel图片
/// </summary>
class ExcelImageObj : ExcelObj<IImage>
{
    #region Excel对象的内容
    public override IImage Content
        => throw new NotImplementedException("由于底层API限制，不支持此操作");
    #endregion
    #region 复制图像
    #region 复制到工作表
    public override IExcelObj<IImage> Copy(IExcelSheet target)
        => target == Sheet ?
            new ExcelImageObj(Sheet, PackShape.Duplicate()) :
            throw new NotSupportedException("由于底层API限制，只支持将图像复制到本工作表");
    #endregion
    #region 复制到文档
    public override Word.IWordParagraphObj<IImage> Copy(Word.IWordDocument target, Index? pos = null)
        => throw new NotSupportedException("不支持此API");
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelObj{Obj}.ExcelObj(IExcelSheet, Shape)"/>
    public ExcelImageObj(IExcelSheet sheet, Shape packShape)
        : base(sheet, packShape)
    {
        if (!packShape.IsImage())
            throw new ArgumentException("这个形状对象不是图片");
    }
    #endregion
}
