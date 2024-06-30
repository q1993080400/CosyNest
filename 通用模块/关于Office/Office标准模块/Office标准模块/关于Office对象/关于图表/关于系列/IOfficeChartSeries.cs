namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office图表中的系列，
/// 系列可以用来显示数据
/// </summary>
public interface IOfficeChartSeries
{
    #region 系列名称
    /// <summary>
    /// 获取系列名称，
    /// 它可以是一个常量，也可以是单元格引用
    /// </summary>
    string Name { get; set; }
    #endregion
    #region X轴数据源
    /// <summary>
    /// 获取X轴数据源
    /// </summary>
    string XData { get; set; }
    #endregion
    #region Y轴数据源
    /// <summary>
    /// 获取Y轴数据源
    /// </summary>
    string YData { get; set; }
    #endregion
}
