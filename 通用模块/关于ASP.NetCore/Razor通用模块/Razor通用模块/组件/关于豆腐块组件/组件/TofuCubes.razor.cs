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
    #region 每行最大字数
    /// <summary>
    /// 获取或设置豆腐块的每行最大字数
    /// </summary>
    [Parameter]
    public int MaxRowLength { get; set; } = 8;
    #endregion
    #region 最大行数
    /// <summary>
    /// 获取能允许显示的最大行数，
    /// 结合<see cref="MaxRowLength"/>，
    /// 无法显示的字符会用省略号代替
    /// </summary>
    [Parameter]
    public int MaxRow { get; set; } = 2;
    #endregion
    #region 宽度
    /// <summary>
    /// 显式指定组件的宽度，
    /// 它可以保证每个豆腐块的宽度一致，
    /// 如果为<see langword="null"/>，则自动计算
    /// </summary>
    [Parameter]
    public string? Width { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderTofuCubesInfo>? ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取渲染参数
    private RenderTofuCubesInfo GetRenderInfo()
    {
        var maxLength = MaxRowLength * MaxRow;
        var text = Text.Length <= maxLength ?
            Text :
            Text[..maxLength] + "..";
        return new()
        {
            Text = text,
            WidthStyle = $"width:{Width ?? $"{MaxRowLength + 1}em"}",
            CSSContainer = "tofuCubesContainer",
            CSSIcon = "tofuCubesIcon",
            CSSText = "tofuCubesText"
        };
    }
    #endregion
    #endregion
}
