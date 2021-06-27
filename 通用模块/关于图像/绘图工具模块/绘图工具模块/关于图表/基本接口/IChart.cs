using System.Maths;
using System.DataFrancis;
using System.Collections.Generic;

namespace System.DrawingFrancis.Chart
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视为一张图表
    /// </summary>
    public interface IChart
    {
        #region 图表的大小
        /// <summary>
        /// 获取或设置图表的大小
        /// </summary>
        ISize Size { get; set; }
        #endregion
        #region 图表的标题
        /// <summary>
        /// 获取或设置图表的标题
        /// </summary>
        string Title { get; set; }
        #endregion
        #region 图表的数据源
        /// <summary>
        /// 获取或设置图表的数据源，
        /// 如果目前没有数据，
        /// 或者数据是通过其他方式获取的，则为<see langword="null"/>
        /// </summary>
        IEnumerable<IData>? Source { get; set; }

        /*问：上文所述“数据通过其他方式获取”是什么意思？
          答：某些平台不使用IData存储数据，而是拥有特殊的数据存储方式，
          举例说明，Excel表格可以从单元格获取数据，根据约定：
          如果一个实现支持多种获取数据的方式，那么Source的优先级最高*/
        #endregion
    }
}
