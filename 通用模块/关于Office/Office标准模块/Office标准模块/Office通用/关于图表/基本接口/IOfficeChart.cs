using System.Office.Excel;

namespace System.Office.Chart;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office图表
/// </summary>
public interface IOfficeChart
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #Excel图表可以被添加到Word中，
      但是Word图表不一定可以添加到Excel中，
      这样可以和通常操作Office的习惯相契合*/
    #endregion
    #region 获取这个接口本身
    /// <summary>
    /// 获取这个接口本身，仅供辅助实现使用
    /// </summary>
    private protected IOfficeChart Base => this;
    #endregion
    #region 系列集合
    /// <summary>
    /// 获取一个枚举所有系列的枚举器
    /// </summary>
    IEnumerable<ISeries> Seriess { get; }
    #endregion
    #region 创建空白系列
    /// <summary>
    /// 创建空白系列，并返回
    /// </summary>
    /// <returns></returns>
    ISeries CreateSeries();
    #endregion
    #region 设置数据区域
    /// <summary>
    /// 设置数据区域，它是设置系列的简略方法
    /// </summary>
    /// <param name="value">待写入的数据区域</param>
    void SetValue(IExcelCells value);
    #endregion
    #region 图表的标题
    /// <summary>
    /// 获取或设置图表的标题
    /// </summary>
    string Title { get; set; }
    #endregion
}
