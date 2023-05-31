namespace Microsoft.JSInterop;

/// <summary>
/// 这个枚举描述页面的可见性状态
/// </summary>
public enum VisibilityState
{
    /// <summary>
    /// 表示页面可见
    /// </summary>
    Visible,
    /// <summary>
    /// 表示页面因为最小化，浏览器处于后台，锁屏等原因已经不可见
    /// </summary>
    Hidden,
    /// <summary>
    /// 表示页面正在渲染中，所以不可见
    /// </summary>
    Prerender
}
