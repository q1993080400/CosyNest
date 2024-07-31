namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office图表
/// </summary>
public interface IOfficeChart : IOfficeObject
{
    #region 标题
    /// <summary>
    /// 获取或设置图表的标题
    /// </summary>
    string Title { get; set; }
    #endregion
    #region 返回图表所有的系列
    /// <summary>
    /// 返回一个对象，它可以用来管理图表的系列
    /// </summary>
    IOfficeChartSeriesManage Series { get; }
    #endregion
    #region 返回图表区
    /// <summary>
    /// 返回一个代表图表区的对象
    /// </summary>
    IOfficeChartArea Area { get; }
    #endregion
    #region 返回绘图区
    /// <summary>
    /// 返回一个代表绘图区的对象
    /// </summary>
    IOfficeChartDraw Draw { get; }
    #endregion
}
