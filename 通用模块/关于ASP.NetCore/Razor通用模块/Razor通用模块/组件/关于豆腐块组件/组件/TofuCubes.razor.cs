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
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderTofuCubesInfo> ChildContent { get; set; }
    #endregion
    #region 静态成员：计算容器宽度
    /// <summary>
    /// 返回一个style文本，它计算容器的宽度，
    /// 这个style必须被应用在组件的父容器上
    /// </summary>
    /// <param name="maxTextLength">豆腐块组件所能显示的最大字数，
    /// 注意：豆腐块只能显示一行文字</param>
    /// <returns></returns>
    public static string ContainerWidthStyle(int maxTextLength = 6)
        => $"--sharingContainerElementWidth:calc({maxTextLength}ic + 2ch)";
    #endregion
    #endregion
    #region 内部成员
    #region 获取渲染参数
    private RenderTofuCubesInfo GetRenderInfo()
        => new()
        {
            Text = Text,
            CSSContainer = "tofuCubesContainer",
            CSSIcon = "tofuCubesIcon",
            CSSText = "tofuCubesText"
        };
    #endregion
    #endregion
}
