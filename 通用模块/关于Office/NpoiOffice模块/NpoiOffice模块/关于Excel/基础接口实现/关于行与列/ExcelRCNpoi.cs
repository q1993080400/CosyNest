using System.Office.Excel.Realize;

using NPOI.SS.UserModel;

namespace System.Office.Excel;

/// <summary>
/// 该类型是使用Npoi实现的Excel行与列的基类
/// </summary>
abstract class ExcelRCNpoi : ExcelRC
{
    #region Npoi格式的工作表
    /// <summary>
    /// 返回Npoi格式的工作表
    /// </summary>
    protected ISheet SheetNpoi
        => Sheet.To<ExcelSheetNpoi>().Sheet;
    #endregion
    #region 未实现的成员
    public override double? HeightOrWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override IRangeStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override void AutoFit()
    {
        throw new NotImplementedException();
    }

    public override void Delete()
    {
        throw new NotImplementedException();
    }
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelRC(IExcelSheet, bool, int, int)"/>
    public ExcelRCNpoi(IExcelSheet sheet, bool isRow, int begin, int end)
        : base(sheet, isRow, begin, end)
    {

    }
    #endregion 
}
