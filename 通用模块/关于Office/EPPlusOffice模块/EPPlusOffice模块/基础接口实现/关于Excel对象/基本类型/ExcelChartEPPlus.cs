using System.MathFrancis.Plane;

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
    #region 未实现的成员
    public IPoint Pos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ISize Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool InTextTop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IOfficeChartArea Area => throw new NotImplementedException();
    public IOfficeChartDraw Draw => throw new NotImplementedException();
    public double Rotation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    #endregion
}
