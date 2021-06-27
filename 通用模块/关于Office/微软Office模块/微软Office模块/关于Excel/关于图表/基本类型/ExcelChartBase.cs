using System.Collections.Generic;
using System.DataFrancis;
using System.Maths;
using System.Office.Chart;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是所有Excel图表的基类
    /// </summary>
    class ExcelChartBase : IOfficeChart
    {
        #region 封装的对象
        #region Excel形状
        /// <summary>
        /// 获取被封装的Excel形状
        /// </summary>
        protected Shape PackShape { get; }
        #endregion
        #region Excel图表
        /// <summary>
        /// 获取被封装的Excel图表
        /// </summary>
        protected Microsoft.Office.Interop.Excel.Chart PackChart
            => PackShape.Chart;
        #endregion
        #endregion
        #region 获取或设置数据源
        #region 以单元格为数据源
        public IExcelCells? SourceFromRange
        {
            get => throw new NotImplementedException("该属性只支持写入，不支持读取");
            set => PackShape.Chart.SetSourceData(value!.To<ExcelCellsMicrosoft>().PackRange);
        }
        #endregion
        #region 以IEnumerable为数据源
        public IEnumerable<IData>? Source
        {
            get => throw new NotImplementedException("此API尚未实现");
            set => throw new NotImplementedException("此API尚未实现");
        }
        #endregion
        #endregion
        #region 获取或设置图表的大小
        public ISize Size
        {
            get => PackShape.GetSize();
            set => PackShape.SetSize(value);
        }
        #endregion
        #region 获取或设置图表的标题
        public string Title
        {
            get => PackChart.ChartTitle.Caption;
            set => PackChart.ChartTitle.Caption = value;
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 将指定的形状对象封装进图表
        /// </summary>
        /// <param name="packShape">被封装的形状对象</param>
        public ExcelChartBase(Shape packShape)
        {
            this.PackShape = packShape;
        }
        #endregion
    }
}
