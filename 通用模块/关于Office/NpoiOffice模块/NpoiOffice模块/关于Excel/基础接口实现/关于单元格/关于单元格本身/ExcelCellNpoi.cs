using System.MathFrancis.Plane;
using System.Office.Excel.Realize;

using NPOI.SS.UserModel;

namespace System.Office.Excel;

/// <summary>
/// 该类型是所有用Npoi实现的单元格的基类
/// </summary>
/// <inheritdoc cref="ExcelCells(IExcelSheet)"/>
abstract class ExcelCellNpoi(IExcelSheet sheet) : ExcelCells(sheet)
{
    #region 工作表对象
    /// <summary>
    /// 获取封装的Npoi工作表对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected ISheet SheetNpoi
        => Sheet.To<ExcelSheetNpoi>().Sheet;
    #endregion
    #region 未实现的成员
    public override string? Link { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override ISizePos VisualPosition => throw new NotImplementedException();

    public override IExcelCells Copy(IExcelCells cells)
    {
        throw new NotImplementedException();
    }

    public override string? FormulaR1C1
    {
        get => null;
        set => throw new NotImplementedException();
    }

    public override IRangeStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    #endregion
    #region 构造函数
    #endregion
}
