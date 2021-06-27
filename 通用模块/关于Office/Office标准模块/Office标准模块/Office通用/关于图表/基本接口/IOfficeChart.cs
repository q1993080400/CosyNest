using System.DrawingFrancis.Chart;
using System.Office.Excel;

namespace System.Office.Chart
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Office图表
    /// </summary>
    public interface IOfficeChart : IChart
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #Excel图表可以被添加到Word中，
          但是Word图表不一定可以添加到Excel中，
          这样可以和通常操作Office的习惯相契合*/
        #endregion
        #region 作为数据源的单元格
        /// <summary>
        /// 获取或设置作为数据源的单元格，
        /// 当这个属性和<see cref="IChart.Source"/>同时设置时，后者优先级更高
        /// </summary>
        IExcelCells? SourceFromRange { get; set; }
        #endregion
    }
}
