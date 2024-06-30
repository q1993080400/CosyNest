using OfficeOpenXml.Drawing.Chart;

namespace System.Office.Excel;

/// <summary>
/// 使用EPPlus实现的Excel图表系列
/// </summary>
/// <param name="excelChartSerie">封装的系列对象</param>
sealed class ExcelChartSeriesEPPlus(ExcelChartSerie excelChartSerie) : IOfficeChartSeries
{
    #region 公开成员
    #region 系列名称
    public string Name
    {
        get => excelChartSerie.Header;
        set => excelChartSerie.Header = value;
    }
    #endregion
    #region X轴数据源
    public string XData
    {
        get => excelChartSerie.XSeries;
        set => excelChartSerie.XSeries = value;
    }
    #endregion
    #region Y轴数据源
    public string YData
    {
        get => excelChartSerie.Series;
        set => excelChartSerie.Series = value;
    }
    #endregion
    #endregion 
}
