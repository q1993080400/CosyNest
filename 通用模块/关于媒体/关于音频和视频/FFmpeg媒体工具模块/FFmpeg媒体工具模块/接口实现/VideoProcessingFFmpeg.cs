using System.IOFrancis;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;

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
        return await MixedFlow(videoPath, audioPath);
    }
    #endregion
    #region 传入文件
    public async Task<IBitRead> MixedFlow(PathText videoPath, PathText audioPath)
    {
        var output = ToolTemporaryFile.CreateTemporaryPath("mp4");
        var conversion = FFmpeg.Conversions.New().AddParameter($"-i {videoPath} -i {audioPath} -c:v copy -c:a aac -strict experimental {output}");
        await conversion.Start();
        return CreateIO.FileStream(output).ToBitPipe().Read;
    }
    #endregion
    #endregion
}
