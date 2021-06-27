using System.Collections.Generic;
using System.Linq;

namespace System.Maths.Geometric
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个多边形
    /// </summary>
    public interface IPolygon : IGeometric
    {
        #region 获取多边形的边数
        /// <summary>
        /// 获取多边形的边的数量
        /// </summary>
        int EdgeCount
            => Content.Count();
        #endregion
        #region 获取多边形的顶点
        /// <summary>
        /// 枚举多边形的所有顶点
        /// </summary>
        IEnumerable<IPoint> Vertex
            => Content.Select(x => x.Node).UnionNesting(true);
        #endregion
    }
}
