using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using System.Maths.Plane;

using BottomLayerImage = SixLabors.ImageSharp.Image;

namespace System.Media.Drawing;

/// <summary>
/// 这个类型是底层使用ImageSharp实现的位图
/// </summary>
sealed class ImageSharpBit : ImageSharp, IBitmap
{
    #region 枚举图像像素
    public IColor[,] Pixel()
    {
        var (w, h) = (Image.Width, Image.Height);
        var array = new IColor[w, h];
        for (int r = 0; r < h; r++)
        {
            for (int c = 0; c < w; c++)
            {
                var color = Image[c, r];
                array[c, r] = CreateDrawingObj.Color(color.R, color.G, color.B, color.A);
            }
        }
        return array;
    }
    #endregion
    #region 生成缩略图
    public IBitmap Thumbnail(ISizePixel maxSize)
    {
        var image = BottomLayerImage.Load(Read().ToStream());
        var (h, v) = maxSize;
        var mod = new ResizeOptions()
        {
            Mode = ResizeMode.Max,
            Size = new(h, v)
        };
        image.Mutate(x => x.Resize(mod));
        return new ImageSharpBit(image, Format);
    }
    #endregion
    #region 构造函数
    #region 直接传入图像
    /// <inheritdoc cref="ImageSharp(BottomLayerImage, string)"/>
    private ImageSharpBit(BottomLayerImage image, string format)
        : base(image, format)
    {

    }
    #endregion
    #region 传入流
    /// <inheritdoc cref="ImageSharp(Stream)"/>
    public ImageSharpBit(Stream stream)
        : base(stream)
    {

    }
    #endregion
    #endregion
}
