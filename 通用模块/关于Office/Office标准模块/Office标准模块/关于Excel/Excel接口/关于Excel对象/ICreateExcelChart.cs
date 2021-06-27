using System.DrawingFrancis.Chart;
using System.Office.Chart;

namespace System.Office.Excel
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来创建Excel图表
    /// </summary>
    public interface ICreateExcelChart : ICreateChart
    {
        #region 创建折线图
        /// <summary>
        /// 创建一个Excel折线图
        /// </summary>
        /// <returns>新创建的折线图</returns>
        new IExcelObj<IOfficeChartLine> CreateLineChart();
        #endregion
    }
}
