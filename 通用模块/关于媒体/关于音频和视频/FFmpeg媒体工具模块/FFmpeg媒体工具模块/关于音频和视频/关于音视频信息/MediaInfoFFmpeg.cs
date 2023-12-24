namespace System.Media.Play;

/// <summary>
/// 这个类型是底层使用FFmpeg实现的<see cref="IMediaInfo"/>
/// </summary>
/// <param name="mediaInfo">封装的媒体信息对象</param>
sealed class MediaInfoFFmpeg(Xabe.FFmpeg.IMediaInfo mediaInfo) : IMediaInfo
{
    #region 返回媒体流
    public IEnumerable<IMediaStream> Stream
    {
        get
        {
            foreach (var stream in mediaInfo.Streams)
            {
                switch (stream)
                {
                    case Xabe.FFmpeg.IVideoStream videoStream:
                        yield return new VideoStreamFFmpeg(videoStream);
                        break;
                    case Xabe.FFmpeg.IAudioStream audioStream:
                        yield return new AudioStreamFFmpeg(audioStream);
                        break;
                }
            }
        }
    }
    #endregion 
}
