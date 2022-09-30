using System.Office.Excel;

namespace System.Office.Chart;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个图表系列，它代表图表上的一个数据轴
/// </summary>
public interface ISeries
{
    #region 系列名称
    /// <summary>
    /// 获取或设置系列的名称
    /// </summary>
    string Name { get; set; }
    #endregion
    #region 删除系列
    /// <summary>
    /// 删除系列
    /// </summary>
    void Delete();
    #endregion
    #region X轴数据
    /// <summary>
    /// 获取或设置X轴数据
    /// </summary>
    IExcelCells? X { get; set; }
    #endregion
    #region Y轴数据
    /// <summary>
    /// 获取或设置Y轴数据
    /// </summary>
    IExcelCells? Y { get; set; }
    #endregion
}
