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
    #region 传入流
    public async Task FormatConversion(IBitRead read, string currentFormat, string targetPath, FormatConversionInfo? info = null)
    {
        await SaveAndProcess(read, currentFormat,
            x => FormatConversion(x, targetPath, info),
            info?.CancellationToken ?? new());
    }
    #endregion
    #region 传入文件
    public async Task FormatConversion(IFile file, string targetPath, FormatConversionInfo? info = null)
    {
        var newInfo = info ?? new();
        var mediaInfo = await FFmpeg.GetMediaInfo(file.Path);
        var videoStream = mediaInfo.VideoStreams.FirstOrDefault();
        var scale = "";
        if ((newInfo, videoStream) is ({ MaxDefinition: { } definition }, { }))
        {
            var size = CreateMath.SizePixel(videoStream.Width, videoStream.Height);
            var (w, _) = size.DimensionalityReduction(definition);
            var proportion = decimal.Divide(size.PixelCount.Horizontal, w);
            scale = $"-vf scale=iw/{proportion}:ih/{proportion}";
        }
        var arguments = $"-i {file.Path} -c:v libx264 -c:a aac {scale} {targetPath}";
        var father = Path.GetDirectoryName(targetPath);
        if (!father.IsVoid())
            Directory.CreateDirectory(father);
        await FFmpeg.Conversions.New().Start(arguments, newInfo.CancellationToken);
    }
    #endregion
    #endregion
    #region 视频截图
    #region 传入流
    public async Task Screenshot(IBitRead read, string currentFormat, IEnumerable<(TimeSpan Fragment, string Path)> infos, Func<decimal, Task>? reportProgress = null, CancellationToken cancellationToken = default)
    {
        await SaveAndProcess(read, currentFormat,
            x => Screenshot(x, infos, reportProgress, cancellationToken),
            cancellationToken);
    }
    #endregion
    #region 传入文件
    public async Task Screenshot(IFile file, IEnumerable<(TimeSpan Fragment, string Path)> infos, Func<decimal, Task>? reportProgress = null, CancellationToken cancellationToken = default)
    {
        var path = file.Path;
        var snippet = FFmpeg.Conversions.FromSnippet;
        reportProgress ??= _ => Task.CompletedTask;
        foreach (var ((fragment, output), index, count) in infos.PackIndex(true))
        {
            var conversion = await snippet.Snapshot(path, output, fragment);
            await conversion.Start(cancellationToken);
            await reportProgress(decimal.Divide(index + 1, count));
        }
    }
    #endregion
    #endregion
    #region 内部成员
    #region 保存并处理管道
    /// <summary>
    /// 将一个管道保存到临时文件，
    /// 并处理这个临时文件，然后将其删除
    /// </summary>
    /// <param name="read">用来读取文件的管道</param>
    /// <param name="currentFormat">新建临时文件的格式</param>
    /// <param name="process">用来处理临时文件的委托，它的参数就是该临时文件本身</param>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    private static async Task SaveAndProcess(IBitRead read, string currentFormat, Func<IFile, Task> process, CancellationToken cancellationToken)
    {
        var path = ToolTemporaryFile.CreateTemporaryPath(currentFormat);
        await read.SaveToFile(path, cancellation: cancellationToken);
        await process(CreateIO.File(path));
        File.Delete(path);
    }
    #endregion
    #endregion
}
