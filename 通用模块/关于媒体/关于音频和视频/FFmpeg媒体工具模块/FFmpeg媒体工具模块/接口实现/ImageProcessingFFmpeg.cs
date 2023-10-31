using System.Media.Drawing;

using Xabe.FFmpeg;

namespace System.Media.Play;

/// <summary>
/// 本接口是<see cref="IImageProcessing"/>的实现，
/// 可以用来处理图像
/// </summary>
sealed class ImageProcessingFFmpeg : IImageProcessing
{
    #region 格式转换
    public async Task FormatConversion(string imagePath, string targetPath, ISizePixel? maxSize = null, CancellationToken cancellation = default)
    {
        var scale = "";
        if (maxSize is { })
        {
            var mediaInfo = await FFmpeg.GetMediaInfo(imagePath, cancellation);
            var videoStream = mediaInfo.VideoStreams.SingleOrDefaultSecure();
            if (videoStream is null or { Width: 0 } or { Height: 0 })
                throw new NotSupportedException($"{imagePath}好像不是一张有效的图片");
            var size = CreateMath.SizePixel(videoStream.Width, videoStream.Height);
            var (w, _) = size.DimensionalityReduction(maxSize);
            var proportion = decimal.Divide(size.PixelCount.Horizontal, w);
            scale = $"-vf scale=iw/{proportion}:ih/{proportion}";
        }
        var arguments = $"-i {imagePath} {scale} {targetPath}";
        ToolIO.CreateFather(targetPath);
        await FFmpeg.Conversions.New().Start(arguments, cancellation);
    }
    #endregion 
}
