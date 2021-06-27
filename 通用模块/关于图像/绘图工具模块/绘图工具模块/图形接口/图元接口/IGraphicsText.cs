namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为画布上的一个文本
    /// </summary>
    public interface IGraphicsText : IGraphicsHasStyle
    {
        #region 获取或设置样式
        /// <summary>
        /// 获取或设置图形的样式
        /// </summary>
        new IGraphicsStyleText Style { get; set; }
        #endregion
        #region 获取或设置文本
        /// <summary>
        /// 获取或设置文本
        /// </summary>
        string? Text { get; set; }
        #endregion
    }
}
