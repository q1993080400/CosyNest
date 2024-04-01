
namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个模式对话框
/// </summary>
public sealed partial class ModelDialog : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 是否打开
    /// <summary>
    /// 获取这个模式对话框是否打开
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsOpen { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 模式对话框的ID
    /// <summary>
    /// 获取模式对话框的ID
    /// </summary>
    private string ID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsOpen)
            await JSWindow.InvokeVoidAsync("OpenModelDialog", ID);
    }
    #endregion
    #endregion
}
