using System.Maths;
using System.Maths.Plane;
using System.Office.Excel.Realize;
using System.Text.RegularExpressions;

using EXRange = Microsoft.Office.Interop.Excel.Range;

namespace System.Office.Excel;

/// <summary>
/// 这个对象代表通过微软COM组件实现的Excel单元格
/// </summary>
class ExcelCellsMicrosoft : ExcelCells, IExcelCells
{
    #region 封装的单元格
    /// <summary>
    /// 返回封装的Excel单元格，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal EXRange PackRange { get; }
    #endregion
    #region 关于单元格本身
    #region 返回视觉位置
    public override ISizePos VisualPosition
    {
        get
        {
            var point = CreateMath.Point(PackRange.Left, -PackRange.Top);
            return CreateMath.SizePos(point, PackRange.Width, PackRange.Height);
        }
    }
    #endregion
    #region 关于单元格内容
    #region 关于值和公式
    #region 获取或设置值
    public override RangeValue Value
    {
        get => new() { Content = PackRange.Value };
        set => PackRange.Value = value.Content switch
        {
            object[] array when Interface.RCCount is (var r and > 1, var c) => array.ToArray(r, c),
            var content => content
        };
    }
    #endregion
    #region 关于公式
    #region A1格式（只读）
    public override string? FormulaA1
    {
        get => PackRange.Formula switch
        {
            string s => s.TrimStart('='),
            _ => null
        };
        set => SetFormulaAssist(value, value => PackRange.Formula = value, true);
    }
    #endregion
    #region R1C1格式
    public override string? FormulaR1C1
    {
        get => PackRange.FormulaR1C1 switch
        {
            string text => text.TrimStart('='),
            _ => null
        };
        set => SetFormulaAssist(value, value => PackRange.FormulaR1C1 = value, true);
    }
    #endregion
    #endregion
    #endregion 
    #region 读写样式
    private IRangeStyle? StyleField;

    public override IRangeStyle Style
    {
        get => StyleField ??= new RangeStyleMicrosoft(PackRange);
        set => ExcelRealizeHelp.CopyStyle(value, Style);
    }
    #endregion
    #region 获取或设置超链接
    #region 辅助方法
    /// <summary>
    /// 检查单元格的公式，并返回它是否为一个链接，
    /// 以及链接的地址部分和显示文本部分
    /// </summary>
    /// <returns></returns>
    private (bool IsLink, string? Link, string? Value) LinkAided()
    {
        var formula = FormulaR1C1;
        if (formula is null)
            return (false, null, null);
        var match = @"^HYPERLINK\(""(?<link>[\S]+?)""(,""?(?<value>[\S]+?)""?)?\)$".Op().
            Regex(RegexOptions.IgnoreCase).MatcheFirst(formula)?.GroupsNamed;
        return match is null ?
        (false, null, null) :
        (true, match["link"].Match, match.TryGetValue("value").Value?.Match);
    }
    #endregion
    #region 正式方法
    public override string? Link
    {
        get
        {
            var link = PackRange.Hyperlinks;
            return link.Count is 1 ? link[1].Address : LinkAided().Link;
        }
        set
        {
            if (value is null)
            {
                if (PackRange.Hyperlinks.Count > 0)         //如果链接集合中有元素
                    PackRange.Hyperlinks.Delete();          //则将其删除
                else
                {
                    var (isLink, _, value2) = LinkAided();       //否则检查公式中是否设置了链接
                    if (isLink)
                        FormulaR1C1 = value2;
                }
            }
            else
            {
                var text = FormulaR1C1 ?? Interface.Text ?? value;
                FormulaR1C1 = @$"HYPERLINK(""{value}"",{(FormulaR1C1 == null ? $"\"{text}\"" : text)})";
            }
        }
    }
    #endregion
    #endregion
    #endregion
    #region 返回单元格完整地址
    public override (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address { get; }
    #endregion
    #region 复制单元格
    public override IExcelCells Copy(IExcelCells cells)
    {
        PackRange.Copy(cells.To<ExcelCellsMicrosoft>().PackRange);
        return cells.Transform(x => x.Transform(size: Interface.AddressMath));
    }
    #endregion
    #endregion
    #region 关于子单元格和其他单元格
    #region 返回子单元格的索引器
    protected override IExcelCells IndexTemplate(int beginRow, int beginColumn, int endRow, int endColumn)
    {
        var add = ExcelRealizeHelp.GetAddress(beginRow, beginColumn, endRow, endColumn);
        return new ExcelCellsMicrosoft(Sheet, PackRange.Range[add]);
    }
    #endregion
    #region 枚举所有子单元格
    public override IEnumerable<IExcelCells> Cells
        => PackRange.Cells.OfType<EXRange>().Select(x => new ExcelCellsMicrosoft(Sheet, x));
    #endregion
    #region 合并和取消合并
    public override bool IsMerge
    {
        get => Equals((object)PackRange.MergeCells, true);
        set
        {
            if (value)
                PackRange.Merge();
            else PackRange.UnMerge();
        }
    }
    #endregion
    #region 替换单元格
    public override void Replace(string content, string replace)
        => PackRange.Replace(content, replace);
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的工作表和单元格封装进对象
    /// </summary>
    /// <param name="sheet">指定的工作表</param>
    /// <param name="packRange">封装的单元格</param>
    public ExcelCellsMicrosoft(IExcelSheet sheet, EXRange packRange)
        : base(sheet)
    {
        this.PackRange = packRange;
        Address = packRange.GetAddress();
    }
    #endregion
}
