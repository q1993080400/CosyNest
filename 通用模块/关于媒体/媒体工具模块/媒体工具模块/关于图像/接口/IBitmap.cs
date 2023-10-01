using System.MathFrancis;
using System.MathFrancis.Plane;

namespace System.Media.Drawing;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个位图
/// </summary>
public interface IBitmap : IImage
{
    #region 获取图片大小
    /// <summary>
    /// 获取图片的大小
    /// </summary>
    new ISizePixel Size
        => this.To<IImage>().Size.ToSizePixel(CreateMath.Size(1, 1), false);
    #endregion
    #region 获取图像的每个像素点
    /// <summary>
    /// 获取图像的每个像素点
    /// </summary>
    /// <returns></returns>
    IColor[,] Pixel();
    #endregion
}
