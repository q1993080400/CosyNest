namespace System.Media.Play;

/// <summary>
/// 这个记录是<see cref="IVideoProcessing.Screenshot(ScreenshotInfo)"/>方法的参数
/// </summary>
public sealed record ScreenshotInfo
{
    #region 要截图的视频路径
    /// <summary>
    /// 获取要截图的视频路径
    /// </summary>
    public required string MediaPath { get; init; }
    #endregion
    #region 截图的时间和路径
    /// <summary>
    /// 这个集合元素的第一个项是要截图的时间，第二个项是截图文件保存路径
    /// </summary>
    public required IEnumerable<(TimeSpan Fragment, string Path)> Fragment { get; init; }
    #endregion
    #region 用于报告进度的委托
    /// <summary>
    /// 用来报告进度的委托，它的参数就是当前进度
    /// </summary>
    public Func<decimal, Task>? ReportProgress { get; init; }
    #endregion
    #region 用来取消异步操作的令牌
    /// <summary>
    /// 用来取消异步操作的令牌
    /// </summary>
    public CancellationToken CancellationToken { get; init; }
    #endregion
}
