
using Xabe.FFmpeg.Downloader;

namespace System.Media.Play;

/// <summary>
/// 该类型为创建FFmpeg实现的视频处理接口提供帮助
/// </summary>
public static class CreateFFmpeg
{
    #region 初始化包
    /// <summary>
    /// 调用这个方法以初始化包，
    /// 在使用本模块之前，必须调用本方法，
    /// 或将FFmpeg放在程序根目录
    /// </summary>
    /// <returns></returns>
    public static async Task Initialization()
    {
        if (!File.Exists("ffmpeg.exe") || !File.Exists("ffprobe.exe"))
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);
    }
    #endregion 
    #region 创建IVideoProcessing
    /// <summary>
    /// 创建一个底层使用FFmpeg实现的<see cref="IVideoProcessing"/>，
    /// 它可以用来处理音视频
    /// </summary>
    public static IVideoProcessing VideoProcessing { get; } = new VideoProcessingFFmpeg();
    #endregion
}
