namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是媒体预览时，用来渲染封面的参数
/// </summary>
public sealed record RenderCoverInfo
{
    #region 要渲染的媒体
    /// <summary>
    /// 获取要渲染的媒体
    /// </summary>
    public required MediaSource MediaSource { get; init; }
    #endregion
    #region 预览媒体所发生的事件
    /// <summary>
    /// 当执行预览媒体的时候，触发这个事件
    /// </summary>
    public required EventCallback PreviewEvent { get; init; }
    #endregion
}
