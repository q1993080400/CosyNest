namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是<see cref="RenderPlayerState"/>中，
/// 允许用户控制的部分
/// </summary>
public sealed record RenderPlayerStateOperational
{
    #region 媒体源
    /// <summary>
    /// 获取要播放的所有媒体的Uri，
    /// 按照优先级从高到低排序
    /// </summary>
    public required IReadOnlyList<string> MediumSource { get; init; }
    #endregion
    #region 是否循环播放
    /// <summary>
    /// 是否循环播放
    /// </summary>
    public bool Loop { get; init; } = true;
    #endregion
    #region 是否静音
    /// <summary>
    /// 获取是否静音
    /// </summary>
    public bool Muted { get; init; }
    #endregion
    #region 是否自动播放
    /// <summary>
    /// 获取播放器是否自动播放
    /// </summary>
    public bool AutoPlay { get; init; } = true;
    #endregion
    #region 已播放长度
    /// <summary>
    /// 获取已播放长度，以秒为单位，
    /// 它可以用于跳转到媒体的某个位置
    /// </summary>
    public double Played { get; init; }
    #endregion
    #region 音量
    private double VolumeField { get; set; } = 0.6;

    /// <summary>
    /// 获取媒体的音量，
    /// 它的合理取值范围是从0到1
    /// </summary>
    public double Volume
    {
        get => VolumeField;
        init => VolumeField = ToolPlayer.RationalizationVolume(value);
    }
    #endregion
}
