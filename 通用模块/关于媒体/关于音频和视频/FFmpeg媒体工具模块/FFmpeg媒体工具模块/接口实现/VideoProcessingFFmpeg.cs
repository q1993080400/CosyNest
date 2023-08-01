using System.IOFrancis;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.Maths;

using Xabe.FFmpeg;

namespace System.Media.Play;

/// <summary>
/// 本接口是<see cref="IVideoProcessing"/>的实现，
/// 它可以用来处理音频和视频
/// </summary>
sealed class VideoProcessingFFmpeg : IVideoProcessing
{
    #region 音视频混流
    #region 传入流
    public async Task<IBitRead> MixedFlow(IBitRead video, IBitRead audio)
    {
        var videoPath = ToolTemporaryFile.CreateTemporaryPath("mp4");
        var audioPath = ToolTemporaryFile.CreateTemporaryPath("mp3");
        await video.SaveToFile(videoPath);
        await audio.SaveToFile(audioPath);
        return await MixedFlow(CreateIO.File(videoPath), CreateIO.File(audioPath));
    }
    #endregion
    #region 传入文件
    public async Task<IBitRead> MixedFlow(IFile videoFile, IFile audioFile)
    {
        var output = ToolTemporaryFile.CreateTemporaryPath("mp4");
        var conversion = FFmpeg.Conversions.New().AddParameter($"-i {videoFile.Path} -i {audioFile.Path} -c:v copy -c:a aac -strict experimental {output}");
        await conversion.Start();
        return CreateIO.FileStream(output).ToBitPipe().Read;
    }
    #endregion
    #endregion
    #region 格式转换
    public async Task FormatConversion(string mediumPath, string targetPath, FormatConversionInfo? info = null)
    {
        info ??= new();
        var mediaInfo = await FFmpeg.GetMediaInfo(mediumPath);
        var videoStream = mediaInfo.VideoStreams.FirstOrDefault();
        var scale = "";
        if ((info, videoStream) is ({ MaxDefinition: { } definition }, { }))
        {
            var size = CreateMath.SizePixel(videoStream.Width, videoStream.Height);
            var (w, _) = size.DimensionalityReduction(definition);
            var proportion = decimal.Divide(size.PixelCount.Horizontal, w);
            scale = $"-vf scale=iw/{proportion}:ih/{proportion}";
        }
        var arguments = $"-i {mediumPath} -c:v libx264 -c:a aac {scale} {targetPath}";
        ToolIO.CreateFather(targetPath);
        await FFmpeg.Conversions.New().Start(arguments, info.CancellationToken);
    }
    #endregion
    #region 视频截图
    public async Task Screenshot(string mediumPath, IEnumerable<(TimeSpan Fragment, string Path)> infos, Func<decimal, Task>? reportProgress = null, CancellationToken cancellationToken = default)
    {
        var snippet = FFmpeg.Conversions.FromSnippet;
        reportProgress ??= _ => Task.CompletedTask;
        foreach (var ((fragment, output), index, count) in infos.PackIndex(true))
        {
            ToolIO.CreateFather(output);
            var conversion = await snippet.Snapshot(mediumPath, output, fragment);
            await conversion.Start(cancellationToken);
            await reportProgress(decimal.Divide(index + 1, count));
        }
    }
    #endregion
}
