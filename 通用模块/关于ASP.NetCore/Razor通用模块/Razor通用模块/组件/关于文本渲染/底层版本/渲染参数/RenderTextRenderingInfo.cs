namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="TextRendering"/>组件的参数
/// </summary>
public sealed record RenderTextRenderingInfo
{
    #region 要渲染的文本
    /// <summary>
    /// 这个集合的元素的第一个项指示该文本的类型，
    /// 第二个项指示要渲染的文本
    /// </summary>
    public required IEnumerable<(RenderTextType RenderTextType, string Text)> RenderText { get; init; }
    #endregion
}
