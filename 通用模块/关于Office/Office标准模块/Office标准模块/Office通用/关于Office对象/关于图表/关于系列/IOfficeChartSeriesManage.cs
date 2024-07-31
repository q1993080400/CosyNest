namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理Excel图表系列
/// </summary>
public interface IOfficeChartSeriesManage : IReadOnlyCollection<IOfficeChartSeries>
{
    #region 添加系列
    /// <summary>
    /// 添加一个空白系列
    /// </summary>
    /// <returns></returns>
    IOfficeChartSeries Add();
    #endregion
}
