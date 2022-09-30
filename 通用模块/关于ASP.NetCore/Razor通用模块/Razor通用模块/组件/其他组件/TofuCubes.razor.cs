namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 表示一个豆腐块组件，
/// 它由图标和说明组成，形态类似各大OS文件夹UI
/// </summary>
public sealed partial class TofuCubes : ComponentBase
{
    #region 组件参数
    #region 图标
    /// <summary>
    /// 获取或设置豆腐块的图标，
    /// 它的参数是豆腐块的文字
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<string> Icon { get; set; }
    #endregion
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
    #region 参数展开
    /// <summary>
    /// 获取参数展开，它适用于组件的容器部分
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }
    #endregion
    #endregion 
}
