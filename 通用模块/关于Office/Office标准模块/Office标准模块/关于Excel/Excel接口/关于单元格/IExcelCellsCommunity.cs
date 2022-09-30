using System.Maths.Plane;
using System.Office.Excel.Realize;

namespace System.Office.Excel;

/// <summary>
/// 该接口封装了<see cref="IExcelCells"/>与<see cref="IExcelSheet"/>的共同API部分
/// </summary>
public interface IExcelCellsCommunity
{
    #region 返回子单元格的索引器
    #region 根据绝对位置
    /// <summary>
    /// 根据起始行列号和结束行列号，
    /// 返回一个或多个单元格
    /// </summary>
    /// <param name="beginRow">开始单元格的行号</param>
    /// <param name="beginColumn">开始单元格的列号</param>
    /// <param name="endRow">结束单元格的行号，如果小于0，代表和起始单元格相同</param>
    /// <param name="endColumn">结束单元格的列号，如果为小于0，代表和起始单元格相同</param>
    /// <returns></returns>
    IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1] { get; }

    /*实现本API请遵循以下规范：
      1.所有行列号从0开始，而不是从1开始，这是为了与C#的习惯相统一
      2.之所以强制实现这个索引器，是因为绝对行列号实现起来容易一些
      3.如果beginRow或beginColumn为负，则取它们的绝对值，
      这是因为要抹平数学坐标和UI坐标之间的差异，在Excel中这两种坐标都可以正常工作*/
    #endregion
    #region 根据相对位置
    /// <summary>
    /// 根据起始行列号和单元格大小，返回一个或多个单元格
    /// </summary>
    /// <param name="beginRow">起始单元格的行号</param>
    /// <param name="beginColumn">起始单元格的列号</param>
    /// <param name="size">这个元组指示单元格的行数和列数</param>
    /// <returns></returns>
    IExcelCells this[int beginRow, int beginColumn, (int RowCount, int ColumnCount) size]
    {
        get
        {
            var (br, bc, er, ec) = ExcelRealizeHelp.GetPosition(beginRow, beginColumn, size);
            return this[br, bc, er, ec];
        }
    }
    #endregion
    #region 根据坐标
    /// <summary>
    /// 根据一个坐标，返回单个单元格
    /// </summary>
    /// <param name="point">单元格的坐标</param>
    /// <returns></returns>
    IExcelCells this[IPoint point]
    {
        get
        {
            var (r, t) = point;
            return this[t, r];
        }
    }
    #endregion
    #region 根据平面
    /// <summary>
    /// 根据一个平面，返回一个单元格
    /// </summary>
    /// <param name="rectangle">这个平面被用来描述单元格的大小和位置，
    /// 如果它的坐标有负数，那么会取绝对值</param>
    /// <returns></returns>
    IExcelCells this[ISizePosPixel rectangle]
    {
        get
        {
            var (br, bc, er, ec) = ExcelRealizeHelp.GetPosition(rectangle);
            return this[br, bc, er, ec];
        }
    }
    #endregion
    #region 根据A1地址
    /// <summary>
    /// 根据A1地址，返回单元格
    /// </summary>
    /// <param name="addressA1">单元格的A1地址</param>
    /// <returns></returns>
    IExcelCells this[string addressA1]
    {
        get
        {
            var (br, bc, er, ec) = ExcelRealizeHelp.AddressToTupts(addressA1);
            return this[br, bc, er, ec];
        }
    }
    #endregion
    #endregion
    #region 搜索单元格
    /// <summary>
    /// 搜索具有指定值或公式的单元格，
    /// 它通常具有更高的性能
    /// </summary>
    /// <param name="content">要搜索的内容</param>
    /// <param name="findValue">如果这个值为<see langword="true"/>，
    /// 则搜索值，否则搜索公式</param>
    /// <returns>搜索到的单元格</returns>
    IEnumerable<IExcelCells> Find(string content, bool findValue = true);
    #endregion
    #region 替换单元格
    /// <summary>
    /// 替换单元格
    /// </summary>
    /// <param name="content">要替换的内容</param>
    /// <param name="replace">替换后的新内容</param>
    void Replace(string content, string replace);
    #endregion
}
