namespace System.DrawingFrancis.Chart
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来创建图表
    /// </summary>
    public interface ICreateChart
    {
        #region 创建折线图
        /// <summary>
        /// 创建一个折线图，并返回
        /// </summary>
        /// <returns></returns>
        IChartLine CreateLineChart();
        #endregion
    }
}
