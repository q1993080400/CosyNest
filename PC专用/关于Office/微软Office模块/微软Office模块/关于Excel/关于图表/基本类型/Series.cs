using System.Office.Chart;

namespace System.Office.Excel.Chart;

/// <summary>
/// 该类型是<see cref="ISeries"/>的实现，
/// 可以用来代表一个系列
/// </summary>
class Series : ISeries
{
    #region 封装的对象
    #region 工作表
    /// <summary>
    /// 获取封装的工作表，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IExcelSheet Sheet { get; }
    #endregion
    #region 系列对象
    /// <summary>
    /// 获取封装的系列对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Microsoft.Office.Interop.Excel.Series PackSeries { get; }
    #endregion
    #endregion 
    #region 获取或设置名称
    public string Name
    {
        get => PackSeries.Name;
        set => PackSeries.Name = value;
    }
    #endregion
    #region 删除系列
    public void Delete()
        => PackSeries.Delete();
    #endregion
    #region 有关系列的值
    #region 辅助方法
    #region 获取值
    /// <summary>
    /// 获取系列的值
    /// </summary>
    /// <returns>一个元组，它的项分别是系列X轴和Y轴的值</returns>
    private (IExcelCells? X, IExcelCells? Y) GetValue()
    {
        var f = PackSeries.Formula.Split(",");
        #region 本地函数
        IExcelCells? Fun(string address)
            => address.IsVoid() ? null : Sheet.Book.ExcelCellsAbs(address);
        #endregion
        return (Fun(f[1]), Fun(f[2]));
    }
    #endregion
    #region 设置值
    /// <summary>
    /// 设置系列的值
    /// </summary>
    /// <param name="value">要设置的值</param>
    /// <param name="setX">如果这个值为<see langword="true"/>，则设置X，否则设置Y</param>
    private void SetValue(IExcelCells? value, bool setX = true)
    {
        var f = PackSeries.Formula.Split(",");
        var add = value is null ? "" : value.AddressText(false, 1);
        f[setX ? 1 : 2] = add;
        if ((setX, f[1], f[2]) is (true, not "", ""))
            f[2] = "{1}";                           //Excel强制要求系列具有Y轴数据，所以需要这个操作以避免异常
        PackSeries.Formula = f.Join(",");
    }
    #endregion
    #endregion
    #region X值
    public IExcelCells? X
    {
        get => GetValue().X;
        set => SetValue(value);
    }
    #endregion
    #region Y值
    public IExcelCells? Y
    {
        get => GetValue().Y;
        set => SetValue(value, false);
    }
    #endregion
    #endregion 
    #region 构造函数
    public Series(Microsoft.Office.Interop.Excel.Series packSeries, IExcelSheet sheet)
    {
        PackSeries = packSeries;
        this.Sheet = sheet;
    }
    #endregion
}
