using System.DrawingFrancis.Chart;

namespace System.Office.Chart
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Office折线图
    /// </summary>
    public interface IOfficeChartLine : IOfficeChart, IChartLine
    {
    }
}
