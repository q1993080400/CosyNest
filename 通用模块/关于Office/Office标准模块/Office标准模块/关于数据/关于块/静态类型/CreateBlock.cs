using System.Collections.Generic;
using System.Linq;
using System.Maths;
using System.Office.Excel;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 这个静态类可以帮助创建块
    /// </summary>
    public static class CreateBlock
    {
        #region 创建IBlockProperty
        #region 直接创建
        /// <summary>
        /// 使用指定的读取属性的委托，写入属性的委托和写入标题的委托初始化<see cref="IBlockProperty"/>
        /// </summary>
        /// <param name="getValue">用来读取属性的委托</param>
        /// <param name="setValue">用来写入属性的委托</param>
        /// <param name="setTitle">用来写入标题的委托</param>
        /// <returns></returns>
        public static IBlockProperty BlockProperty
            (Func<IExcelCells, object?> getValue, Action<IExcelCells, object?> setValue, Action<IExcelCells>? setTitle = null)
            => new BlockProperty(getValue, setValue, setTitle);
        #endregion
        #region 指定位置，读取委托，写入委托，标题
        /// <summary>
        /// 指定属性位于块的指定位置，并通过指定的委托读写属性
        /// </summary>
        /// <param name="row">属性在块中的行号</param>
        /// <param name="col">属性在块中的列号</param>
        /// <param name="getValue">用来读取属性的委托，参数就是已经定位好的属性单元格</param>
        /// <param name="setValue">用来写入属性的委托，参数就是已经定位好的属性单元格，以及属性的新值</param>
        /// <param name="title">属性的标题，它将通过<see cref="IExcelCells.Value"/>进行写入</param>
        /// <returns></returns>
        public static IBlockProperty BlockProperty
            (int row, int col, Func<IExcelCells, object?> getValue, Action<IExcelCells, object?> setValue, string? title = null)
            => BlockProperty
            (cell => getValue(cell[row, col]),
                (cells, value) => setValue(cells[row, col], value),
                title is null ? null : new Action<IExcelCells>(cell => cell[row, col].Value = title));
        #endregion
        #region 指定位置，并利用Value读写属性
        /// <summary>
        /// 指定属性位于块的指定位置，并通过<see cref="IExcelCells.Value"/>来读写属性
        /// </summary>
        /// <param name="row">属性在块中的行号</param>
        /// <param name="col">属性在块中的列号</param>
        /// <param name="title">属性的标题，它将通过<see cref="IExcelCells.Value"/>进行写入</param>
        /// <returns></returns>
        public static IBlockProperty BlockProperty(int row, int col, string? title = null)
            => BlockProperty(row, col,
                cell => cell.Value?.Content,
                (cell, value) => cell.Value = new(value), title);
        #endregion
        #endregion
        #region 创建IBlockMap
        #region 直接创建
        /// <summary>
        /// 直接创建一个<see cref="IBlockMap"/>，并返回
        /// </summary>
        /// <param name="size">块的大小</param>
        /// <param name="isHorizontal">如果这个值为<see langword="true"/>，
        /// 代表块是水平分布，否则代表块是垂直分布</param>
        /// <param name="property">这个字典的键是属性的名称，值是用来读写属性的方式</param>
        /// <returns></returns>
        public static IBlockMap Map(ISizePixel size, bool isHorizontal, IReadOnlyDictionary<string, IBlockProperty> property)
            => new BlockMap(size, isHorizontal, property);
        #endregion
        #region 自动推断块的大小
        /// <summary>
        /// 创建一个<see cref="IBlockMap"/>，
        /// 块的大小可以通过属性数量推断出来
        /// </summary>
        /// <param name="isHorizontal">如果这个值为<see langword="true"/>，
        /// 代表块是水平分布，否则代表块是垂直分布</param>
        /// <param name="property">这个字典的键是属性的名称，值是用来读写属性的方式</param>
        /// <returns></returns>
        public static IBlockMap Map(bool isHorizontal, IReadOnlyDictionary<string, IBlockProperty> property)
            => Map(CreateMath.SizePixel
                (isHorizontal ? 1 : property.Count,
                isHorizontal ? property.Count : 1),
                isHorizontal, property);
        #endregion
        #region 属性依次排列，并通过Value进行读写
        /// <summary>
        /// 创建一个<see cref="IBlockMap"/>，
        /// 且块的所有属性通过<see cref="IExcelCells.Value"/>进行读写
        /// </summary>
        /// <param name="isHorizontal">如果这个值为<see langword="true"/>，
        /// 代表块是水平分布，否则代表块是垂直分布</param>
        /// <param name="property">这个数组枚举属性的名称和标题</param>
        /// <returns></returns>
        public static IBlockMap Map(bool isHorizontal, params (string Name, string? Title)[] property)
            => Map(isHorizontal, property.PackIndex().ToDictionary
                (x => x.Elements.Name,
                x => BlockProperty
                (isHorizontal ? x.Index : 0,
                isHorizontal ? 0 : x.Index,
                x.Elements.Title)));
        #endregion
        #endregion
        #region 创建用于定位下一个块的委托
        #region 预设值：按照水平或垂直方向依次移动
        #region 通过委托获取第一个块
        /// <summary>
        /// 创建一个用于获取下一个块的枚举器，
        /// 它从第一个块开始，按照水平或垂直方向依次移动，以获取下一个块
        /// </summary>
        /// <param name="map">描述块特征的地图</param>
        /// <param name="first">用来获取第一个块位置的委托，
        /// 它的参数就是块所在的工作簿</param>
        /// <param name="source">块所在的工作簿</param>
        /// <returns></returns>
        public static IEnumerator<IExcelCells> Next(IBlockMap map, Func<IExcelBook, IExcelCells> first, IExcelBook source)
        {
            #region 本地函数
            IEnumerable<IExcelCells> Fun()
            {
                var (w, h) = map.Size;
                var horizontal = map.IsHorizontal;
                var current = first(source).Offset(w, h, 0, 0);
                yield return current;
                while (true)
                {
                    yield return current = current.Offset(right: horizontal ? w : 0, down: horizontal ? 0 : h);
                }
            }
            #endregion
            return Fun().GetEnumerator();
        }
        #endregion
        #region 直接指定第一个块
        /// <summary>
        /// 创建一个用于获取下一个块的枚举器，
        /// 它从第一个块开始，按照水平或垂直方向依次移动，以获取下一个块
        /// </summary>
        /// <param name="map">描述块特征的地图</param>
        /// <param name="first">第一个块所在的位置</param>
        /// <returns></returns>
        public static IEnumerator<IExcelCells> Next(IBlockMap map, IExcelCells first)
            => Next(map, x => first, first.Book);
        #endregion
        #endregion
        #endregion
        #region 创建IDataPipeAdd
        /// <summary>
        /// 创建一个<see cref="IDataPipeAdd"/>，并返回，
        /// 它可以通过块向Excel工作簿添加数据
        /// </summary>
        /// <param name="map">块地图，它描述块的特征</param>
        /// <param name="next">这个枚举器用来返回下一个块</param>
        /// <returns></returns>
        public static IDataPipeAdd PipeAdd(IBlockMap map, IEnumerator<IExcelCells> next)
            => new DataPipeAddBlock(map, next);
        #endregion
        #region 创建IDataPipeQuery
        /// <summary>
        /// 创建一个<see cref="IDataPipeQuery"/>，并返回，
        /// 它可以通过块从Excel工作簿中查询数据
        /// </summary>
        /// <param name="map">块地图，它描述块的特征</param>
        /// <param name="extraction">这个委托被用来从Excel工作簿中提取块</param>
        /// <param name="source">获取数据所在的工作簿</param>
        /// <returns></returns>
        public static IDataPipeQuery PipeQuery(IBlockMap map, ExtractionBlock extraction, params IExcelBook[] source)
            => new DataPipeQueryBlock(map, extraction, source);
        #endregion
    }
}
