using System.Collections.Generic;
using System.Linq;
using System.Office.Excel;
using System.Threading.Tasks;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 这个类型是<see cref="IDataPipeAdd"/>的实现，
    /// 可以通过块向Excel工作簿提交数据
    /// </summary>
    class DataPipeAddBlock : IDataPipeAdd
    {
        #region 获取是否支持绑定
        public bool CanBinding => true;
        #endregion
        #region 添加数据所需要的信息
        #region 块地图
        /// <summary>
        /// 获取块地图，它描述块的特征
        /// </summary>
        private IBlockMap Map { get; }
        #endregion
        #region 用来返回下一个块的枚举器
        /// <summary>
        /// 这个枚举器用来返回下一个块
        /// </summary>
        private IEnumerator<IExcelCells> Next { get; }
        #endregion
        #endregion
        #region 同步添加数据
        #region 辅助方法
        #region 写入标题
        /// <summary>
        /// 辅助方法，向块中写入标题
        /// </summary>
        private void SetTitle()
        {
            var title = Map.Property.Values.Select(x => x.SetTitle).Where(x => x is { }).ToArray();
            if (title.Any())
            {
                var isH = Map.IsHorizontal;
                var titleCell = Next.Current.Offset(right: isH ? -1 : 0, down: isH ? 0 : -1);
                title.ForEach(x => x!(titleCell));
            }
        }
        #endregion
        #region 遍历并添加数据
        /// <summary>
        /// 辅助方法，遍历并添加数据，然后返回数据的数量
        /// </summary>
        /// <param name="datas">待添加的数据</param>
        /// <param name="binding">如果这个值为<see langword="true"/>，
        /// 则在添加数据的时候，还会将数据绑定</param>
        /// <returns></returns>
        private int DataAddAided(IEnumerable<IData> datas, bool binding)
            => datas.Aggregate(0, (seed, data) =>
            {
                var current = Next.Current;
                foreach (var (name, (_, set, _)) in Map.Property)
                {
                    set(current, data[name]);
                }
                if (binding)
                    data.Binding = new BlockBinding(current, Map.Property);
                Next.MoveNext();
                return ++seed;
            });
        #endregion
        #endregion
        #region 正式方法
        public Task Add(IEnumerable<IData> datas, bool binding)
        {
            Next.MoveNext();
            var first = Next.Current;
            SetTitle();
            var (_, bc, _, ec) = first.Address;
            var count = DataAddAided(datas, binding);
            var end = Map.IsHorizontal ? bc + count * Map.Size.PixelCount.Horizontal : ec;
            ToolException.Ignore<NotImplementedException>
                (() => first.Sheet.GetRC(bc, end, false).AutoFit());            //自动适配列的大小，提升可读性
            return Task.CompletedTask;
        }

        /*问：为什么在适配列的大小的时候，要忽略掉NotImplementedException异常？
          答：因为AutoFit是非关键API，某些Excel模块可能没有实现它，
          如果因为这个，让它们不能使用块来提取数据是不恰当的*/
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="map">块地图，它描述块的特征</param>
        /// <param name="next">这个枚举器用来返回下一个块</param>
        public DataPipeAddBlock(IBlockMap map, IEnumerator<IExcelCells> next)
        {
            this.Map = map;
            this.Next = next;
        }
        #endregion
    }
}
