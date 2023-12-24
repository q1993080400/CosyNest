namespace System.Media.Play;

/// <summary>
/// 这个类型是底层使用FFmpeg实现的<see cref="IVideoStream"/>
/// </summary>
/// <param name="videoStream">封装的视频流</param>
sealed class VideoStreamFFmpeg(Xabe.FFmpeg.IVideoStream videoStream)
    : MediaStreamFFmpeg(videoStream), IVideoStream
{
    #region 内部成员
    #region 默认扩展名
    protected override string DefaultExtensionName => "mp4";
    #endregion
    #endregion
}
