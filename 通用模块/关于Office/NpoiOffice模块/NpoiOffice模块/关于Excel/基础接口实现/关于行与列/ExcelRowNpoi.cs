
using NPOI.SS.UserModel;

namespace System.Office.Excel;

/// <summary>
/// 该类型是底层使用Npoi实现的Excel行
/// </summary>
sealed class ExcelRowNpoi : ExcelRCNpoi
{
    #region 行集合
    /// <summary>
    /// 获取封装的行集合，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IEnumerable<IRow> Rows { get; }
    #endregion
    #region 是否隐藏
    public override bool? IsHide
    {
        get => Rows.Aggregate(Rows.First().Hidden, (x, y) => x == y.Hidden ? x : null);
        set => Rows.ForEach(x => x.Hidden = value ?? throw new ArgumentNullException(nameof(IsHide)));
    }
    #endregion
    #region 构造函数
    /// <param name="rows">封装的列集合，本对象的功能就是通过它实现的</param>
    /// <inheritdoc cref="ExcelRCNpoi(IExcelSheet, bool, int, int)"/>
    public ExcelRowNpoi(IExcelSheet sheet, int begin, int end, IEnumerable<IRow> rows)
        : base(sheet, true, begin, end)
    {
        this.Rows = rows.ToArray();
    }
    #endregion
}
