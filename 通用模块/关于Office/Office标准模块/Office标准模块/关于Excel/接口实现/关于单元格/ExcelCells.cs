using System.MathFrancis.Plane;

namespace System.Office.Excel.Realize;

/// <summary>
/// 在实现<see cref="IExcelCells"/>时，
/// 可以继承自本类型，以减少重复的工作
/// </summary>
/// <remarks>
/// 用指定的参数初始化单元格
/// </remarks>
/// <param name="sheet">指定的工作表</param>
public abstract class ExcelCells(IExcelSheet sheet) : ExcelRange(sheet), IExcelCells
{
    #region 返回IExcelCells接口
    /// <summary>
    /// 返回这个对象的接口形式，
    /// 它可以用来访问一些显式实现的成员
    /// </summary>
    protected IExcelCells Interface
        => this;
    #endregion
    #region 关于单元格本身
    #region 关于单元格内容
    #region 关于值和公式
    #region 设置或者获取值
    public abstract RangeValue Value { get; set; }
    #endregion
    #region 关于公式
    #region A1格式
    public abstract string? FormulaA1 { get; set; }
    #endregion
    #region R1C1格式
    public abstract string? FormulaR1C1 { get; set; }
    #endregion 
    #region 辅助方法
    /// <summary>
    /// 写入公式的辅助方法，
    /// 它会根据实现标准，过滤掉不需要的写入
    /// </summary>
    /// <param name="value">待写入的公式</param>
    /// <param name="setFormula">该委托用于写入公式</param>
    /// <param name="addEqualSign">如果该参数为<see langword="true"/>，
    /// 则给公式加上等号，否则去掉等号</param>
    protected void SetFormulaAssist(string? value, Action<string?> setFormula, bool addEqualSign)
    {
        var isRevoke = value is null or "";
        if (!isRevoke && Interface.Count > 1 && !IsMerge)
            throw new NotSupportedException($"拥有多个单元格的单元格不能写入公式，除非它们已经合并，或写入null");
        if ((isRevoke, Value.Content, FormulaA1) is not (true, { }, null))         //请查阅公式的实现标准，以了解为什么这么写
        {
            value = value?.TrimStart('=');
            setFormula(addEqualSign && value is { } ? "=" + value : value);
        }
    }
    #endregion
    #endregion
    #endregion
    #region 获取或设置超链接
    public abstract string? Link { get; set; }
    #endregion
    #endregion
    #region 获取完整地址
    public abstract (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address { get; }
    #endregion
    #region 以文本形式返回地址（重写辅助方法）
    private protected override string AddressTextSimple(bool isR1C1)
        => ExcelRealizeHelp.GetAddress(Interface.AddressMath, !isR1C1);
    #endregion
    #region 返回视觉位置
    public abstract ISizePos VisualPosition { get; }
    #endregion
    #region 复制单元格
    public abstract IExcelCells Copy(IExcelCells cells);
    #endregion
    #region 替换单元格
    public abstract void Replace(string content, string replace);
    #endregion
    #endregion
    #region 关于子单元格和其他单元格
    #region 合并和取消合并
    public abstract bool IsMerge { get; set; }
    #endregion
    #region 根据绝对位置返回单元格
    #region 正式方法
    public IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1]
    {
        get
        {
            beginRow = Math.Abs(beginRow);
            beginColumn = Math.Abs(beginColumn);
            endRow = endRow == -1 ? beginRow : endRow;
            endColumn = endColumn == -1 ? beginColumn : endColumn;
            return IndexTemplate(beginRow, beginColumn, endRow, endColumn);
        }
    }
    #endregion
    #region 模板方法
    /// <summary>
    /// <see cref="this[int, int, int, int]"/>的模板方法，
    /// 它可以保证所有参数均不为负值
    /// </summary>
    /// <inheritdoc cref="this[int, int, int, int]"/>
    protected abstract IExcelCells IndexTemplate(int beginRow, int beginColumn, int endRow, int endColumn);
    #endregion
    #endregion
    #region 枚举所有子单元格
    public virtual IEnumerable<IExcelCells> Cells
    {
        get
        {
            var (br, bc, er, ec) = Address;
            for (; br <= er; br++)
            {
                for (int bcCopy = bc; bcCopy <= ec; bcCopy++)
                {
                    yield return Sheet[br, bcCopy];
                }
            }
        }
    }
    #endregion
    #region 搜索单元格
    public virtual IEnumerable<IExcelCells> Find(string content, bool findValue = true)
        => Cells.Where(x =>
         (findValue ? x.Value.ToText : x.FormulaR1C1)?.Contains(content) ?? false);

    #endregion
    #endregion
}
