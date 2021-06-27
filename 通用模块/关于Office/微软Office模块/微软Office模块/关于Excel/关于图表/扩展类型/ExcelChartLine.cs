using Microsoft.Office.Interop.Excel;

using System.Office.Chart;

namespace System.Office.Excel.Chart
{
    /// <summary>
    /// 这个类型代表一个Excel折线图
    /// </summary>
    class ExcelChartLine : ExcelChartBase, IOfficeChartLine
    {
        #region 构造函数
        /// <inheritdoc cref="ExcelChartBase(Shape)"/>
        public ExcelChartLine(Shape packShape)
            : base(packShape)
        {

        }
        #endregion 
    }
}
