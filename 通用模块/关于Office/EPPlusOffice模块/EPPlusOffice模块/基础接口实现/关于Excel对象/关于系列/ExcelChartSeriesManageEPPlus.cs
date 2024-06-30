using System.Collections;

using OfficeOpenXml.Drawing.Chart;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是EPPlus实现的图表系列管理对象
/// </summary>
/// <param name="excelChartSerie">封装的Excel图表系列</param>
sealed class ExcelChartSeriesManageEPPlus(ExcelChartSeries<ExcelChartSerie> excelChartSerie) : IOfficeChartSeriesManage
{
    #region 添加系列
    public IOfficeChartSeries Add()
    {
        var series = excelChartSerie.Add("");
        return new ExcelChartSeriesEPPlus(series);
    }
    #endregion 
    #region 系列的数量
    public int Count
        => excelChartSerie.Count;
    #endregion
    #region 枚举所有系列
    public IEnumerator<IOfficeChartSeries> GetEnumerator()
        => excelChartSerie.Select(x => new ExcelChartSeriesEPPlus(x)).
        GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
}
