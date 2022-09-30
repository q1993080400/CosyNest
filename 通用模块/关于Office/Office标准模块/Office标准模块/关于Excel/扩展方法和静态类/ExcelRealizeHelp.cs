using System.Maths.Plane;
using System.Office.Realize;
using System.Reflection;

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
    /// <param name="sheets">要检查的工作薄的工作表容器</param>
    /// <param name="sheetName">要检查的工作表名称</param>
    /// <param name="renamed">一个用于修改工作表名，使其不重名的委托，
    /// 它的第一个参数是旧名称，第二个参数是尝试失败的次数，从2开始，返回值就是新的名称，
    /// 如果为<see langword="null"/>，则使用一个默认方法</param>
    /// <returns>新的工作表名称，保证不重复</returns>
    public static string SheetRepeat(IExcelSheetCollection sheets, string sheetName, Func<string, int, string>? renamed = null)
    {
        renamed ??= (x, y) => $"{x}({y})";
        return sheets.Select(x => x.Name).Distinct(sheetName, renamed);
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
    #region 关于格式
    #region 复制格式
    #region 缓存属性
    /// <summary>
    /// 这个属性缓存<see cref="IRangeStyle"/>中所有公开，
    /// 而且既能读取又能写入的属性，为复制格式提供方便
    /// </summary>
    private static IEnumerable<PropertyInfo> CacheStyleProperty { get; }
    = typeof(IRangeStyle).GetProperties().
        Where(x => x.IsAlmighty()).ToArray();
    #endregion
    #region 复制单元格格式
    /// <summary>
    /// 复制单元格格式
    /// </summary>
    /// <param name="source">待复制的格式</param>
    /// <param name="target">复制的目标格式</param>
    public static void CopyStyle(IRangeStyle source, IRangeStyle target)
        => CacheStyleProperty.ForEach(x => x.Copy(source, target));
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
    public static (int BeginRow, int BeginColumn, int EndRow, int EndColumn) GetPosition(ISizePosPixel rectangle)
    {
        var (h, v) = rectangle;
        var (r, t) = rectangle.FirstPixel.Abs();
        return GetPosition(t, r, (v, h));
    }
    #endregion
    #endregion
    #endregion
    #region 估算打印时间
    #region 传入纸张数量
    /// <summary>
    /// 估算打印完成所需要的时间
    /// </summary>
    /// <param name="count">打印的纸张数量</param>
    /// <param name="isPrintFromPaper">如果这个值为<see langword="true"/>，
    /// 则打印到纸张，否则为打印到文件</param>
    /// <returns></returns>
    public static Task EstimatedPrintingTime(int count, bool isPrintFromPaper)
    {
        var time = 100 * count;
        return Task.Delay(300 + (isPrintFromPaper ? time * 5 : time));
    }

    /*算法如下：
      300毫秒是与打印机通讯的延迟
      打印到纸张是物理打印，较慢，所以打印每张纸的时间需增加5倍*/
    #endregion
    #region 传入起始页和份数
    /// <param name="begin">起始页</param>
    /// <param name="end">结束页</param>
    /// <param name="num">打印份数</param>
    /// <inheritdoc cref="EstimatedPrintingTime(int, bool)"/>
    public static Task EstimatedPrintingTime(int begin, int end, int num, bool isPrintFromPaper)
        => EstimatedPrintingTime((end - begin + 1) * num, isPrintFromPaper);
    #endregion
    #endregion
}
