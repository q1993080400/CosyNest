using System.Maths.Plane;
using System.Office.Excel;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 本类型是一个最简单的<see cref="IDataPipeFrom"/>实现，
/// 它从固定的位置开始，在工作表中垂直移动提取数据，
/// 直到找不到新的数据为止
/// </summary>
sealed class BlockFromSimple : IDataPipeFrom
{
    #region 数据所在的工作表
    /// <summary>
    /// 返回数据所在的工作表
    /// </summary>
    private IExcelSheet Sheet { get; }
    #endregion
    #region 第一条数据的位置
    /// <summary>
    /// 返回第一条数据所在的位置
    /// </summary>
    private ISizePosPixel First { get; }
    #endregion
    #region 枚举数据的列
    /// <summary>
    /// 按照顺序，依次枚举数据的列名以及是否必填，
    /// 如果出现了没有必填数据的数据，则会停止提取
    /// </summary>
    private IEnumerable<BlockSimpleColumn> Columns { get; }
    #endregion
    #region 拉取数据
    IQueryable<Data> IDataPipeFrom.Query<Data>()
    {
        #region 本地函数
        IEnumerable<IData> Fun()
        {
            var create = IDataPipe.CreateData<Data>();
            var position = First;
            while (true)
            {
                var cell = Sheet[position];
                var data = create();
                foreach (var ((columnName, isRequired, getValue), index, _) in Columns.PackIndex())
                {
                    var value = getValue(cell[0, index]);
                    if (isRequired && value is null)
                        yield break;
                    data[columnName] = value;
                }
                yield return data;
                position = position.Transform(offsetTop: -1);
            }
        }
        #endregion
        return Fun().CastEntity<Data>().AsQueryable();
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="sheet">数据所在的工作表</param>
    /// <param name="first">第一条数据所在的位置</param>
    /// <param name="columns">按照顺序，依次枚举数据的列，
    /// 如果出现了某一必填数据为<see langword="null"/>的数据，则会停止提取</param>
    public BlockFromSimple(IExcelSheet sheet, ISizePosPixel first,
        IEnumerable<BlockSimpleColumn> columns)
    {
        this.Sheet = sheet;
        this.First = first;
        this.Columns = columns.ToArray();
    }
    #endregion
}
