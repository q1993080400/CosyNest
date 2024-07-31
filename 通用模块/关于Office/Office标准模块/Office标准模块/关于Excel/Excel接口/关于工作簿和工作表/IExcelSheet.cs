namespace System.Office.Excel;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个Excel工作表
/// </summary>
public interface IExcelSheet : IExcelCellsCommunity
{
    #region 关于工作簿与工作表
    #region 返回工作簿
    /// <summary>
    /// 返回本工作表所在的工作簿
    /// </summary>
    IExcelBook Book { get; }
    #endregion
    #region 返回工作表管理对象
    /// <summary>
    /// 返回一个用来管理所有工作表的对象
    /// </summary>
    IExcelSheetManage SheetManage { get; }
    #endregion
    #region 工作表的名称
    /// <summary>
    /// 获取或设置工作表的名称
    /// </summary>
    string Name { get; set; }
    #endregion
    #region 工作表的索引
    /// <summary>
    /// 获取或设置工作表在工作簿中的索引
    /// </summary>
    int Index { get; set; }
    #endregion
    #region 对工作表的操作
    #region 复制工作表
    /// <summary>
    /// 将这个工作表复制到新工作簿，
    /// 并放置在工作表集合的末尾
    /// </summary>
    /// <param name="collection">目标工作簿的工作表容器，
    /// 如果为<see langword="null"/>，则复制到本工作簿</param>
    /// <param name="renamed">一个用于修改工作表名，使其不重名的委托，
    /// 它的第一个参数是旧名称，第二个参数是尝试失败的次数，从2开始，返回值就是新的名称，
    /// 如果为<see langword="null"/>，则使用一个默认方法</param>
    /// <returns>复制后的新工作表</returns>
    IExcelSheet Copy(IExcelSheetManage? collection = null, Func<string, int, string>? renamed = null);
    #endregion
    #region 剪切工作表
    /// <summary>
    /// 将这个工作表剪切到其他工作簿
    /// </summary>
    /// <param name="collection">目标工作簿的工作表容器</param>
    /// <param name="renamed">一个用于修改工作表名，使其不重名的委托，
    /// 它的第一个参数是旧名称，第二个参数是尝试失败的次数，从2开始，返回值就是新的名称，
    /// 如果为<see langword="null"/>，则使用一个默认方法</param>
    /// <returns>剪切后的新工作表</returns>
    IExcelSheet Cut(IExcelSheetManage collection, Func<string, int, string>? renamed = null)
    {
        var newSheet = Copy(collection, renamed);
        Delete();
        return newSheet;
    }
    #endregion
    #region 删除工作表
    /// <summary>
    /// 将这个工作表从工作簿中删除
    /// </summary>
    void Delete();

    /*实现本API请遵循以下规范：
      如果删除了工作簿中的唯一工作表，
      则不会引发异常，而是将工作簿文件删除*/
    #endregion
    #endregion
    #region 返回页面对象
    /// <summary>
    /// 返回页面对象，
    /// 它可以管理这个工作表的页面设置和打印
    /// </summary>
    ISheetPage Page { get; }
    #endregion
    #endregion
    #region 关于单元格
    #region 返回所有单元格
    /// <summary>
    /// 返回工作表的所有单元格
    /// </summary>
    IExcelCells Cell { get; }

    /*实现本API请遵循以下规范：
      #如果底层实现支持，则应该尽可能裁剪空白单元格，例如：
      假设工作表中只有R3C3一个单元格有值，则该属性应该返回R1C1:R3C3，
      这是为了遍历单元格时尽可能地节约性能*/
    #endregion
    #region 返回行或者列
    /// <summary>
    /// 从这个工作表返回行或者列
    /// </summary>
    /// <param name="isRow">如果这个值为<see langword="true"/>，
    /// 代表返回行，否则返回列</param>
    /// <param name="begin">开始行列号</param>
    /// <param name="end">结束行列号，如果这个值为<see langword="null"/>，
    /// 则默认为与开始行列数相等</param>
    /// <returns></returns>
    IExcelRC GetRC(bool isRow, int begin, int? end = null);
    #endregion
    #endregion
    #region 关于Excel对象
    #region 图表管理对象
    /// <summary>
    /// 这个对象可以用来管理这个工作表中的所有图表
    /// </summary>
    IOfficeObjectManageCommon<IOfficeChart> ChartManage { get; }
    #endregion
    #endregion
}
