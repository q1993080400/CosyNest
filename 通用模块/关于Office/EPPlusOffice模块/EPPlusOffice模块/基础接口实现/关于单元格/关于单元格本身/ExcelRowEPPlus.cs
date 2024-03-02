using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel;

/// <summary>
/// 该类型是底层使用EPPlus实现的Excel行
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="row">行对象，本对象的功能就是通过它实现的</param>
/// <inheritdoc cref="ExcelRC(IExcelSheet, bool, int, int)"/>
sealed class ExcelRowEPPlus(IExcelSheet sheet, IEnumerable<ExcelRow> row, int begin, int end) : ExcelRC(sheet, true, begin, end)
{
    #region 封装的行对象
    /// <summary>
    /// 获取封装的行对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IEnumerable<ExcelRow> Row { get; } = row.ToArray();
    #endregion
    #region 是否隐藏
    public override bool? IsHide
    {
        get => Row.Aggregate((bool?)Row.First().Hidden,
            (x, y) => y.Hidden == x ? x : null);
        set
        {
            var v = value is null ? throw new ArgumentNullException(nameof(IsHide)) : value.Value;
            Row.ForEach(x => x.Hidden = v);
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
