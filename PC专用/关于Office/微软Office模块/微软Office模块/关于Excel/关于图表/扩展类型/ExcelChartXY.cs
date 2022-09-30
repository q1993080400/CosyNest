using System.Office.Chart;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel.Chart;

/// <summary>
/// 这个类型表示一个XY散点图
/// </summary>
class ExcelChartXY : ExcelChartBase, IOfficeChartXY
{
    #region 双向映射表
    /// <summary>
    /// 获取XY散点图的双向映射表
    /// </summary>
    public static ITwoWayMap<XYVariant, XlChartType> Map { get; }
    = CreateCollection.TwoWayMap
        ((XYVariant.Lines, XlChartType.xlXYScatterLines),
        (XYVariant.LinesNoMarkers, XlChartType.xlXYScatterLinesNoMarkers),
        (XYVariant.Smooth, XlChartType.xlXYScatterSmooth),
        (XYVariant.SmoothNoMarkers, XlChartType.xlXYScatterSmoothNoMarkers),
        (XYVariant.XY, XlChartType.xlXYScatter));
    #endregion
    #region 散点图变种
    public XYVariant Variant
    {
        get => Map.BMapA(PackChart.ChartType);
        set => PackChart.ChartType = Map.AMapB(value);
    }
    #endregion
    #region 构造函数
    public ExcelChartXY(Shape packShape, IExcelSheet sheet)
        : base(packShape, sheet)
    {
    }
    #endregion
}
