namespace System.Office.Excel.Realize;

/// <summary>
/// 在实现<see cref="IExcelRange"/>时，
/// 可以继承自本类型，以减少重复的工作
/// </summary>
/// <remarks>
/// 用指定的工作表初始化对象
/// </remarks>
/// <param name="sheet">这个范围所在的工作表</param>
public abstract class ExcelRange(IExcelSheet sheet) : IExcelRange
{
    #region 获取单元格所在的工作表
    public IExcelSheet Sheet { get; } = sheet;
    #endregion
    #region 设置或获取样式
    public abstract IRangeStyle Style { get; set; }
    #endregion
    #region 复制单元格为图片
    public virtual Task CopyPictureToClipboard()
         => throw new NotImplementedException($"这个API没有被实现，一般情况下，只有微软COM组件实现的Excel操作对象才会实现这个API");
    #endregion
    #region 返回单元格地址
    #region 辅助方法
    /// <summary>
    /// 获取范围地址的简单形式，它不包括文件名等信息
    /// </summary>
    /// <param name="isR1C1">如果这个值为<see langword="true"/>，
    /// 代表以R1C1形式返回，否则代表以A1形式返回</param>
    /// <returns></returns>
    private protected abstract string AddressTextSimple(bool isR1C1);
    #endregion
    #region 正式方法
    public string AddressText(bool isR1C1 = true, AddressTextMod whole = 0)
    {
        var address = AddressTextSimple(isR1C1);
        return whole switch
        {
            AddressTextMod.Simple => address,
            AddressTextMod.WithSheetName => $"{Sheet.Name}!{address}",
            AddressTextMod.WithFileName => ExcelRealizeHelp.GetAddressFull
            (Sheet.Book.Path ?? throw new NullReferenceException("该工作簿尚未保存到文件中，无法获取单元格的完全路径"),
            Sheet.Name, address),
            var mod => throw new NotSupportedException($"未能识别地址模式{mod}")
        };
    }
    #endregion
    #endregion
    #region 重写的ToString
    public sealed override string ToString()
        => AddressText();
    #endregion
}
