using System.Maths;
using System.Maths.Plane;
using System.Office.Excel;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 该类型可以用来帮助创建与块有关的对象，
/// 它可以从Excel中拉取或推送数据
/// </summary>
public static class CreateBlock
{
    #region 有关推送数据
    #region 创建IDataPipeTo
    #region 复杂方法
    /// <summary>
    /// 创建一个<see cref="IDataPipeTo"/>，
    /// 它可以将数据推送到Excel
    /// </summary>
    /// <inheritdoc cref="BlockToComplex(IExcelSheet, BlockFind, BlockSet, BlockFilter?)"/>
    public static IDataPipeTo BlockTo(IExcelSheet sheet, BlockFind blockFind, BlockSet blockSet, BlockFilter? blockFilter = null)
        => new BlockToComplex(sheet, blockFind, blockSet, blockFilter);
    #endregion
    #region 极简方法
    /// <summary>
    /// 创建一个可以将数据推送到Excel的<see cref="IDataPipeTo"/>，
    /// 它使用最简单的方式对数据进行定位，那就是每一列表示一个字段，每一行表示一条数据
    /// </summary>
    /// <param name="setValue">这个委托被用来向单元格写入字段，
    /// 它的第一个参数是字段的名称，第二个参数是字段的值，
    /// 第三个参数是待写入的数据，可以用它访问数据的其他属性，
    /// 第四个参数是字段所在的单元格，如果为<see langword="null"/>，
    /// 则使用默认方法</param>
    /// <param name="setTitle">这个委托被用来写入标题，
    /// 它的第一个参数是标题的名称，第二个参数是标题所在的单元格</param>
    /// <returns></returns>
    /// <inheritdoc cref="BlockToComplex(IExcelSheet, BlockFind, BlockSet, BlockFilter?)"/>
    public static IDataPipeTo BlockToSimple(IExcelSheet sheet,
        Action<string, object?, IData, IExcelCells>? setValue = null,
        Action<string, IExcelCells>? setTitle = null)
    {
        var row = -1;
        setValue ??= (_, value, _, cell) => cell.Value = new(value);
        setTitle ??= (name, cell) => cell.Value = name;
        return BlockTo(sheet,
            data => CreateMath.SizePosPixel(0, row--, data.Count, 1),
            (data, cell) =>
            {
                var pros = data.Sort(x => x.Key).ToArray();
                if (row is -2)
                {
                    var topCell = cell.Transform(-1);
                    pros.Select(x => x.Key).Zip(topCell.Cells).ForEach(x => setTitle(x.First, x.Second));
                }
                pros.Zip(cell.Cells).ForEach(x => setValue(x.First.Key, x.First.Value, data, x.Second));
            });
    }
    #endregion
    #endregion
    #region 创建BlockFind
    /// <summary>
    /// 通过数据地图创建一个<see cref="Block.BlockFind"/>，
    /// 每调用一次本方法返回的委托，都会将数据位置向前推动一次
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static BlockFind BlockFind(DataMap map)
    {
        var count = 0;
        return data => map.GetPosition(count++);
    }

    /*问：将数据位置向前推动是什么意思？
      答：假设返回的委托为fun()，且每条数据只有一行一列，
      从(0,0)开始，依次向下垂直排列，
      则第一次调用fun()返回(0,0)，
      第二次调用返回(1,0)，
      第三次调用返回(2,0)，依此类推*/
    #endregion
    #endregion
    #region 有关拉取数据
    #region 简单实现
    #region 指定完整的列元数据
    /// <summary>
    /// 创建一个最简单的<see cref="IDataPipeFrom"/>，
    /// 它从固定的位置开始，在工作表中垂直移动提取数据，
    /// 直到找不到新的数据为止
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="BlockFromSimple.BlockFromSimple(IExcelSheet, ISizePosPixel, IEnumerable{BlockSimpleColumn})"/>
    public static IDataPipeFrom BlockFromSimple(IExcelSheet sheet, ISizePosPixel first,
        IEnumerable<BlockSimpleColumn> columns)
        => new BlockFromSimple(sheet, first, columns);
    #endregion
    #region 仅指定列名和是否必填
    /// <inheritdoc cref="BlockFromSimple(IExcelSheet, ISizePosPixel, IEnumerable{BlockSimpleColumn})"/>
    public static IDataPipeFrom BlockFromSimple(IExcelSheet sheet, ISizePosPixel first,
        params (string ColumnsName, bool IsRequired)[] columns)
        => new BlockFromSimple(sheet, first,
            columns.Select(x => new BlockSimpleColumn()
            {
                ColumnName = x.ColumnsName,
                IsRequired = x.IsRequired
            }));
    #endregion
    #region 仅指定列名
    /// <param name="columnsNames">列的名称，它们全部为必填，
    /// 如果数据的任意一个列为<see langword="null"/>，则停止提取</param>
    /// <inheritdoc cref="BlockFromSimple(IExcelSheet, ISizePosPixel, IEnumerable{BlockSimpleColumn})"/>
    public static IDataPipeFrom BlockFromSimple(IExcelSheet sheet, ISizePosPixel first, params string[] columnsNames)
        => BlockFromSimple(sheet, first, columnsNames.Select(x => (x, true)).ToArray());
    #endregion
    #endregion
    #region 复杂实现
    /// <summary>
    /// 创建一个<see cref="IDataPipeFrom"/>，
    /// 它可以从工作表中拉取数据
    /// </summary>
    /// <inheritdoc cref="BlockFromComplex.BlockFromComplex(IExcelSheet, BlockGetDatas, BlockGet)"/>
    public static IDataPipeFrom BlockFromComplex(IExcelSheet sheet, BlockGetDatas blockGetDatas, BlockGet blockGet)
        => new BlockFromComplex(sheet, blockGetDatas, blockGet);
    #endregion
    #endregion
}
