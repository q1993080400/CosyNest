
namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个模式对话框，
/// 它只要渲染，就默认打开，
/// 如果不需要打开，就应该不渲染它
/// </summary>
public sealed partial class ModelDialog : ComponentBase, IContentComponent<RenderFragment<string>>
{
    #region 组件参数
    #region 子内容
    /// <summary>
    /// 组件的子内容，
    /// 它的参数就是模式对话框的ID，
    /// 必须将这个ID赋值给一个dialog标签，
    /// 否则无法正常工作
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<string> ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 模式对话框的ID
    /// <summary>
    /// 获取模式对话框的ID
    /// </summary>
    private string ID { get; } = CreateASP.JSObjectName();
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await JSWindow.InvokeVoidAsync("OpenModelDialog", ID);
    }
    #endregion
    #endregion
}
