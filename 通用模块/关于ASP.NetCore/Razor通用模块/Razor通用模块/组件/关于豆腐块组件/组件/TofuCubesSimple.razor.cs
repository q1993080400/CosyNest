namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是豆腐块组件的开箱即用版本，
/// 它由图标和说明组成，形态类似各大OS文件夹UI
/// </summary>
public sealed partial class TofuCubesSimple : ComponentBase
{
    #region 组件参数
    #region 文字
    /// <inheritdoc cref="TofuCubes.Text"/>
    [EditorRequired]
    [Parameter]
    public string Text { get; set; }
    #endregion
    #region 每行最大字数
    /// <inheritdoc cref="TofuCubes.MaxRowLength"/>
    [Parameter]
    public int MaxRowLength { get; set; } = 6;
    #endregion
    #region 宽度
    /// <inheritdoc cref="TofuCubes.Width"/>
    [Parameter]
    public string? Width { get; set; }
    #endregion
    #region 渲染图标部分的委托
    /// <summary>
    /// 获取用来渲染图标部分的委托，
    /// 它的参数就是要渲染的文字
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<string> RenderIcon { get; set; }
    #endregion
    #region 高亮文本
    /// <summary>
    /// 获取高亮文本的集合
    /// </summary>
    [Parameter]
    public IReadOnlyCollection<string>? Highlight { get; set; }
    #endregion
    #region 高亮组
    /// <summary>
    /// 获取高亮组，
    /// 通过指定它，可以为高亮指定不同的样式
    /// </summary>
    [Parameter]
    public string HighlightGroup { get; set; } = "highlightGroup";
    #endregion
    #region 参数展开
    /// <summary>
    /// 获取容器的参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
}
