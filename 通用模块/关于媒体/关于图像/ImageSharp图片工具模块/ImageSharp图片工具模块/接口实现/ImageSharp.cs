using System.IOFrancis;
using System.IOFrancis.Bit;
using System.Maths;
using System.Maths.Plane;
using System.Media.Drawing.Graphics;

using static System.Net.Mime.MediaTypeNames;

using BottomLayerImage = SixLabors.ImageSharp.Image;

namespace System.Media.Drawing;

/// <summary>
/// 该类型表示一个通过ImageSharp实现的图像
/// </summary>
class ImageSharp : FromIO, IImage
{
    #region 封装的图像对象
    /// <summary>
    /// 获取封装的图像对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected Image<Rgba32> Image { get; }
    #endregion
    #region 枚举图像细节
    public IEnumerable<IGraphics> Details => throw new NotImplementedException();
    #endregion
    #region 关于图片本身
    #region 图片的大小
    public ISize Size
        => CreateMath.Size(Image.Width, Image.Height);
    #endregion
    #region 图片的格式
    protected override string FormatTemplate { get; }
    #endregion
    #endregion
    #region 关于保存和释放图片
    #region 保存对象
    protected override async Task SaveRealize(string path, bool isSitu)
    {
        Directory.CreateDirectory(IO.Path.GetDirectoryName(path)!);
        await Image.SaveAsync(path);
    }
    #endregion
    #region 返回读取对象的流
    public IBitRead Read()
    {
        var stream = new MemoryStream();
        Action<BottomLayerImage, Stream> save = Format switch
        {
            "jpeg" or "jpg" => ImageExtensions.SaveAsJpeg,
            "png" => ImageExtensions.SaveAsPng,
            "bmp" => ImageExtensions.SaveAsJpeg,
            "gif" => ImageExtensions.SaveAsGif,
            "tga" => ImageExtensions.SaveAsTga,
            var f => throw new NotSupportedException($"未识别{f}格式的图像")
        };
        save(Image, stream);
        stream.Reset();
        return stream.ToBitPipe().Read;
    }
    #endregion
    #region 释放对象
    protected override ValueTask DisposeAsyncActualRealize()
    {
        Image.Dispose();
        return ValueTask.CompletedTask;
    }
    #endregion
    #endregion
    #region 构造函数
    #region 直接传入图像
    /// <summary>
    /// 直接传入图像和格式，并创建对象
    /// </summary>
    /// <param name="image">封装的图像</param>
    /// <param name="format">图像的格式</param>
    protected ImageSharp(BottomLayerImage image, string format)
        : base(null, CreateImageSharp.Support)
    {
        Image = image.CloneAs<Rgba32>();
        FormatTemplate = format;
        image.Dispose();
    }
    #endregion
    #region 使用流
    /// <summary>
    /// 从指定的流初始化对象
    /// </summary>
    /// <param name="stream">图片所在的流</param>
    public ImageSharp(Stream stream)
        : base(null, CreateImageSharp.Support)
    {
        using var cacheImage = BottomLayerImage.Load(stream);
        Image = cacheImage.CloneAs<Rgba32>();
        stream.Reset();
        FormatTemplate = BottomLayerImage.DetectFormat(stream).FileExtensions.First();
    }
    #endregion
    #endregion
}
