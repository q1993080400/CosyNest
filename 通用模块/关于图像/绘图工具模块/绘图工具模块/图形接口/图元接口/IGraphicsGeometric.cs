using System.Maths.Geometric;

namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视作一个画布上的几何图形
    /// </summary>
    /// <typeparam name="GraphicsGeometric">画布上的几何图形的类型</typeparam>
    public interface IGraphicsGeometric<GraphicsGeometric> : IGraphicsHasStyle
        where GraphicsGeometric : IGeometric
    {
        #region 获取或设置样式
        /// <summary>
        /// 获取或设置图形的样式
        /// </summary>
        new IGraphicsStyleShape Style { get; set; }
        #endregion
        #region 获取几何图形
        /// <summary>
        /// 获取画布所要呈现的几何图形
        /// </summary>
        GraphicsGeometric Geometric { get; }
        #endregion
    }
}
