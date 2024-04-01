namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office图表
/// </summary>
public interface IOfficeChart : IOfficeObj
{
    #region 枚举所有系列
    /// <summary>
    /// 枚举图表中的所有系列
    /// </summary>
    IEnumerable<IOfficeChartSeries> Series { get; }
    #endregion
}
