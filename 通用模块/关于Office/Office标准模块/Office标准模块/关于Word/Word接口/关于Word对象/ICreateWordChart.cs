using System.DrawingFrancis.Chart;
using System.Office.Chart;

namespace System.Office.Word
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来创建Word图表
    /// </summary>
    public interface ICreateWordChart : ICreateChart
    {
        #region 创建折线图
        /// <summary>
        /// 创建一个Word折线图，并将其添加到文档的末尾
        /// </summary>
        /// <returns></returns>
        new IWordParagraphObj<IOfficeChartLine> CreateLineChart();
        #endregion
    }
}
