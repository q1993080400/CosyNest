using System.Maths;

namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视为画布上的一个图形，它是不可变的
    /// </summary>
    public interface IGraphics
    {
        #region 图形的位置
        /// <summary>
        /// 获取图形的位置
        /// </summary>
        IPoint Position { get; }
        #endregion
        #region 图形的图层
        /// <summary>
        /// 获取图形的图层，
        /// 越小代表越接近上方，最小为0
        /// </summary>
        int Layer { get; }
        #endregion
    }
}
