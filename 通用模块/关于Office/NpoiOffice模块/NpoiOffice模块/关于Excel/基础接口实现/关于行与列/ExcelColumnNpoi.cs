namespace System.Office.Excel;

/// <summary>
/// 该类型是底层使用Npoi实现的Excel列
/// </summary>
/// <param name="styles">该集合枚举所有列的样式</param>
/// <inheritdoc cref="ExcelRCNpoi(IExcelSheet, bool, int, int)"/>
sealed class ExcelColumnNpoi(IExcelSheet sheet, int begin, int end) : ExcelRCNpoi(sheet, false, begin, end)
{
    #region 是否隐藏
    #region 辅助方法
    /// <summary>
    /// <see cref="IsHide"/>的辅助方法，
    /// 它返回所有子列的列号
    /// </summary>
    /// <returns></returns>
    private int[] IsHideAssist()
    {
        var (b, e) = Interface.Range;
        return CreateCollection.RangeBE(b, e).ToArray();
    }
    #endregion
    #region 正式属性
    public override bool? IsHide
    {
        get
        {
            var cols = IsHideAssist();
            #region 本地函数
            bool? Fun(int col)
                => SheetNpoi.GetColumnWidth(col) == 0;
            #endregion
            return cols.Aggregate(Fun(cols[0]), (x, y) => x == Fun(y) ? x : null);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value is false)
                throw new NotSupportedException($"由于Npoi底层限制，只能隐藏列，不能取消隐藏列");
            foreach (var item in IsHideAssist())
            {
                SheetNpoi.SetColumnHidden(item, value.Value);
            }
        }
    }

    #endregion
    #endregion
    #region 构造函数
    #endregion
}
