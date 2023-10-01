using System.MathFrancis.Plane;
using System.Media.Drawing.Graphics;

namespace System.Media.Drawing;

/// <summary>
/// 这个类型是<see cref="IGraphicsPixel"/>的实现，
/// 可以被视为一个像素点
/// </summary>
/// <remarks>
/// 使用指定的颜色，位置和图层初始化像素点
/// </remarks>
/// <param name="color">像素点的颜色</param>
/// <param name="position">像素点的位置</param>
/// <param name="layer">像素点的图层</param>
sealed class GraphicsPixel(IColor color, IPoint position, int layer = 0) : IGraphicsPixel
{
    #region 获取像素点的颜色
    public IColor Color { get; } = color;
    #endregion
    #region 像素点的位置
    public IPoint Position { get; } = position;
    #endregion
    #region 像素点的图层
    public int Layer { get; } = layer;

    #endregion
}
