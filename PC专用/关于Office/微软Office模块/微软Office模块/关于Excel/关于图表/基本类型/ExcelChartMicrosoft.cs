using System.MathFrancis;

using Microsoft.Office.Interop.Excel;

namespace System.Office;

/// <summary>
/// 这个类型是底层使用微软COM组件实现的Excel图表对象
/// </summary>
/// <param name="shape">封装的形状对象</param>
sealed class ExcelChartMicrosoft(Shape shape) : IOfficeChart
{
    #region 返回图表区
    public IOfficeChartArea Area { get; }
        = new ExcelChartAreaMicrosoft(shape.Chart.ChartArea);
    #endregion
    #region 返回绘图区
    public IOfficeChartDraw Draw { get; }
        = new ExcelChartDrawMicrosoft(shape.Chart.PlotArea);
    #endregion
    #region 未实现的成员
    public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IOfficeChartSeriesManage Series => throw new NotImplementedException();
    public IPoint<double> Pos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ISize<double> Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool InTextTop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public double Rotation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    #endregion
}
