using System.Collections.Generic;

namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个组合的图形，
    /// 它由若干更小的图形组合而成
    /// </summary>
    public interface IGraphicsCombination : IGraphics
    {
        #region 获取子图形
        /// <summary>
        /// 获取该图形的所有子图形
        /// </summary>
        IEnumerable<IGraphics> Son { get; }
        #endregion
    }
}
