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
    #region 视频的地址
    /// <summary>
    /// 获取视频的地址
    /// </summary>
    public required string Src { get; init; }
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
    #region 视频的ID
    /// <summary>
    /// 获取视频的ID
    /// </summary>
    public string ID { get; init; } = ToolASP.CreateJSObjectName();
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
