namespace System.Office.Chart;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个XY散点图
/// </summary>
public interface IOfficeChartXY : IOfficeChart
{
    #region 散点图变种
    /// <summary>
    /// 获取或设置XY散点图的变种
    /// </summary>
    public XYVariant Variant { get; set; }
    #endregion
}

#region XY散点图变种
/// <summary>
/// 这个枚举表示XY散点图的变种
/// </summary>
public enum XYVariant
{
    /// <summary>
    /// 普通散点图
    /// </summary>
    XY,
    /// <summary>
    /// 折线散点图
    /// </summary>
    Lines,
    /// <summary>
    /// 带线条和无数据标记的散点图
    /// </summary>
    LinesNoMarkers,
    /// <summary>
    /// 平滑线散点图
    /// </summary>
    Smooth,
    /// <summary>
    /// 平滑线和无数据标记的散点图
    /// </summary>
    SmoothNoMarkers
}
#endregion
