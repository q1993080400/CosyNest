namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 表示一个豆腐块组件，
/// 它由图标和说明组成，形态类似各大OS文件夹UI
/// </summary>
public sealed partial class TofuCubes : ComponentBase, IContentComponent<RenderFragment<RenderTofuCubesInfo>>
{
    #region 组件参数
    #region 文字
    /// <summary>
    /// 获取或设置豆腐块的文字
    /// </summary>
    [EditorRequired]
    [Parameter]
    public string Text { get; set; }
    #endregion
    #region 可显示的最大字数
    /// <summary>
    /// 获取或设置豆腐块可显示的最大字数，
    /// 注意：豆腐块只支持显示一行文字
    /// </summary>
    [Parameter]
    public int MaxTextLength { get; set; } = 6;
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderTofuCubesInfo> ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取渲染参数
    private RenderTofuCubesInfo GetRenderInfo()
    {
        var width = $"width: calc({MaxTextLength}ic + 2ch)";
        return new()
        {
            Text = Text,
            TextStyle = width,
            ContainerWidth = width,
            CSSContainer = "tofuCubesContainer",
            CSSIcon = "tofuCubesIcon",
            CSSText = "tofuCubesText"
        };
    }
    #endregion
    #endregion
}
