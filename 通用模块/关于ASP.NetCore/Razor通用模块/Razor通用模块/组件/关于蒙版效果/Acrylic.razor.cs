namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件能够让放置在其中的子内容自动获得亚克力效果背景
/// </summary>
public sealed partial class Acrylic : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 子内容
    /// <summary>
    /// 组件的子内容，如果它为<see langword="null"/>,
    /// 表示亚克力效果覆盖整个屏幕
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    #endregion
    #region 背景图片的Uri
    /// <summary>
    /// 获取或设置背景图片的Uri
    /// </summary>
    [Parameter]
    public string? BackgroundUri { get; set; }
    #endregion
    #region 遮罩层CSS
    /// <summary>
    /// 获取或设置遮罩层的CSS样式，
    /// 它是在背景图片上的蒙版
    /// </summary>
    [Parameter]
    public string MaskCSS { get; set; } = "mask";
    #endregion
    #region 参数展开
    /// <summary>
    /// 获取参数展开，它适用于组件的容器部分
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }
    #endregion
    #endregion
}
