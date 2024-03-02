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
    public async Task MixedFlow(string videoPath, string audioPath, string outputPath)
    {
        ToolIO.CreateFather(outputPath);
        var conversion = FFmpeg.Conversions.New().AddParameter($"-i {videoPath} -i {audioPath} -c:v copy -c:a aac -strict experimental {outputPath}");
        await conversion.Start();
    }
    #endregion
    #region 格式转换
    public async Task FormatConversion(FormatConversionInfo info)
    {
        var mediaPath = info.MediaPath;
        var targetPath = info.TargetPath;
        var mediaInfo = await FFmpeg.GetMediaInfo(mediaPath);
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
        var arguments = $"-i {mediaPath} -c:v {(emphasizeCompatibility ? "libx264" : "libvpx-vp9")} -c:a {(emphasizeCompatibility ? "aac" : "libopus")} {GetScale()} {targetPath}";
        ToolIO.CreateFather(targetPath);
        await FFmpeg.Conversions.New().Start(arguments, info.CancellationToken);
    }
    #endregion
    #region 视频截图
    public async Task Screenshot(ScreenshotInfo info)
    {
        var allFragment = info.Fragment.OrderBy(x => x.Fragment).Index().ToArray();
        var count = allFragment.Length;
        if (count is 0)
            return;
        var snippet = FFmpeg.Conversions.FromSnippet;
        var reportProgress = info.ReportProgress ?? new Func<decimal, Task>(_ => Task.CompletedTask);
        var mediaPath = info.MediaPath;
        var cancellationToken = info.CancellationToken;
        foreach (var ((fragment, output), index) in allFragment)
        {
            ToolIO.CreateFather(output);
            var conversion = await snippet.Snapshot(mediaPath, output, fragment);
            await conversion.Start(cancellationToken);
            await reportProgress(decimal.Divide(index + 1, count));
        }
    }
    #endregion
    #region 获取音视频信息
    public async Task<IMediaInfo> GetMediaInfo(string mediaPath)
    {
        var mediaInfo = await FFmpeg.GetMediaInfo(mediaPath);
        return new MediaInfoFFmpeg(mediaInfo);
    }
    #endregion
}
