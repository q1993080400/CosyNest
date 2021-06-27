using System.DrawingFrancis.Chart;
using System.Office.Chart;

namespace System.Office.Excel.Realize
{
    /// <summary>
    /// 在实现<see cref="ICreateExcelChart"/>的时候，
    /// 可以继承自本类型，以减少重复的工作
    /// </summary>
    public abstract class CreateChartExcel : ICreateExcelChart
    {
        #region 创建折线图
        #region 隐式实现
        public abstract IExcelObj<IOfficeChartLine> CreateLineChart();
        #endregion
        #region 显式实现
        IChartLine ICreateChart.CreateLineChart()
            => CreateLineChart().Content;
        #endregion 
        #endregion
    }
}
