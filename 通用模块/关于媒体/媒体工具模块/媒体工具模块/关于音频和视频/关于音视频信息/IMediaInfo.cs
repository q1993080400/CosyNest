namespace System.Media.Play;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个媒体文件的信息
/// </summary>
public interface IMediaInfo
{
    #region 返回这个媒体的流
    /// <summary>
    /// 返回这个媒体文件的流，
    /// 它包括视频流，音频流，字幕流等
    /// </summary>
    IEnumerable<IMediaStream> Stream { get; }
    #endregion
}
