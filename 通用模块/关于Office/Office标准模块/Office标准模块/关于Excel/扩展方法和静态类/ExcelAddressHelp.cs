using System.IOFrancis.FileSystem;
using System.Text.RegularExpressions;
using System.Office.Realize;
using System.MathFrancis.Plane;
using System.MathFrancis;

using static System.MathFrancis.CreateMath;

namespace System.Office.Excel.Realize;

public static partial class ExcelRealizeHelp
{
    //这个静态类专门声明和地址有关的API

    #region 匹配地址的正则表达式
    #region 匹配R1C1绝对地址
    /// <summary>
    /// 返回一个用来匹配R1C1绝对地址的正则表达式
    /// </summary>
    public static IRegex MatchR1C1Abs { get; }
    =/*language=regex*/@"^R\d+C\d+(:R\d+C\d+)?$".Op().Regex();
    #endregion
    #region 匹配A1地址
    /// <summary>
    /// 返回一个用来匹配A1地址的正则表达式
    /// </summary>
    public static IRegex MatchA1 { get; }
    =/*language=regex*/@"\$?(?<bc>[A-Z]+)\$?(?<br>\d+)(?<end>:\$?(?<ec>[A-Z]+)\$?(?<er>\d+))?".Op().Regex();
    #endregion
    #endregion
    #region 返回文本地址
    #region 将列号转换为文本形式
    /// <summary>
    /// 将列号转换为文本形式
    /// </summary>
    /// <param name="column">待转换的列号，从0开始</param>
    /// <returns>列号对应的A1格式地址</returns>
    private static string ColumnToText(int column)
    {
        ExceptionIntervalOut.Check(0, null, column);
        const int begin = 'A';
        var packIndex = ToolBit.FromDecimal(column, 26).Integer.PackIndex().ToArray();
        var count = packIndex.Length;
        return packIndex.Select(x => (char)(begin + (x.Index == 0 && count > 1 ? x.Elements - 1 : x.Elements))).Join();
    }
    #endregion
    #region 根据行列号
    /// <summary>
    /// 根据起止行列号，返回行或列的地址
    /// </summary>
    /// <param name="begin">开始行列号，从0开始</param>
    /// <param name="end">结束行列号，从0开始</param>
    /// <param name="isRow">如果这个值为<see langword="true"/>，
    /// 代表要返回地址的对象是行，否则是列</param>
    /// <param name="isA1">如果这个值为<see langword="true"/>，则返回A1地址，否则返回R1C1地址</param>
    /// <returns></returns>
    public static string GetAddressRC(int begin, int end, bool isRow, bool isA1 = true)
    {
        ExceptionIntervalOut.Check(0, null, begin, end);
        begin++;
        end++;
        return (isRow, isA1) switch
        {
            (true, true) => $"{begin}:{end}",
            (false, true) => $"{ColumnToText(begin - 1)}:{ColumnToText(end - 1)}",
            (true, false) => $"R{begin}:R{end}",
            (false, false) => $"C{begin}:C{end}"
        };
    }
    #endregion
    #region 根据坐标
    /// <summary>
    /// 根据坐标，返回地址，
    /// 注意：这里的坐标全部从0开始
    /// </summary>
    /// <param name="br">起始行号</param>
    /// <param name="bc">起始列号</param>
    /// <param name="er">结束行号，如果小于0，则与起始行号相同</param>
    /// <param name="ec">结束列号，如果小于0，则与起始列号相同</param>
    /// <param name="isA1">如果这个值为<see langword="true"/>，则返回A1地址，否则返回R1C1地址</param>
    /// <returns></returns>
    public static string GetAddress(int br, int bc, int er = -1, int ec = -1, bool isA1 = true)
    {
        ExceptionIntervalOut.Check(0, null, br, bc);
        er = er < 0 ? br : er;
        ec = ec < 0 ? bc : ec;
        var isSingle = br == er && bc == ec;
        if (isA1)
        {
            #region 本地函数
            static string FunA1(int r, int c)
                => ColumnToText(c) + (r + 1);
            #endregion
            return FunA1(br, bc) +
            (isSingle ? "" : $":{FunA1(er, ec)}");
        }
        #region 本地函数
        static string FunR1C1(int r, int c)
            => $"R{r + 1}C{c + 1}";
        #endregion
        return FunR1C1(br, bc) +
            (isSingle ? "" : $":{FunR1C1(er, ec)}");
    }
    #endregion
    #region 根据ISizePosPixel
    /// <summary>
    /// 根据一个平面，返回它的地址
    /// </summary>
    /// <param name="rectangular">待返回地址的平面</param>
    /// <param name="isA1">如果这个值为<see langword="true"/>，则返回A1地址，否则返回R1C1地址</param>
    /// <returns></returns>
    public static string GetAddress(ISizePosPixel rectangular, bool isA1 = true)
    {
        var (topLeft, bottomRight) = rectangular.Boundaries;
        var (bc, br) = topLeft.Abs();
        var (ec, er) = bottomRight.Abs();
        return GetAddress(br, bc, er, ec, isA1);
    }
    #endregion
    #region 获取完整地址
    /// <summary>
    /// 获取一个单元格的完整地址，
    /// 它由文件名，工作表名和单元格地址组成
    /// </summary>
    /// <param name="fullPath">工作簿所在的文件</param>
    /// <param name="sheetName">工作表的名称</param>
    /// <param name="address">单元格地址</param>
    /// <returns></returns>
    public static string GetAddressFull(string fullPath, string sheetName, string address)
    {
        var (father, file) = ToolPath.SplitPath(fullPath);
        return $"'{father}\\[{file}]{sheetName}'!{address.ToUpper()}";
    }
    #endregion
    #endregion
    #region 返回数学地址
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
        return SizePosPixel(Point(beginColumn, -beginRow), Point(ec, -er));
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
        return SizePosPixel(beginColumn, ToolArithmetic.Abs(beginRow), columnCount, rowCount);
    }
    #endregion
    #endregion
    #region 解析地址
    #region 返回行列数
    /// <summary>
    /// 根据A1地址，返回开始和结束的行列数，
    /// 注意：返回的行列数从0开始
    /// </summary>
    /// <param name="addressA1">待解析的A1地址</param>
    /// <returns></returns>
    public static (int BeginRow, int BeginCol, int EndRow, int EndCol) AddressToTupts(string addressA1)
    {
        addressA1 = addressA1.ToUpper();
        var mathce = MatchA1.MatcheSingle(addressA1)?.GroupsNamed ??
            throw new Exception($"{addressA1}不是合法的A1地址格式");
        #region 用来获取列号的本地函数
        static int Get(IMatch add)
            => ToolBit.ToDecimal(26, add.Match.Select(x => x - 64).ToArray(), null) - 1;
        #endregion
        var bc = Get(mathce["bc"]);
        var br = mathce["br"].Match.To<int>() - 1;
        if (mathce.ContainsKey("end"))
        {
            var ec = Get(mathce["ec"]);
            var er = mathce["er"].Match.To<int>() - 1;
            return (br, bc, er, ec);
        }
        return (br, bc, br, bc);
    }
    #endregion
    #region 返回ISizePos
    /// <summary>
    /// 解析一个A1格式的地址，并将它转换为等效的平面
    /// </summary>
    /// <param name="addressA1">待解析的A1格式地址</param>
    /// <returns></returns>
    public static ISizePosPixel AddressToSizePos(string addressA1)
    {
        var (br, bc, er, ec) = AddressToTupts(addressA1);
        return SizePosPixel(
            Point(bc, -br),
            ec - bc + 1, er - br + 1);
    }
    #endregion
    #endregion
}
