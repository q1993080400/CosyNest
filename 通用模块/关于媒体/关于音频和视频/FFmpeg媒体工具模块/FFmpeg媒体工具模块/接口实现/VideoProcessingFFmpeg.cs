using System.IOFrancis;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.MathFrancis;

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
        #region 返回缩放比例的本地函数
        string GetScale()
        {
            if ((info, videoStream) is ({ MaxDefinition: { } definition }, { }))
            {
                var size = CreateMath.SizePixel(videoStream.Width, videoStream.Height);
                var (ow, oh) = size;
                var (cw, _) = size.DimensionalityReduction(definition);
                var proportion = decimal.Divide(ow, cw);
                var (fw, fh) = ((int)(ow / proportion), (int)(oh / proportion));
                var (rw, rh) = (fw % 2, fh % 2);
                if (rw is not 0 || rh is not 0)
                    return $"-vf scale={fw - rw}:{fh - rh}";    //FFmpeg不知道为什么，不允许视频的长宽为奇数，这段代码就是为了解决这个问题
                return $"-vf scale={fw}:-1";
            }
            return "";
        }
        #endregion
        var emphasizeCompatibility = info.EmphasizeCompatibility;
        var arguments = $"-i {mediumPath} -c:v {(emphasizeCompatibility ? "libx264" : "libvpx-vp9")} -c:a {(emphasizeCompatibility ? "aac" : "libopus")} {GetScale()} {targetPath}";
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
