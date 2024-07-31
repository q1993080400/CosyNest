using System.Office.Excel;
using System.Office.Excel.Realize;

using Microsoft.Office.Interop.Excel;

namespace System.Office;

/// <summary>
/// 这个对象是底层使用微软COM对象的<see cref="IExcelSheet"/>的实现，
/// 可以视为一个Excel工作表
/// </summary>
/// <param name="worksheet">封装的Excel工作表</param>
/// <inheritdoc cref="ExcelSheet(IExcelSheetManage)"/>
sealed class ExcelSheetMicrosoft(ExcelSheetManageMicrosoft sheetManage, Worksheet worksheet) : ExcelSheet(sheetManage)
{
    #region 公开成员
    #region 工作表名称
    public override string Name
    {
        get => worksheet.Name;
        set => worksheet.Name = value;
    }
    #endregion
    #region 工作表索引
    public override int Index
    {
        get => worksheet.Index - 1;
        set => throw new NotImplementedException();
    }
    #endregion
    #region 页面管理对象
    public override ISheetPage Page
        => new ExcelPageSheetMicrosoft(this, worksheet);
    #endregion
    #region 关于单元格
    #region 返回所有单元格
    public override IExcelCells Cell
    {
        get
        {
            var (br, bc, er, ec) = worksheet.UsedRange.GetAddress();
            var range = worksheet.Range[ExcelRealizeHelp.GetAddress(br, bc, er, ec)];
            return new ExcelCellsMicrosoft(this, range);
        }
    }
    #endregion
    #region 根据行列号返回Range
    public override IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1]
    {
        get
        {
            var address = ExcelRealizeHelp.GetAddress(beginRow, beginColumn, endRow, endColumn);
            return new ExcelCellsMicrosoft(this, worksheet.Range[address]);
        }
    }
    #endregion
    #endregion
    #region 关于Office对象
    #region 返回图表管理对象
    public override IOfficeObjectManageCommon<IOfficeChart> ChartManage { get; } 
        = new ExcelChartManageMicrosoft(worksheet.Shapes);
    #endregion
    #endregion
    #endregion
    #region 未实现的成员
    public override IExcelRC GetRC(bool isRow, int begin, int? end)
    {
        throw new NotImplementedException();
    }

    public override void Delete()
    {
        throw new NotImplementedException();
    }

    public override IExcelSheet Copy(IExcelSheetManage? collection = null, Func<string, int, string>? renamed = null)
    {
        throw new NotImplementedException();
    }
    #endregion 
}
