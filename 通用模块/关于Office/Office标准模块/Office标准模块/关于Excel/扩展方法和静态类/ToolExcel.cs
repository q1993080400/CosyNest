using System.Maths;

using static System.Maths.CreateMath;
using static System.Maths.ToolArithmetic;

namespace System.Office.Excel
{
    /// <summary>
    /// 有关Excel的工具类
    /// </summary>
    public static class ToolExcel
    {
        #region 创建数学地址
        #region 绝对地址
        /// <summary>
        /// 指定开始和结束单元格的行列数，
        /// 并返回该单元格的数学地址
        /// </summary>
        /// <param name="beginRow">起始单元格的行数</param>
        /// <param name="beginColumn">起始单元格的列数</param>
        /// <param name="endRow">结束单元格的行数，
        /// 如果为<see langword="null"/>，则与<paramref name="beginRow"/>相同</param>
        /// <param name="endColumn">结束单元格的列数，
        /// 如果为<see langword="null"/>，则与<paramref name="beginColumn"/>相同</param>
        /// <returns></returns>
        public static ISizePosPixel AddressMathAbs(int beginRow, int beginColumn, int? endRow = null, int? endColumn = null)
        {
            var er = endRow ?? beginRow;
            var ec = endColumn ?? beginColumn;
            ExceptionIntervalOut.Check(0, null, beginColumn, ec);
            return SizePosPixel(Point(beginColumn, Abs(beginRow)), Point(ec, Abs(er)));
        }
        #endregion
        #region 相对地址
        /// <summary>
        /// 指定开始单元格的行列数，
        /// 以及单元格行数量和列数量,
        /// 并返回该单元格的数学地址
        /// </summary>
        /// <param name="beginRow">起始单元格的行数</param>
        /// <param name="beginColumn">起始单元格的列数</param>
        /// <param name="rowCount">单元格的行数量</param>
        /// <param name="columnCount">单元格的列数量</param>
        /// <returns></returns>
        public static ISizePosPixel AddressMathRel(int beginRow, int beginColumn, int rowCount = 1, int columnCount = 1)
        {
            ExceptionIntervalOut.Check(0, null, beginRow);
            ExceptionIntervalOut.Check(1, null, rowCount, columnCount);
            return SizePosPixel(beginColumn, Abs(beginRow), columnCount, rowCount);
        }
        #endregion
        #endregion
    }
}
