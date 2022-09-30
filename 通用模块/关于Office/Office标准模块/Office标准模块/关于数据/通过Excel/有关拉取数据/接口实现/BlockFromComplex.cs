using System.Office.Excel;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 该类型是一个复杂的<see cref="IDataPipeFrom"/>实现，
/// 它可以进行高度的自定义，但同时也比较难以学习
/// </summary>
sealed record BlockFromComplex : IDataPipeFrom
{
    #region 数据所在的工作表
    /// <summary>
    /// 获取数据所在的工作表
    /// </summary>
    public IExcelSheet Sheet { get; init; }
    #endregion
    #region 用于枚举数据单元格的委托
    /// <inheritdoc cref="Block.BlockGetDatas"/>
    private BlockGetDatas BlockGetDatas { get; }
    #endregion
    #region 用来获取数据的委托
    /// <inheritdoc cref="Block.BlockGet"/>
    private BlockGet BlockGet { get; }
    #endregion
    #region 拉取数据
    IQueryable<Data> IDataPipeFrom.Query<Data>()
        => BlockGetDatas(Sheet).Select(x => BlockGet(x).Copy<Data>()).AsQueryable();
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="sheet">数据所在的工作表</param>
    /// <param name="blockGetDatas">该委托枚举数据所在的单元格</param>
    /// <param name="blockGet">该委托将单元格转化为数据</param>
    public BlockFromComplex(IExcelSheet sheet, BlockGetDatas blockGetDatas, BlockGet blockGet)
    {
        this.Sheet = sheet;
        this.BlockGetDatas = blockGetDatas;
        this.BlockGet = blockGet;
    }
    #endregion
}
