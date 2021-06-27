namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个像素点
    /// </summary>
    public interface IGraphicsPixel : IGraphics
    {
        #region 获取像素点的颜色
        /// <summary>
        /// 获取像素点的颜色
        /// </summary>
        IColor Color { get; }
        #endregion
    }
}
