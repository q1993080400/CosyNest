using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace System.Media.Drawing;

/// <summary>
/// 这个静态类可以用来创建和图像有关的对象，
/// 它们在底层使用ImageSharp实现
/// </summary>
public static class CreateImageSharp
{
    #region 获取支持的文件类型
    /// <summary>
    /// 获取受支持的图片文件类型
    /// </summary>
    public static IFileType Support { get; }
    = CreateIO.FileType("受ImageSharp支持的文件类型",
        "jpeg", "jpg", "png", "bmp", "gif", "tga");
    #endregion
    #region 创建图像对象
    #region 使用路径
    /// <param name="path">图像所在的路径</param>
    /// <inheritdoc cref="Image{Image}(Stream)"/>
    public static Image Image<Image>(PathText path)
        where Image : IImage
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return Image<Image>(stream);
    }
    #endregion
    #region 使用流
    /// <summary>
    /// 创建一个底层使用ImageSharp实现的图像
    /// </summary>
    /// <typeparam name="Image">返回值类型，
    /// 它仅支持<see cref="IImage"/>和<see cref="IBitmap"/></typeparam>
    /// <returns></returns>
    /// <inheritdoc cref="ImageSharp(Stream)"/>
    /// <exception cref="NotSupportedException">泛型参数不正确，它只支持<see cref="IImage"/>和<see cref="IBitmap"/></exception>
    public static Image Image<Image>(Stream stream)
        where Image : IImage
    {
        var type = typeof(Image);
        if (type == typeof(IImage))
            return new ImageSharp(stream).To<Image>();
        if (type == typeof(IBitmap))
            return new ImageSharpBit(stream).To<Image>();
        throw new NotSupportedException($"泛型参数只支持{nameof(IImage)}和{nameof(IBitmap)}两种类型");
    }
    #endregion
    #endregion
}
