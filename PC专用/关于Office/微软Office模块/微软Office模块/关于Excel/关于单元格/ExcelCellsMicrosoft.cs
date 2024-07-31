using System.Office.Excel.Realize;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个对象代表通过微软COM组件实现的Excel单元格
/// </summary>
/// <param name="range">封装的Excel单元格对象，
/// 本对象的功能就是通过它实现的</param>
/// <inheritdoc cref="ExcelCells(IExcelSheet)"/>
sealed class ExcelCellsMicrosoft(ExcelSheetMicrosoft sheet, MSExcelRange range) : ExcelCells(sheet)
{
    #region 公开成员
    #region 复制单元格为图片
    public override async Task CopyPictureToClipboard()
    {
        range.CopyPicture(XlPictureAppearance.xlPrinter);
        await Task.Delay(50);
    }
    #endregion
    #endregion
    #region 未实现的成员
    public override RangeValue Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public override string? FormulaA1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public override string? FormulaR1C1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public override string? Link { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address => throw new NotImplementedException();

    public override IExcelCells CopyTo(IExcelCells cells)
    {
        throw new NotImplementedException();
    }

    public override void Replace(string content, string replace)
    {
        throw new NotImplementedException();
    }

    public override bool IsMerge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected override IExcelCells IndexTemplate(int beginRow, int beginColumn, int endRow, int endColumn)
    {
        throw new NotImplementedException();
    }

    public override IRangeStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    #endregion 
}
