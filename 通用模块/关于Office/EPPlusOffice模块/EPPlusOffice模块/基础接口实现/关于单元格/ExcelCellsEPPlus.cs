using System.Collections.Generic;
using System.Linq;
using System.Maths;

using OfficeOpenXml;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是使用EPPlus实现的Excel单元格
    /// </summary>
    class ExcelCellsEPPlus : Realize.ExcelCells
    {
        #region 封装的Excel单元格
        /// <summary>
        /// 获取封装的Excel单元格，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private ExcelRangeBase Range { get; }
        #endregion
        #region 关于单元格
        #region 单元格的值
        public override RangeValue? Value
        {
            get => new(Range.Value);
            set => Range.Value = value?.Content;
        }
        #endregion
        #region 单元格地址
        public override (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address { get; }
        #endregion
        #region 是否合并单元格
        public override bool IsMerge
        {
            get => Range.Merge;
            set => Range.Merge = value;
        }
        #endregion
        #endregion
        #region 关于子单元格和其他单元格
        #region 枚举所有子单元格
        public override IEnumerable<IExcelCells> CellsAll
            => Range.Select(x => new ExcelCellsEPPlus(Sheet, x));
        #endregion
        #region 按索引获取子单元格
        public override IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1]
        {
            get
            {
                var range = Range switch
                {
                    ExcelRange r => r[++beginRow, ++beginColumn, endRow < 0 ? beginRow : endRow, endColumn < 0 ? beginColumn : endColumn],
                    ExcelRangeBase r when (beginRow, beginColumn, endRow, endColumn) is (0, 0, 0 or -1, 0 or -1) => r,
                    _ => throw new ArgumentException("仅有一个单元格，除非地址为(0,0,0,0)，否则无法获取子单元格")
                };
                return new ExcelCellsEPPlus(Sheet, range);
            }
        }
        #endregion
        #endregion
        #region 未实现的成员
        public override string? FormulaR1C1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string? Link { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override ISizePos VisualPosition => throw new NotImplementedException();

        protected override IExcelCells MergeRange => throw new NotImplementedException();

        public override IRangeStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion
        #region 构造函数
        /// <inheritdoc cref="Realize.ExcelCells.ExcelCells(IExcelSheet)"/>
        /// <param name="range">封装的Excel单元格，本对象的功能就是通过它实现的</param>
        public ExcelCellsEPPlus(IExcelSheet sheet, ExcelRangeBase range)
            : base(sheet)
        {
            Range = range;
            var start = Range.Start;
            var row = start.Row - 1;
            var column = start.Column - 1;
            Address = (row, column, row + Range.Rows - 1, column + Range.Columns - 1);
        }
        #endregion
    }
}
