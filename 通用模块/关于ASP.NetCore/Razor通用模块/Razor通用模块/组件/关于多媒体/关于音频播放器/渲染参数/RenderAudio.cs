namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="Audio"/>组件的参数
/// </summary>
public sealed record RenderAudio
{
    #region 媒体源
    /// <summary>
    /// 获取音频的源
    /// </summary>
    public required IEnumerable<MediumSource> MediaSource { get; init; }
    #endregion
    #region 是否循环
    /// <summary>
    /// 获取是否循环
    /// </summary>
    public bool Loop { get; init; } = true;
    #endregion
    #region 是否静音
    /// <summary>
    /// 获取是否静音
    /// </summary>
    public bool Muted { get; init; }
    #endregion
    #region 是否播放
    /// <summary>
    /// 获取这个音频是否正在播放
    /// </summary>
    public bool Playing { get; init; } = true;
    #endregion
}
