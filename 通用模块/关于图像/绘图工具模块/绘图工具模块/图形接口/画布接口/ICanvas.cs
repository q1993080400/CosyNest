using System.Collections.Generic;

namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个画布，它可以用来绘制和枚举图像
    /// </summary>
    public interface ICanvas
    {
        #region 返回画布的细节
        /// <summary>
        /// 返回画布的细节，
        /// 也就是组成画布的所有图形，
        /// 被添加到这个集合中的图形会被绘制在画布上
        /// </summary>
        ICollection<IGraphics> Details { get; }
        #endregion
        #region 获取图形创建器
        /// <summary>
        /// 获取一个图形创建器，
        /// 它可以用来创建和这个画布兼容的基本图形
        /// </summary>
        ICreateGraphics CreateGraphics { get; }
        #endregion
    }
}
