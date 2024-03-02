using NPOI.SS.UserModel;

namespace System.Office.Excel;

/// <summary>
/// 该类型代表一个底层由Npoi实现的单一单元格，
/// 它没有子单元格，或者说只有自己一个子单元格
/// </summary>
/// <param name="cell">封装的单元格，本对象的功能就是通过它实现的</param>
/// <inheritdoc cref="Realize.ExcelCells.ExcelCells(IExcelSheet)"/>
sealed class ExcelCellSingleNpoi(IExcelSheet sheet, ICell cell) : ExcelCellNpoi(sheet), IExcelCells
{
    #region 公开成员
    #region 关于单元格本身
    #region 获取或设置值
    public override RangeValue Value
    {
        get
        {
            var type = cell.CellType;
            return (type is CellType.Formula ? cell.CachedFormulaResultType : type) switch
            {
                CellType.Blank => new(null),
                CellType.Numeric or CellType.Formula => new(cell.NumericCellValue),
                _ => new(cell.StringCellValue),
            };
        }
        set
        {
            FormulaA1 = null;
            var v = value.Content;
            switch (v)
            {
                case null or string:
                    cell.SetCellValue(v?.ToString());
                    break;
                case int or double or Num:
                    cell.SetCellValue(v.To<double>());
                    break;
                case DateTime t:
                    cell.SetCellValue(t);
                    Style.Format = "yyyy/M/d";
                    break;
                case Array { Length: 1 } a:             //单个单元格只允许写入只有一个元素的数组
#pragma warning disable CA2011
                    Value = new(a.GetValue(0));         //此处并不会产生无限递归Bug，是VS分析器搞错了
                    break;
#pragma warning restore
                case Array:
                    throw new NotSupportedException("禁止在单个单元格中写入数组，除非它只有一个元素");
                default:
                    throw new ArgumentException($"不能将类型{v.GetType()}写入单元格的值", nameof(Value));
            }
        }
    }
    #endregion
    #region 单元格公式（A1格式）
    public override string? FormulaA1
    {
        get => cell.CellType is CellType.Formula ? cell.CellFormula : null;
        set => SetFormulaAssist(value, value => cell.CellFormula = value, false);
    }
    #endregion
    #region 单元格地址
    public override (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address
    {
        get
        {
            var br = cell.RowIndex;
            var bc = cell.ColumnIndex;
            return (br, bc, br, bc);
        }
    }
    #endregion
    #region 单元格样式
    public override IRangeStyle Style
    {
        get => new ExcelCellStyleSingleNpoi(cell);
        set => throw new NotImplementedException();
    }
    #endregion
    #endregion
    #region 关于子单元格与其他单元格
    #region 返回子单元格数量
    public int Count => 1;
    #endregion
    #region 枚举子单元格
    public override IEnumerable<IExcelCells> Cells
    {
        get
        {
            yield return this;
        }
    }
    #endregion
    #region 根据索引获取单元格
    protected override IExcelCells IndexTemplate(int beginRow, int beginColumn, int endRow, int endColumn)
        => (beginRow, beginColumn, endRow, endColumn) is (0, 0, 0, 0) ?
        this : throw new IndexOutOfRangeException("本单元格是一个1行1列的单元格，除非参数为(0,0,0,0)，否则本方法必然引发异常");
    #endregion
    #region 是否合并
    public override bool IsMerge
    {
        get => false;
        set
        {

        }
    }
    #endregion
    #region 替换单元格
    public override void Replace(string content, string replace)
         => throw new NotImplementedException();
    #endregion
    #endregion
    #endregion 
    #region 内部成员
    #endregion
    #region 构造函数
    #endregion
}
