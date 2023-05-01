namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="TextRendering"/>组件的参数
/// </summary>
public sealed record RenderTextRenderingInfo
{
    #region 要渲染的文本
    /// <summary>
    /// 这个集合的元素的第一个项指示该文本是否为Uri，
    /// 第二个项指示要渲染的文本
    /// </summary>
    public required IEnumerable<(bool IsUri, string Text)> RenderText { get; init; }
    #endregion
    #region 容器CSS样式
    /// <summary>
    /// 指示容器的CSS样式，
    /// 在这里默认容器是一个pre标签
    /// </summary>
    public required string ContainerCSS { get; init; }
    #endregion
}
