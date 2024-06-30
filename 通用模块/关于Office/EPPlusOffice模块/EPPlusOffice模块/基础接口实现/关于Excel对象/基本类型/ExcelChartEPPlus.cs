using OfficeOpenXml.Drawing.Chart;

namespace System.Office.Excel;

/// <summary>
/// 这个对象是所有Excel图表的基类
/// </summary>
/// <param name="chart">封装的Excel图表对象</param>
sealed class ExcelChartEPPlus(ExcelChart chart) : IOfficeChart
{
    #region 标题
    public string Title
    {
        get => chart.Title.Text;
        set => chart.Title.Text = value;
    }
    #endregion
    #region 返回图表所有系列
    public IOfficeChartSeriesManage Series { get; }
        = new ExcelChartSeriesManageEPPlus(chart.Series);
    #endregion
}
