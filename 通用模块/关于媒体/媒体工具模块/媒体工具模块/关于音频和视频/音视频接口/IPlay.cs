namespace System.Media.Play;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个播放视频或音频的任务
/// </summary>
public interface IPlay
{
    #region 是否正在播放
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则继续播放，否则停止播放
    /// </summary>
    bool IsPlay { get; set; }
    #endregion
    #region 播放长度
    /// <summary>
    /// 指示音频或视频的长度
    /// </summary>
    TimeSpan Length { get; }
    #endregion
    #region 进度
    /// <summary>
    /// 获取或设置音频或视频的进度，
    /// 它指的是已经播放完的部分
    /// </summary>
    TimeSpan Schedule { get; set; }
    #endregion
}
