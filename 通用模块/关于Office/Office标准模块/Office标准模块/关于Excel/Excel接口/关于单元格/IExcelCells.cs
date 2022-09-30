using System.Maths.Plane;
using System.Office.Excel.Realize;

namespace System.Office.Excel;

/// <summary>
/// 凡是实现本接口的类型，
/// 都可以被视作Excel单元格，
/// 它可以包含一个或者多个子单元格
/// </summary>
public interface IExcelCells : IExcelRange, IExcelCellsCommunity
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

      问：在此前，曾有一个规范，在读写合并单元格的值和公式时，
      应该视为读写合并单元格第一个单元格的值和公式，这是为了和常人的思维习惯相对应，
      那么，为什么这个规范被取消了？
      答：因为作者认为，合并单元格这个概念是针对直接接触UI的非专业人士，
      对于通过API操作Excel的程序员来说，它的意义并不重要，所以不值得为此增加额外的复杂度*/
    #endregion
    #region 获取或设置值
    /// <summary>
    /// 设置或者获取范围的值
    /// </summary>
    RangeValue Value { get; set; }
    #endregion
    #region 关于公式
    #region 说明文档
    /*注释：
      #实现本API请遵循以下规范：
      如果单元格有值，但是没有公式，
      那么将公式设置为null或空字符串的时候，
      不应改变单元格的值

      #如果存在多个单元格，则按照以下原则读写公式：

      读取：
      若已经合并，则返回合并单元格第一个单元格的公式，否则返回null

      写入：
      若已经合并，则写入合并单元格第一个单元格的公式，
      否则抛出异常，除非写入的值是null或空字符串，它表示撤销公式，
      这是因为公式可能会使用相对引用，它不能像写入Value一样，
      依次写入每个子单元格的公式

      #由于Range重写了ToString方法，直接返回单元格的地址，
      所以Formula可以用这种方式写入：
      假设有ExcelRange类型的变量a，地址是R1C1，
      那么公式"=R1C1+1"可以写成：$"={a}+1"*/
    #endregion
    #region A1格式
    /// <summary>
    /// 获取或设置A1格式的公式
    /// </summary>
    string? FormulaA1 { get; set; }
    #endregion
    #region R1C1格式
    /// <summary>
    /// 设置或者获取单元格的公式，最前面的等号是可选的，
    /// 注意：公式引用只支持R1C1格式
    /// </summary>
    string? FormulaR1C1 { get; set; }
    #endregion
    #endregion 
    #region 格式化后的文本
    /// <summary>
    /// 当读取这个属性时，返回格式化后的文本，
    /// 当写入这个属性时，等同于设置<see cref="Value"/>
    /// </summary>
    string? Text
    {
        get => Value.ToText;
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
            return ExcelRealizeHelp.AddressMathAbs(br, bc, er, ec);
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
    #region 复制单元格
    #region 传入单元格
    /// <summary>
    /// 复制单元格
    /// </summary>
    /// <param name="cells">复制的目标单元格，
    /// 如果它的大小与本单元格不匹配，则从它的左上角开始，复制完整的单元格</param>
    /// <returns>复制后的新单元格</returns>
    IExcelCells Copy(IExcelCells cells);

    /*问：其他框架提供了复制值，复制属性等API，
      为什么本框架仅提供完全复制的API？
      答：因为复制值，复制属性等操作是非专业人士使用UI的思维，
      在通过编程调用时，直接a.Value=b.Value就可以了*/
    #endregion
    #region 传入位置，仅能复制到同一工作表
    /// <param name="row">复制的目标单元格左上角的行</param>
    /// <param name="col">复制的目标单元格左上角的列</param>
    /// <inheritdoc cref="Copy(IExcelCells)"/>
    IExcelCells Copy(int row, int col)
        => Copy(Sheet[row, col]);
    #endregion
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
        => Cells.Where(x => x.Value.Content is not (null or "" or 0) || x.FormulaR1C1 is { });
    #endregion
    #region 枚举所有子单元格
    /// <summary>
    /// 获取一个枚举所有子单元格的枚举器
    /// </summary>
    IEnumerable<IExcelCells> Cells { get; }

    /*实现本API请遵循以下规范：
      单元格的定义是子单元格的容器，也就是说，
      如果本单元格是一个1行1列的单元格，则它的子单元格包括它自己*/
    #endregion
    #endregion
    #region 返回子单元格数量
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

    /*实现本API请遵循以下规范：
      如果本单元格没有合并，但是位于一个合并单元格的内部，
      即便该单元格完全将它包括进来，这个属性仍然应该返回false*/
    #endregion
    #region 变换单元格
    #region 复杂方法
    /// <summary>
    /// 改变单元格的位置和大小，
    /// 并返回一个新的单元格
    /// </summary>
    /// <param name="transform">该委托的参数是当前单元格范围，返回值是新单元格范围</param>
    /// <returns></returns>
    IExcelCells Transform(Func<ISizePosPixel, ISizePosPixel> transform)
        => Sheet[transform(AddressMath)];
    #endregion
    #region 简单方法
    /// <summary>
    /// 将单元格偏移指定的位置，
    /// 并返回一个新的单元格
    /// </summary>
    /// <param name="offsetRow">偏移的行数</param>
    /// <param name="offsetColumns">偏移的列数</param>
    /// <returns></returns>
    IExcelCells Transform(int offsetRow = 0, int offsetColumns = 0)
        => Transform(x => x.Transform(offsetRight: offsetColumns, offsetTop: -offsetRow));
    #endregion
    #endregion
    #endregion
}
