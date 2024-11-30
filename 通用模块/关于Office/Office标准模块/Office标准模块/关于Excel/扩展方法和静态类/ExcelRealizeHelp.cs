using System.MathFrancis;
using System.Office.Realize;

namespace System.Office.Excel.Realize;

/// <summary>
/// 这个类型为实现Excel对象提供帮助
/// </summary>
public static partial class ExcelRealizeHelp
{
    #region 关于工作表
    #region 修改工作表名称以保证不重复
    /// <summary>
    /// 如果一个工作表名称在工作薄内已经存在，
    /// 则不断的修改它，直到没有重复的名称为止，
    /// 这个方法可以避免由于工作表重名导致的异常
    /// </summary>
    /// <param name="sheetNames">要检查的工作表名集合</param>
    /// <param name="sheetName">要检查的工作表名称</param>
    /// <param name="renamed">一个用于修改工作表名，使其不重名的委托，
    /// 它的第一个参数是旧名称，第二个参数是尝试失败的次数，从2开始，返回值就是新的名称，
    /// 如果为<see langword="null"/>，则使用一个默认方法</param>
    /// <returns>新的工作表名称，保证不重复</returns>
    public static string SheetRepeat(IEnumerable<string> sheetNames, string sheetName, Func<string, int, string>? renamed = null)
    {
        renamed ??= static (x, y) => $"{x}({y})";
        return sheetNames.ToArray().Distinct(sheetName, renamed);
    }
    #endregion
    #endregion
    #region 关于单元格
    #region 返回最大行列数
    #region 2003版
    #region 最大行数
    /// <summary>
    /// 返回Excel2003版允许的最大行数
    /// </summary>
    public const int MaxRow2003 = 65536;
    #endregion
    #region 最大列数
    /// <summary>
    /// 返回Excel2003版允许的最大列数
    /// </summary>
    public const int MaxColumn2003 = 256;
    #endregion
    #endregion
    #region 2007版
    #region 最大行数
    /// <summary>
    /// 返回Excel2007版允许的最大行数
    /// </summary>
    public const int MaxRow2007 = 1048576;
    #endregion
    #region 最大列数
    /// <summary>
    /// 返回Excel2003版允许的最大列数
    /// </summary>
    public const int MaxColumn2007 = 16384;
    #endregion
    #endregion
    #endregion
    #region 关于位置
    #region 通过相对位置获取绝对位置
    /// <summary>
    /// 通过单元格的相对位置获取绝对位置
    /// </summary>
    /// <param name="beginRow">起始单元格的行号</param>
    /// <param name="beginColumn">起始单元格的列号</param>
    /// <param name="size">这个元组指示单元格的行数和列数</param>
    /// <returns></returns>
    public static (int BeginRow, int BeginColumn, int EndRow, int EndColumn) GetPosition(int beginRow, int beginColumn, (int RowCount, int ColumnCount) size)
    {
        var (rc, cc) = size;
        return (beginRow, beginColumn, beginRow + rc - 1, beginColumn + cc - 1);
    }
    #endregion
    #region 通过数学位置获取绝对位置
    /// <summary>
    /// 根据数学位置来获取单元格的绝对位置
    /// </summary>
    /// <param name="rectangle">这个平面被用来描述单元格的大小和位置，
    /// 如果它的坐标有负数，那么会取绝对值</param>
    /// <returns></returns>
    public static (int BeginRow, int BeginColumn, int EndRow, int EndColumn) GetPosition(ISizePos<int> rectangle)
    {
        var (h, v) = rectangle.Size;
        var (r, t) = rectangle.Position.Abs();
        return GetPosition(t, r, (v, h));
    }
    #endregion
    #endregion
    #endregion
}
