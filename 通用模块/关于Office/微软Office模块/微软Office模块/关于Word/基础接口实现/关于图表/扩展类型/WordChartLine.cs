using System.Office.Chart;

using Microsoft.Office.Interop.Word;

namespace System.Office.Word.Chart
{
    /// <summary>
    /// 这个类型代表一个Word折线图
    /// </summary>
    class WordChartLine : WordChartBase, IOfficeChartLine
    {
        #region 构造函数
        /// <inheritdoc cref="WordChartBase(InlineShape)"/>
        public WordChartLine(InlineShape packShape)
            : base(packShape)
        {

        }
        #endregion
    }
}
