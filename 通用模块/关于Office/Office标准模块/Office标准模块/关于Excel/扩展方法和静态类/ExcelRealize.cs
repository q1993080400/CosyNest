using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Maths;
using System.Office.Realize;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System.Office.Excel.Realize
{
    /// <summary>
    /// 这个类型为实现Excel对象提供帮助
    /// </summary>
    public static class ExcelRealize
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
        #region 关于Range
        #region 复制格式
        #region 缓存属性
        /// <summary>
        /// 这个属性缓存<see cref="IRangeStyle"/>中所有公开，
        /// 而且既能读取又能写入的属性，为复制格式提供方便
        /// </summary>
        private static IEnumerable<PropertyInfo> CacheStyleProperty { get; }
        = typeof(IRangeStyle).GetProperties().
            Where(x => x.GetPermissions() == null && !x.IsStatic()).ToArray();
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
        #region 关于地址
        #region 返回地址
        #region 将列号转换为文本形式
        /// <summary>
        /// 将列号转换为文本形式
        /// </summary>
        /// <param name="column">待转换的列号，从0开始</param>
        /// <returns>列号对应的A1格式地址</returns>
        internal static string ColumnToText(int column)
        {
            ExceptionIntervalOut.Check(0, null, column);
            const int begin = 'A';
            return ToolBit.FromDecimal(column, 26).Integer.PackIndex(true).
                Select(x => (char)(begin + (x.Index == 0 && x.Count > 1 ? x.Elements - 1 : x.Elements))).Join();
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
            var father = Path.GetDirectoryName(fullPath);
            var file = Path.GetFileName(fullPath);
            return $"'{father}\\[{file}]{sheetName}'!{address}";
        }
        #endregion
        #endregion
        #region 解析A1地址
        #region 返回行列数
        /// <summary>
        /// 根据A1地址，返回开始和结束的行列数，
        /// 注意：返回的行列数从0开始
        /// </summary>
        /// <param name="Address">待解析的A1地址</param>
        /// <returns></returns>
        public static (int BeginRow, int BeginCol, int EndRow, int EndCol) AddressToTupts(string addressA1)
        {
            var mathce = /*language=regex*/@"^\$?(?<bc>[A-Z]+)\$?(?<br>\d+)(?<end>:\$?(?<ec>[A-Z]+)\$?(?<er>\d+))?$".
                ToRegex().MatcheFirst(addressA1)?.GroupsNamed;
            if (mathce is null)
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
            return CreateMath.SizePosPixel(
                CreateMath.Point(bc, -br),
                ec - bc + 1, er - br + 1);
        }
        #endregion
        #endregion
        #endregion
        #endregion
    }
}
