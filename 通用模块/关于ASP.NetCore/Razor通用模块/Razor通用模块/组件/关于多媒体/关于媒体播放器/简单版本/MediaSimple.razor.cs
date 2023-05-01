namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是<see cref="Media"/>组件的开箱即用版本
/// </summary>
public sealed partial class MediaSimple : ComponentBase
{
    #region 组件参数
    #region 是否关闭自动记忆播放进度
    /// <inheritdoc cref="Media.CloseMemoryProgress"/>
    [Parameter]
    public bool CloseMemoryProgress { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 这个参数展开会被用在媒体标签上
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? MediaAttributes { get; set; }
    #endregion
    #endregion
}
