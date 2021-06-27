using System.Collections.Generic;
using System.Maths;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来描述块的基本特征
    /// </summary>
    public interface IBlockMap
    {
        #region 块的大小
        /// <summary>
        /// 获取块的大小
        /// </summary>
        ISizePixel Size { get; }
        #endregion
        #region 返回块是水平分布还是垂直分布
        /// <summary>
        /// 如果这个值为<see langword="true"/>，
        /// 代表块是水平分布，否则代表块是垂直分布
        /// </summary>
        bool IsHorizontal { get; }
        #endregion
        #region 枚举读写属性的方式
        /// <summary>
        /// 获取一个字典，它的键是属性的名称，
        /// 值是用来读写属性的方式
        /// </summary>
        IReadOnlyDictionary<string, IBlockProperty> Property { get; }
        #endregion
    }
}
