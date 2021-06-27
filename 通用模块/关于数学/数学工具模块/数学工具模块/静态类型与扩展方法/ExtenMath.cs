using System.Collections.Generic;
using System.Linq;
using System.Maths.Geometric;

namespace System.Maths
{
    /// <summary>
    /// 有关数学的扩展方法全部放在这里
    /// </summary>
    public static class ExtenMath
    {
        #region 关于IBessel
        #region 返回多条贝塞尔曲线的全部坐标
        /// <summary>
        /// 返回多条贝塞尔曲线的全部控制点坐标
        /// </summary>
        /// <param name="bessels">待分解的贝塞尔曲线集合</param>
        /// <param name="distinct">如果这个值为<see langword="true"/>，还会去除重复的坐标</param>
        /// <returns></returns>
        public static IEnumerable<IPoint> AllPoint(this IEnumerable<IBessel> bessels, bool distinct = false)
            => bessels.Select(x => x.Node).UnionNesting(distinct);
        #endregion
        #endregion
    }
}
