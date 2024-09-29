using System.Drawing;

namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office图表的图表区
/// </summary>
public interface IOfficeChartArea
{
    #region 图表区填充
    /// <summary>
    /// 获取或设置图表区填充
    /// </summary>
    Color? Fill { get; set; }
    #endregion
}
