namespace System.Office.Chart;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office折线图
/// </summary>
public interface IOfficeChartLine : IOfficeChart
{
    #region 折线图变种
    /// <summary>
    /// 获取或设置折线图的变种
    /// </summary>
    LineVariant Variant { get; set; }
    #endregion
}

#region 折线图变种
/// <summary>
/// 这个枚举表示折线图的变种
/// </summary>
public enum LineVariant
{
    /// <summary>
    /// 普通折线图
    /// </summary>
    Line,
    /// <summary>
    /// 带标记的堆积线条
    /// </summary>
    Markers,
    /// <summary>
    /// 带标记的堆积线条
    /// </summary>
    MarkersStacked,
    /// <summary>
    /// 百分比堆积折线标记
    /// </summary>
    MarkersStacked100,
    /// <summary>
    /// 堆积线条
    /// </summary>
    Stacked,
    /// <summary>
    /// 百分比堆积折线图
    /// </summary>
    Stacked100
}
#endregion
