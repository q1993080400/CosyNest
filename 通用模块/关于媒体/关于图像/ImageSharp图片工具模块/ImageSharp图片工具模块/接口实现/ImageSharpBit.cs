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
    #region 构造函数
    #region 传入流
    /// <inheritdoc cref="ImageSharp(Stream)"/>
    public ImageSharpBit(Stream stream)
        : base(stream)
    {

    }
    #endregion
    #endregion
}
