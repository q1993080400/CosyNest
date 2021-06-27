using System.Maths;

namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个可变图形
    /// </summary>
    public interface IGraphicsVar : IGraphics
    {
        #region 获取或设置图形的位置
        /// <summary>
        /// 获取或设置图形的位置
        /// </summary>
        new IPoint Position { get; set; }
        #endregion
        #region 获取或设置图形的图层
        /// <summary>
        /// 获取或设置图形的图层，
        /// 越小代表越接近上方，最小为0
        /// </summary>
        new int Layer { get; set; }
        #endregion
    }
}
