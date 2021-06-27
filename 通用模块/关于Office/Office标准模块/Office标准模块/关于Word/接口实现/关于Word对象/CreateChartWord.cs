using System.DrawingFrancis.Chart;
using System.Office.Chart;

namespace System.Office.Word.Realize
{
    /// <summary>
    /// 在实现<see cref="ICreateWordChart"/>时，
    /// 可以继承自本类型，以减少重复的工作
    /// </summary>
    public abstract class CreateChartWord : ICreateWordChart
    {
        #region 创建折线图
        #region 显式实现
        public abstract IWordParagraphObj<IOfficeChartLine> CreateLineChart();
        #endregion
        #region 隐式实现
        IChartLine ICreateChart.CreateLineChart()
            => CreateLineChart().Content;
        #endregion
        #endregion
    }
}
