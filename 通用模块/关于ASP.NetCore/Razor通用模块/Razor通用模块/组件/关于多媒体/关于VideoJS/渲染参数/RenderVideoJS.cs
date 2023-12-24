namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是<see cref="VideoJS"/>的渲染参数
/// </summary>
public sealed record RenderVideoJS
{
    #region 是否自动播放
    /// <summary>
    /// 获取是否自动播放
    /// </summary>
    public bool Autoplay { get; init; }
    #endregion
    #region 视频的源
    /// <summary>
    /// 获取视频的源，
    /// 它的顺序很重要，
    /// 函数会以此作为优先级依次加载媒体
    /// </summary>
    public required IEnumerable<MediumSource> Source { get; init; }
    #endregion
    #region 是否为音频专用模式
    /// <summary>
    /// 如果这个值为<see langword="true"/>，表示它是专门用来播放音频的，
    /// 为<see langword="false"/>，表示它是一个视频播放器，
    /// 为<see langword="null"/>，表示自动推断
    /// </summary>
    public bool? AudioOnlyMode { get; init; }
    #endregion
    #region 是否循环播放
    /// <summary>
    /// 获取视频是否循环播放
    /// </summary>
    public bool Loop { get; init; }
    #endregion
    #region 是否显示控件
    /// <summary>
    /// 获取是否显示控件
    /// </summary>
    public bool Controls { get; init; }
    #endregion
    #region 宽度
    /// <summary>
    /// 获取播放器的宽度
    /// </summary>
    public int Width { get; init; } = 300;
    #endregion
    #region 高度
    /// <summary>
    /// 获取播放器的高度
    /// </summary>
    public int Height { get; init; } = 500;
    #endregion
}
