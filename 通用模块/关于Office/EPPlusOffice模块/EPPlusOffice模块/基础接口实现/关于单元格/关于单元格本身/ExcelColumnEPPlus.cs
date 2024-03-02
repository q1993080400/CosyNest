using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 使用指定的参数初始化对象
/// </summary>
/// <param name="column">列对象，本对象的功能就是通过它实现的</param>
/// <inheritdoc cref="ExcelRC(IExcelSheet, bool, int, int)"/>
sealed class ExcelColumnEPPlus(IExcelSheet sheet, IEnumerable<ExcelColumn> column, int begin, int end) : ExcelRC(sheet, false, begin, end)
{
    #region 封装的列对象
    /// <summary>
    /// 获取封装的列对象，本对象的功能就是通过它实现的
    /// </summary>
    private IEnumerable<ExcelColumn> Column { get; } = column.ToArray();
    #endregion
    #region 是否隐藏
    public override bool? IsHide
    {
        get => Column.Aggregate((bool?)Column.First().Hidden,
            (x, y) => y.Hidden == x ? x : null);
        set
        {
            var v = value is null ? throw new ArgumentNullException(nameof(IsHide)) : value.Value;
            Column.ForEach(x => x.Hidden = v);
        }
    }
    #endregion
    #region 未实现的成员
    public override double? HeightOrWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override void AutoFit()
    {
        throw new NotImplementedException();
    }

    public override void Delete()
    {
        throw new NotImplementedException();
    }

    public override IRangeStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    #endregion
    #region 构造函数
    #endregion
}
