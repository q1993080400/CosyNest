namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个模态对话框
/// </summary>
public sealed partial class ModalDialog : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 对话框是否打开
    /// <summary>
    /// 获取对话框是否打开
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; } = true;
    #endregion
    #region 组件的子内容
    [Parameter]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 这个字典用来接收参数展开，
    /// 它控制了模态对话框遮罩层的样式
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }
    #endregion
    #endregion
}
