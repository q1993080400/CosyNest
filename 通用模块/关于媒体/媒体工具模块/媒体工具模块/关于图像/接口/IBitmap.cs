using System.Maths;
using System.Maths.Plane;

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
    #region 生成缩略图
    /// <summary>
    /// 生成一个缩略图，并返回
    /// </summary>
    /// <param name="maxSize">缩略图的最大大小，
    /// 函数会保证长宽比不会改变</param>
    /// <returns></returns>
    IBitmap Thumbnail(ISizePixel maxSize);
    #endregion
}
