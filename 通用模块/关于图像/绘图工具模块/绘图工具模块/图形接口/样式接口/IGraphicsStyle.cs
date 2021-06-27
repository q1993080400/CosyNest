namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个基本图形样式
    /// </summary>
    public interface IGraphicsStyle
    {
        #region 填充颜色
        /// <summary>
        /// 获取或设置这个样式的填充颜色
        /// </summary>
        IColor Fill { get; set; }
        #endregion
        #region 描边颜色
        /// <summary>
        /// 获取或设置这个样式的描边颜色
        /// </summary>
        IColor Stroke { get; set; }
        #endregion
    }
}
