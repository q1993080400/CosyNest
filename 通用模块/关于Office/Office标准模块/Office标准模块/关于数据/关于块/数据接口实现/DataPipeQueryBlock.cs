using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Office.Excel;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 这个类型是<see cref="IDataPipeQuery"/>的实现，
    /// 可以通过块从Excel工作簿中提取数据
    /// </summary>
    class DataPipeQueryBlock : IDataPipeQuery
    {
        #region 获取是否支持绑定
        public bool CanBinding => true;
        #endregion
        #region 查询数据所需要的信息
        #region 数据所在的工作簿
        /// <summary>
        /// 获取数据所在的工作簿
        /// </summary>
        private IEnumerable<IExcelBook> Source { get; }
        #endregion
        #region 块地图
        /// <summary>
        /// 获取块地图，它描述块的特征
        /// </summary>
        private IBlockMap Map { get; }
        #endregion
        #region 用来提取块的委托
        /// <summary>
        /// 这个委托被用来从Excel工作簿中提取块
        /// </summary>
        private ExtractionBlock Extraction { get; }
        #endregion
        #endregion
        #region 查询数据
        public IEnumerableFit<IData> Query(Expression<Func<PlaceholderData, bool>>? expression, bool binding)
            => Extraction(Source, Map).Select(cell =>
            {
                var data = CreateDataObj.Data(Map.Property.Keys.ToArray());
                foreach (var (name, (get, _, _)) in Map.Property)
                {
                    data[name] = get(cell);
                }
                if (binding)
                    data.Binding = new BlockBinding(cell, Map.Property);
                return data;
            }).Fit();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="map">块地图，它描述块的特征</param>
        /// <param name="extraction">这个委托被用来从Excel工作簿中提取块</param>
        /// <param name="source">获取数据所在的工作簿</param>
        public DataPipeQueryBlock(IBlockMap map, ExtractionBlock extraction, params IExcelBook[] source)
        {
            this.Map = map;
            this.Extraction = extraction;
            this.Source = source;
        }
        #endregion
    }
    #region 用来提取块的委托
    /// <summary>
    /// 这个委托被用来从Excel工作簿中提取块
    /// </summary>
    /// <param name="source">块所在的工作簿</param>
    /// <param name="map">块地图，它描述块的特征</param>
    /// <returns></returns>
    public delegate IEnumerable<IExcelCells> ExtractionBlock(IEnumerable<IExcelBook> source, IBlockMap map);
    #endregion
}
