using OfficeOpenXml.Drawing.Chart;

namespace System.Office.Excel;

/// <summary>
/// 这个对象是所有Excel图表的基类
/// </summary>
/// <param name="chart">封装的Excel图表对象</param>
sealed class ExcelChartEPPlus(ExcelChart chart) : IOfficeChart
{
    #region 枚举所有系列
    public IEnumerable<IOfficeChartSeries> Series
        => chart.Series.Select(x => new ExcelChartSeriesEPPlus(x));
    #endregion
}
