using System.Maths.Plane;
using System.Office.Chart;

using Microsoft.Office.Interop.Excel;

using ISeries = System.Office.Chart.ISeries;

namespace System.Office.Excel.Chart;

/// <summary>
/// 这个类型是所有Excel图表的基类
/// </summary>
class ExcelChartBase : IOfficeChart
{
    #region 封装的对象
    #region Excel形状
    /// <summary>
    /// 获取被封装的Excel形状
    /// </summary>
    protected Shape PackShape { get; }
    #endregion
    #region Excel图表
    /// <summary>
    /// 获取被封装的Excel图表
    /// </summary>
    protected Microsoft.Office.Interop.Excel.Chart PackChart
        => PackShape.Chart;
    #endregion
    #region 所在工作表
    /// <summary>
    /// 获取图表所在的工作表
    /// </summary>
    private IExcelSheet Sheet { get; }
    #endregion
    #endregion
    #region 设置数据源
    public void SetValue(IExcelCells value)
        => PackChart.SetSourceData(value.To<ExcelCellsMicrosoft>().PackRange);
    #endregion
    #region 获取或设置图表的大小
    public ISize Size
    {
        get => PackShape.GetSize();
        set => PackShape.SetSize(value);
    }
    #endregion
    #region 获取或设置图表的标题
    public string Title
    {
        get => PackChart.ChartTitle.Caption;
        set => PackChart.ChartTitle.Caption = value;
    }
    #endregion
    #region 枚举所有系列
    public IEnumerable<ISeries> Seriess
    {
        get
        {
            foreach (Microsoft.Office.Interop.Excel.Series item in PackChart.SeriesCollection())
            {
                yield return new Series(item, Sheet);
            }
        }
    }
    #endregion
    #region 创建系列
    public ISeries CreateSeries()
    {
        SeriesCollection seriess = PackChart.SeriesCollection();
        return new Series(seriess.NewSeries(), Sheet);
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的形状对象封装进图表
    /// </summary>
    /// <param name="packShape">被封装的形状对象</param>
    public ExcelChartBase(Shape packShape, IExcelSheet sheet)
    {
        this.PackShape = packShape;
        this.Sheet = sheet;
    }
    #endregion
}
