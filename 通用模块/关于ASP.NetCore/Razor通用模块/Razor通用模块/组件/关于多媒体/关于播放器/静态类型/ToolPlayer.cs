namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个静态类为实现播放器提供帮助
/// </summary>
public static class ToolPlayer
{
    #region 有关音量
    #region 初始化播放器音量
    /// <summary>
    /// 初始化播放器音量
    /// </summary>
    /// <param name="runtime">JS运行时</param>
    /// <returns></returns>
    public static async Task<double> InitializationPlayVolume(IJSRuntime runtime)
        => await runtime.InvokeAsync<double>("InitializationPlayVolume");
    #endregion
    #region 合理化播放器音量
    /// <summary>
    /// 将播放器音量控制在一个合理的范围内
    /// </summary>
    /// <param name="volume">旧的播放器音量</param>
    /// <returns></returns>
    public static double RationalizationVolume(double volume)
    {
        var newVolume = volume > 1 ? volume / 100 : volume;
        return Math.Round(Math.Clamp(newVolume, 0, 1), 2);
    }
    #endregion
    #region 记录播放器音量
    /// <summary>
    /// 记录播放器音量
    /// </summary>
    /// <param name="runtime">JS运行时对象</param>
    /// <param name="volume">要记录的播放器音量</param>
    /// <returns></returns>
    public static async Task RecordPlayVolume(IJSRuntime runtime, double volume)
        => await runtime.InvokeVoidAsync("RecordPlayVolume", RationalizationVolume(volume));
    #endregion
    #endregion
}
