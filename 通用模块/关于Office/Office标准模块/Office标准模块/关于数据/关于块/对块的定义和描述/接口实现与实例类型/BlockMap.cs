using System.Collections.Generic;
using System.Maths;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 这个类型是<see cref="IBlockMap"/>的实现，
    /// 可以用来描述块的特征
    /// </summary>
    class BlockMap : IBlockMap
    {
        #region 块的大小
        public ISizePixel Size { get; }
        #endregion
        #region 返回块是水平分布还是垂直分布
        public bool IsHorizontal { get; }
        #endregion
        #region 枚举读写属性的方式
        public IReadOnlyDictionary<string, IBlockProperty> Property { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="size">块的大小</param>
        /// <param name="isHorizontal">如果这个值为<see langword="true"/>，
        /// 代表块是水平分布，否则代表块是垂直分布</param>
        /// <param name="property">这个字典的键是属性的名称，值是用来读写属性的方式</param>
        public BlockMap(ISizePixel size, bool isHorizontal, IReadOnlyDictionary<string, IBlockProperty> property)
        {
            this.Size = size;
            this.IsHorizontal = isHorizontal;
            this.Property = property;
        }
        #endregion
    }
}
