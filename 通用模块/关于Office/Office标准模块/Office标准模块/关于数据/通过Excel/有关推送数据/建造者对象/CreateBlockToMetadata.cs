using Options = System.Collections.Generic.IReadOnlyDictionary<string, object>;
using System.Office.Excel;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 该记录可以用来创建一个<see cref="IDataPipeTo"/>，
/// 它通过元数据获取数据在工作表中的排列方式，并可以向工作表中推送数据
/// </summary>
public sealed record CreateBlockToMetadata : BlockBuildBase<IDataPipeTo>
{
    #region 使用说明
    /*使用说明：
      本类型比较复杂，它按照以下流程执行拉取操作，每个流程都有详细说明：

      1.执行由BlockBuildBase负责的操作，在这个类型的源文件中，
      你可以看到有关这些操作的详情
    
      2.调用BlockFilter，来过滤或分割数据，需要分割数据的原因是：
      块的大小是有限的，如果一个数据包含一个集合，而且这个集合的元素数量很多的话，
      块很可能无法容纳这条数据，需要将它分割为多条
    
      3.调用BlockSet写入每条数据，通过指定它，可以推送复杂的数据
    
      4.整条数据写入完成以后，调用OnBlockSeted，它可以执行一些后续操作，
      例如改变单元格的格式等*/
    #endregion
    #region 写入数据的配置
    #region 用来过滤和分割数据的委托
    /// <summary>
    /// 该委托用于过滤和分割数据，它的第一个参数是待分割的数据，
    /// 第二个参数是配置选项，第三个参数是数据地图，返回值是分割后的数据，
    /// 如果为<see langword="null"/>，代表不过滤和分割
    /// </summary>
    public Func<IEnumerable<IData>, Options, DataMap, IEnumerable<IData>>? BlockFilter { get; init; }
    #endregion
    #region 用来写入数据的委托
    /// <summary>
    /// 该委托用于将数据写入单元格，
    /// 它的第一个参数是从元数据中提取的配置选项，
    /// 第二个参数是待写入的数据的键，第三个参数是待写入的键所在的数据，
    /// 第四个参数是该数据列要写入的单元格，如果为不指定，
    /// 则直接写入<see cref="IExcelCells.Value"/>
    /// </summary>
    public Action<Options, string, IData, IExcelCells> BlockSet { get; init; }
        = (_, key, data, cell) => cell.Value = new(data[key]);
    #endregion
    #region 数据写入后执行的委托
    /// <summary>
    /// 当每条数据被写入完成后，执行这个委托，
    /// 委托的第一个参数是提取的配置选项，
    /// 第二个参数是待写入的数据，第三个参数是数据地图，
    /// 第四个参数是整个数据的界限单元格
    /// </summary>
    public Action<Options, IData, DataMap, IExcelCells>? OnBlockSeted { get; init; }
    #endregion
    #endregion
    #region 创建IDataPipeTo
    /// <summary>
    /// 创建一个<see cref="IDataPipeTo"/>，
    /// 它可以用来将数据推送到工作表
    /// </summary>
    /// <param name="sheet">数据所在的工作表</param>
    /// <returns></returns>
    public override IDataPipeTo Create(IExcelSheet sheet)
    {
        #region 本地函数
        BlockToComplex Fun()
        {
            var (dataMap, options) = Initialization(sheet.Book);
            #region 写入数据的本地函数
            void BlockSet(IData data, IExcelCells cell)
            {
                foreach (var (columnName, position) in dataMap.ColumnBoundary)
                {
                    this.BlockSet(options, columnName, data, cell[position]);
                }
                OnBlockSeted?.Invoke(options, data, dataMap, cell);
            }
            #endregion
            BlockFilter? blockFilter = BlockFilter is null ? null : data => BlockFilter(data, options, dataMap);
            return new(sheet,
                CreateBlock.BlockFind(dataMap), BlockSet, blockFilter);
        }
        #endregion
        var cache = CachePipe?.To<BlockToComplex>();
        return cache is null || !ReferenceEquals(cache.Sheet.Book, sheet.Book) ?
            CachePipe = Fun() : cache with                      //如果工作表位于同一工作簿中，则直接使用缓存
            {
                Sheet = sheet
            };
    }
    #endregion 
}
