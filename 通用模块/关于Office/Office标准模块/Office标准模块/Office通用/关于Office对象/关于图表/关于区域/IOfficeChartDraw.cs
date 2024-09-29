using System.Drawing;

namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office图表的绘图区
/// </summary>
public interface IOfficeChartDraw
{
    #region 绘图区填充
    /// <summary>
    /// 获取或设置绘图区填充
    /// </summary>
    Color? Fill { get; set; }
    #endregion
}
