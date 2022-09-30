namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件是一个九宫格布局，
/// 将内容放置在九宫格的任意一个位置
/// </summary>
public sealed partial class NineGrids : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 有关子内容位置
    #region 行位置
    /// <summary>
    /// 指示所在行的起始和结束，
    /// 它的语法和css中的grid-row相同
    /// </summary>
    [Parameter]
    public string Row { get; set; } = "2";
    #endregion
    #region 列位置
    /// <summary>
    /// 指示所在列的起始和结束，
    /// 它的语法和css中的grid-column相同
    /// </summary>
    [Parameter]
    public string Column { get; set; } = "2";
    #endregion
    #endregion
    #region 参数展开
    /// <summary>
    /// 该字典指示封装九宫格的容器的特性
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #region 是否允许横屏
    /// <summary>
    /// 如果<see cref="IsAbsolute"/>和本属性均为<see langword="true"/>，
    /// 则在手机端还会强制横屏
    /// </summary>
    [Parameter]
    public bool CanHorizontalScreen { get; set; }
    #endregion
    #region 相对于屏幕或父元素
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示九宫格相对于屏幕，否则表示相对于父元素
    /// </summary>
    [Parameter]
    public bool IsAbsolute { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    #endregion
    #region 子内容容器CSS
    /// <summary>
    /// 获取或设置用来容纳子内容的容器的CSS类名
    /// </summary>
    [Parameter]
    public string? ChildContentCSS { get; set; }
    #endregion
    #endregion
}
