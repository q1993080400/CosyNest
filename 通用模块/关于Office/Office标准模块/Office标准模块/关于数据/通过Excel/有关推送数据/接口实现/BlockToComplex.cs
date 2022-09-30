using System.Office.Excel;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 该类型是<see cref="IDataPipeTo"/>的实现，可以用来将数据推送到单元格，
/// 它能够进行高度的自定义，但同时也非常复杂
/// </summary>
sealed record BlockToComplex : IDataPipeTo
{
    #region 推送所需要的数据
    #region 数据所在的工作表
    /// <summary>
    /// 获取数据所在的工作表
    /// </summary>
    public IExcelSheet Sheet { get; init; }
    #endregion
    #region 寻找属性的委托
    /// <inheritdoc cref="Block.BlockFind"/>
    private BlockFind BlockFind { get; }
    #endregion
    #region 写入属性的委托
    /// <inheritdoc cref="Block.BlockSet"/>
    private BlockSet BlockSet { get; }
    #endregion
    #region 过滤数据的委托
    /// <inheritdoc cref="Block.BlockFilter"/>
    private BlockFilter BlockFilter { get; } = static x => x;
    #endregion
    #endregion
    #region 有关推送数据
    #region 删除数据
    Task IDataPipeTo.Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region 添加或更新数据
    Task IDataPipeTo.AddOrUpdate<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
        => Task.Run(() =>
        {
            foreach (var item in BlockFilter(datas))
            {
                BlockSet(item, Sheet[BlockFind(item)]);
            }
        }, cancellation);
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="sheet">数据所在的工作表</param>
    /// <param name="blockFind">该委托用于从工作表中寻找字段</param>
    /// <param name="blockSet">该委托用于将属性的值写入单元格</param>
    /// <param name="blockFilter">该委托用于过滤数据，在数据被推送到工作表之前，
    /// 会先调用本方法对其进行转换，如果为<see langword="null"/>，则不进行过滤</param>
    public BlockToComplex(IExcelSheet sheet, BlockFind blockFind, BlockSet blockSet, BlockFilter? blockFilter)
    {
        this.Sheet = sheet;
        this.BlockFind = blockFind;
        this.BlockSet = blockSet;
        if (blockFilter is { })
            this.BlockFilter = blockFilter;
    }
    #endregion
}
