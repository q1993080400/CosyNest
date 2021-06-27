namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视作一个画布上的形状的样式
    /// </summary>
    public interface IGraphicsStyleShape : IGraphicsStyle
    {
        #region 描边宽度
        /// <summary>
        /// 获取或设置描边宽度
        /// </summary>
        Num StrokeWidth { get; set; }
        #endregion
    }
}
