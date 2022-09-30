using Microsoft.Office.Interop.Excel;

using System.Office.Chart;

namespace System.Office.Excel.Chart;

/// <summary>
/// 这个类型代表一个Excel折线图
/// </summary>
class ExcelChartLine : ExcelChartBase, IOfficeChartLine
{
    #region 双向映射表
    /// <summary>
    /// 获取折线图类型的双向映射表
    /// </summary>
    public static ITwoWayMap<LineVariant, XlChartType> Map { get; }
    = CreateCollection.TwoWayMap
        ((LineVariant.Line, XlChartType.xlLine),
        (LineVariant.Stacked100, XlChartType.xlLineStacked100),
        (LineVariant.Markers, XlChartType.xlLineMarkers),
        (LineVariant.MarkersStacked, XlChartType.xlLineMarkersStacked),
        (LineVariant.MarkersStacked100, XlChartType.xlLineMarkersStacked100),
        (LineVariant.Stacked, XlChartType.xlLineStacked));
    #endregion
    #region 折线图变种
    public LineVariant Variant
    {
        get => Map.BMapA(PackChart.ChartType);
        set => PackChart.ChartType = Map.AMapB(value);
    }
    #endregion
    #region 构造函数
    /// <inheritdoc cref="ExcelChartBase(Shape,IExcelSheet)"/>
    public ExcelChartLine(Shape packShape, IExcelSheet sheet)
        : base(packShape, sheet)
    {

    }
    #endregion
}
