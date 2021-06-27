namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个具有样式的图形
    /// </summary>
    public interface IGraphicsHasStyle : IGraphicsVar
    {
        #region 获取样式
        /// <summary>
        /// 获取图形的样式
        /// </summary>
        IGraphicsStyle Style { get; }
        #endregion
    }
}
