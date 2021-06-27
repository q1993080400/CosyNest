using System.Collections.Generic;
using System.Linq;
using System.Maths;
using System.Office.Realize;

namespace System.Office.Excel
{
    /// <summary>
    /// 凡是实现本接口的类型，
    /// 都可以被视作Excel单元格，
    /// 它可以包含一个或者多个子单元格
    /// </summary>
    public interface IExcelCells : IExcelRange
    {
        #region 说明文档
        /*对实现本类型的规范：
          #在读写关于单元格内容或样式的属性时，如果这个单元格已经合并，
          那么应该正确的读写整个合并单元格（而不是自己）的属性，
          例如：A1和A2已经合并，那么无论写入A1或A2的Value，
          都应该写入A1:A2的Value，这样做的目的在于：
          与日常操作Excel的习惯相符合
           
          #对于不属于单元格内容的属性，不适用于第一条，例如：
          读取A1单元格的Count时，无论A1是否合并，都应当返回1*/
        #endregion
        #region 关于单元格本身
        #region 关于单元格内容
        #region 说明文档：对实现API的规范
        /*注释：实现Value和Formula属性请遵循以下规范：
          #写入Value应考虑数字格式，例如：
          如果写入一个DateTime，那么数字格式也应该改为日期，
          让Value的写入值和返回值类型相同
          
          #如果单元格是合并单元格中的一个子单元格，
          那么在读写Value和FormulaR1C1时，应该读写合并单元格的值或公式，举例说明：
          假设有合并单元格R1C1:R2C2，那么R2C2的Value和FormulaR1C1应该从R1C1中找，
          这样的目的是使对单元格的读写符合常人的思维习惯*/
        #endregion
        #region 获取或设置值
        /// <summary>
        /// 设置或者获取范围的值
        /// </summary>
        RangeValue? Value { get; set; }
        #endregion
        #region 获取或者设置公式
        /// <summary>
        /// 设置或者获取单元格的公式，最前面的等号是可选的，
        /// 注意：公式引用只支持R1C1格式
        /// </summary>
        string? FormulaR1C1 { get; set; }

        /*注释：
          #实现本API请遵循以下规范：
          如果单元格有值，但是没有公式，
          那么将公式设置为Null或空字符串的时候，
          不应改变单元格的值
           
          #由于Range重写了ToString方法，直接返回单元格的地址，
          所以Formula可以用这种方式写入：
          假设有ExcelRange类型的变量a，地址是R1C1，
          那么公式"=R1C1+1"可以写成：$"={a}+1"*/
        #endregion
        #region 格式化后的文本
        /// <summary>
        /// 当读取这个属性时，返回格式化后的文本，
        /// 当写入这个属性时，等同于设置<see cref="Value"/>
        /// </summary>
        string? Text
        {
            get => Value?.ToText;
            set => Value = value;
        }
        #endregion
        #region 获取或设置超链接
        /// <summary>
        /// 获取或设置这个单元格的超级链接，
        /// 如果为<see langword="null"/>，代表没有链接
        /// </summary>
        string? Link { get; set; }

        /*实现本API请遵循以下规范：
          #Excel中创建链接有两种方式：
          直接添加链接和使用HYPERLINK函数，
          这个属性应该能够同时识别这两种链接方式*/
        #endregion
        #endregion
        #region 关于单元格地址
        #region 获取完整地址
        /// <summary>
        /// 获取单元格的完整地址，由起始行，起始列，结束行，结束列组成
        /// </summary>
        (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address { get; }
        #endregion
        #region 返回数学地址
        /// <summary>
        /// 返回数学地址，也就是代表单元格的平面
        /// </summary>
        ISizePosPixel AddressMath
        {
            get
            {
                var (br, bc, er, ec) = Address;
                return ToolExcel.AddressMathAbs(br, bc, er, ec);
            }
        }
        #endregion
        #endregion
        #region 返回视觉位置
        /// <summary>
        /// 返回本单元格的视觉位置，
        /// 也就是单元格在屏幕上的坐标和大小，
        /// 以像素为单位
        /// </summary>
        ISizePos VisualPosition { get; }
        #endregion
        #endregion
        #region 关于子单元格和其他单元格
        #region 关于子单元格
        #region 获取枚举子单元格的枚举器
        #region 枚举非空单元格
        /// <summary>
        /// 获取一个枚举所有非空子单元格的枚举器
        /// </summary>
        IEnumerable<IExcelCells> CellsNotNull
            => CellsAll.Where(x => x.Value != null || x.FormulaR1C1 != null);
        #endregion
        #region 枚举所有子单元格
        /// <summary>
        /// 获取一个枚举所有子单元格的枚举器
        /// </summary>
        IEnumerable<IExcelCells> CellsAll { get; }
        #endregion
        #endregion
        #region 返回单元格的子单元格数量
        /// <summary>
        /// 返回单元格内部的所有子单元格数量
        /// </summary>
        int Count
        {
            get
            {
                var (r, c) = RCCount;
                return r * c;
            }
        }
        #endregion
        #region 返回单元格的开始和结束
        /// <summary>
        /// 返回单元格的开始单元格和结束单元格
        /// </summary>
        (IExcelCells Begin, IExcelCells End) BeginEnd
        {
            get
            {
                var (br, bc, er, ec) = Address;
                return (Sheet[br, bc], Sheet[er, ec]);
            }
        }
        #endregion
        #region 返回子单元格的索引器
        #region 说明文档
        /*说明文档：
           1.经过深思熟虑，作者决定取消通过地址返回子单元格的功能，原因在于：
           1.1支持这个功能极大的增加了开发的复杂度，地址需要考虑A1格式和R1C1格式的不同，
           如果想要解析它，需要通过比较复杂的手段
           1.2除了符合旧有习惯以外，支持它带来的好处不明显，
           而且Excel不是为专业编程人员设计的，它的很多习惯不应该被带到这里
           1.3地址的行列号是从1开始的，但是在编程中习惯从0开始，
           它们之间的转换如果遗漏，很容易引起Bug*/
        #endregion
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
          2.之所以强制实现这个索引器，是因为绝对行列号实现起来容易一些*/
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
                var (rc, cc) = size;
                return this[beginRow, beginColumn, beginRow + rc - 1, beginColumn + cc - 1];
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
                var (h, v) = rectangle;
                var (r, t) = rectangle.FirstPixel.Abs();
                return this[t, r, (v, h)];
            }
        }
        #endregion
        #endregion
        #endregion
        #region 关于行与列
        #region 返回单元格的行列数
        /// <summary>
        /// 返回一个元组，
        /// 分别指示单元格的行数量和列数量
        /// </summary>
        (int RowCount, int ColCount) RCCount
        {
            get
            {
                var (br, bc, er, ec) = Address;
                return (er - br + 1, ec - bc + 1);
            }
        }
        #endregion
        #region 返回所有的行或列
        /// <summary>
        /// 返回这个单元格中所有的行或列
        /// </summary>
        /// <param name="isRow">如果这个值为<see langword="true"/>，则返回行，否则返回列</param>
        /// <returns></returns>
        IExcelRC GetRC(bool isRow)
        {
            var (br, bc, er, ec) = Address;
            return Sheet.GetRC(isRow ? br : bc, isRow ? er : ec, isRow);
        }
        #endregion
        #endregion
        #region 合并和取消合并
        /// <summary>
        /// 在读取这个值的时候，返回单元格是否合并，
        /// 在写入这个值的时候，合并或取消合并单元格
        /// </summary>
        bool IsMerge { get; set; }
        #endregion
        #region 偏移单元格
        /// <summary>
        /// 偏移这个单元格，并返回一个新的单元格
        /// </summary>
        /// <param name="extendToRight">单元格的大小会向右扩展指定的列数</param>
        /// <param name="extenToDown">单元格的大小会向下扩展指定的列数</param>
        /// <param name="right">向右移动的单元格数</param>
        /// <param name="down">向下移动的单元格数</param>
        /// <returns></returns>
        IExcelCells Offset(int extendToRight = 0, int extenToDown = 0, int right = 0, int down = 0)
            => Sheet[AddressMath.Transform(extendToRight, extenToDown, right, -down)];
        #endregion
        #endregion
    }
}