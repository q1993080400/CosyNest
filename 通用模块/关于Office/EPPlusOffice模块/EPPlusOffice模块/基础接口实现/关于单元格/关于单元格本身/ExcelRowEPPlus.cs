using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 该类型是底层使用EPPlus实现的Excel行
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="rows">行对象，本对象的功能就是通过它实现的</param>
/// <inheritdoc cref="ExcelRC(IExcelSheet, bool, int, int)"/>
sealed class ExcelRowEPPlus(IExcelSheet sheet, IEnumerable<ExcelRow> rows, int begin, int end) : ExcelRC(sheet, true, begin, end)
{
    #region 是否隐藏
    public override bool? IsHide
    {
        get => rows.Aggregate((bool?)rows.First().Hidden,
            (x, y) => y.Hidden == x ? x : null);
        set
        {
            var v = value is null ? throw new ArgumentNullException(nameof(IsHide)) : value.Value;
            rows.ForEach(x => x.Hidden = v);
        }
    }
    #endregion
    #region 自动调整大小
    public override void AutoFit()
    {
        foreach (var row in rows)
        {
            row.CustomHeight = false;
        }
    }
    #endregion
    #region 未实现的成员
    public override double? HeightOrWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override void Delete()
    {
        throw new NotImplementedException();
    }

    public override IRangeStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    #endregion
    #region 构造函数
    #endregion
}
