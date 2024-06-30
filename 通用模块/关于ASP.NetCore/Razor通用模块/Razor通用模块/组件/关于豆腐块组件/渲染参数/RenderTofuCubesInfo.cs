namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是豆腐块组件的渲染参数
/// </summary>
public sealed record RenderTofuCubesInfo
{
    #region 获取容器的CSS类名
    /// <summary>
    /// 获取容器的CSS类名
    /// </summary>
    public required string CSSContainer { get; init; }
    #endregion
    #region 图标的CSS类名
    /// <summary>
    /// 获取图标的CSS类名
    /// </summary>
    public required string CSSIcon { get; init; }
    #endregion
    #region 文字的CSS类名
    /// <summary>
    /// 获取文字的CSS类名
    /// </summary>
    public required string CSSText { get; init; }
    #endregion
    #region 文字的样式
    /// <summary>
    /// 获取文字的样式（不是CSS类名）
    /// </summary>
    public required string TextStyle { get; init; }
    #endregion
    #region 容器的宽度
    /// <summary>
    /// 获取容器的宽度样式（不是CSS类名）
    /// </summary>
    public required string ContainerWidth { get; init; }
    #endregion
    #region 要渲染的文字
    /// <summary>
    /// 获取要渲染的文字
    /// </summary>
    public required string Text { get; init; }
    #endregion
}
