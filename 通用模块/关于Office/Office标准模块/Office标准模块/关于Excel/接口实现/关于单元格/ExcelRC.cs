using System.Collections;

namespace System.Office.Excel.Realize;

/// <summary>
/// 在实现<see cref="IExcelRC"/>时，
/// 可以继承自本类型，以减少重复的工作
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="sheet">这个单元格行列所在的工作表</param>
/// <param name="isRow">如果这个值为<see langword="true"/>，
/// 代表这个对象是行，否则代表这个对象是列</param>
/// <param name="begin">开始行号或列号</param>
/// <param name="end">结束行号或列号</param>
public abstract class ExcelRC(IExcelSheet sheet, bool isRow, int begin, int end) : ExcelRange(sheet), IExcelRC
{
    #region 接口形式
    /// <summary>
    /// 返回本对象的接口形式，它可以用来访问一些显式实现的成员
    /// </summary>
    protected IExcelRC Interface => this;
    #endregion
    #region 返回对象是否为行
    public bool IsRow { get; } = isRow;
    #endregion
    #region 关于行或者列的位置和规模
    #region 返回开始和结束行列数
    public (int Begin, int End) Range { get; } = (begin, end);
    #endregion
    #region 以文本形式返回地址（重写辅助方法）
    private protected override string AddressTextSimple(bool isR1C1)
    {
        var (begin, end) = Range;
        return ExcelRealizeHelp.GetAddressRC(begin, end, IsRow, !isR1C1);
    }
    #endregion
    #endregion
    #region 关于迭代器
    public virtual IEnumerator<IExcelRC> GetEnumerator()
    {
        var (b, e) = Range;
        for (; b <= e; b++)
        {
            yield return Sheet.GetRC(IsRow, b, b);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 关于行与列的样式
    #region 隐藏和取消隐藏
    public abstract bool? IsHide { get; set; }
    #endregion
    #region 获取或设置高度或宽度
    public abstract double? HeightOrWidth { get; set; }
    #endregion
    #region 自动调整行高与列宽
    public abstract void AutoFit();
    #endregion
    #endregion
    #region 删除行或者列
    public abstract void Delete();

    #endregion
}
