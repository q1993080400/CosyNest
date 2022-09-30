using Options = System.Collections.Generic.IReadOnlyDictionary<string, object>;
using System.Office.Excel;
using System.Maths.Plane;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 该记录可以用来创建一个<see cref="IDataPipeFrom"/>，
/// 它通过元数据获取数据在工作表中的排列方式，并可以向工作表中拉取数据
/// </summary>
public sealed record CreateBlockFromMetadata : BlockBuildBase<IDataPipeFrom>
{
    #region 使用说明
    /*使用说明：
      本类型比较复杂，它按照以下流程执行拉取操作，每个流程都有详细说明：

      1.执行由BlockBuildBase负责的操作，在这个类型的源文件中，
      你可以看到有关这些操作的详情
    
      2.进入定位阶段，调用BlockGetDatas获取一个迭代器，
      它枚举每条数据所在的位置，但是不枚举单元格
    
      3.调用BlockFilter，它会将BlockGetDatas返回的ISizePosPixel转换为单元格，
      并检查哪些单元格是有效的，如果出现第一个无效单元格，整个拉取过程会停止
    
      4.对每个有效的数据单元格，注意，它是包含整个数据的单元格，
      而不是单个单元格，调用BlockGet，来获取数据的每个列的值，
      通过指定它，可以提取复杂类型的数据，拉取完成*/
    #endregion
    #region 关于定位数据
    #region 枚举所有数据范围
    #region 默认方法
    /// <summary>
    /// 该方法是<see cref="BlockGetDatas"/>的默认方法，
    /// 它可以用于枚举数据的范围
    /// </summary>
    /// <param name="options">数据的选项</param>
    /// <param name="map">数据地图</param>
    /// <returns>枚举所有数据范围的枚举器</returns>
    public static IEnumerable<ISizePosPixel> BlockGetDatasDefault(Options options, DataMap map)
    {
        for (int index = 0; true; index++)
        {
            yield return map.GetPosition(index);
        }
    }
    #endregion
    #region 正式委托
    /// <summary>
    /// 该委托用于枚举数据的范围，
    /// 它的第一个参数是数据的选项，第二个参数是数据地图，
    /// 返回值就是枚举所有数据范围的枚举器
    /// </summary>
    public Func<Options, DataMap, IEnumerable<ISizePosPixel>> BlockGetDatas { get; init; } = BlockGetDatasDefault;
    #endregion
    #endregion
    #region 筛选有效范围
    /// <summary>
    /// 获取一个用来筛选有效范围的委托，
    /// 当出现无效的范围以后，会停止数据提取
    /// </summary>
    public Func<IExcelCells, bool> BlockFilter { get; init; }
        = static cell => cell.Cells.All(x => x.Value.Content is { });
    #endregion
    #endregion
    #region 用来获取数据的委托
    /// <summary>
    /// 该委托的第一个参数是配置选项，
    /// 第二个参数是待读取的数据的键，
    /// 第三个参数是数据的列所在的单元格，
    /// 返回值就是提取到的数据列的值
    /// </summary>
    public Func<Options, string, IExcelCells, object?> BlockGet { get; init; }
        = static (_, _, cell) => cell.Value.Content;
    #endregion
    #region 创建IDataPipeFrom
    public override IDataPipeFrom Create(IExcelSheet sheet)
    {
        #region 本地函数
        IDataPipeFrom Fun()
        {
            var (dataMap, options) = Initialization(sheet.Book);
            #region 用来枚举数据单元格的本地函数
            IEnumerable<IExcelCells> GetDatas(IExcelSheet sheet)
                => BlockGetDatas(options, dataMap).Select(pos => sheet[pos]).TakeWhile(BlockFilter);
            #endregion
            #region 用来读取数据的本地函数
            IData Get(IExcelCells cell)
            {
                var data = CreateDataObj.DataEmpty();
                foreach (var (key, position) in dataMap.ColumnBoundary)
                {
                    data[key] = BlockGet(options, key, cell[position]);
                }
                return data;
            }
            #endregion
            return CreateBlock.BlockFromComplex(sheet, GetDatas, Get);
        }
        #endregion
        var cache = CachePipe?.To<BlockFromComplex>();
        return cache is null || !ReferenceEquals(cache.Sheet.Book, sheet.Book) ?
            CachePipe = Fun() : cache with                      //如果工作表位于同一工作簿中，则直接使用缓存
            {
                Sheet = sheet
            };
    }
    #endregion 
}
