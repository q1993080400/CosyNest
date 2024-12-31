using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是底层使用Bootstrap实现的弹窗，
/// 它不使用Web原生的dialog标签，
/// 在特殊情况下，可以避免某些弹出组件被遮盖的问题
/// </summary>
public sealed partial class BootstrapModelDialog : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 是否打开
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示弹窗打开，否则表示关闭，
    /// 请始终保留本组件，并通过这个参数来控制是否打开，
    /// 而不是关闭弹窗时就销毁本组件，
    /// 这是因为Bootstrap弹窗的实现缺陷造成的
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsOpen { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 缓存的是否打开参数
    private bool IsOpenCache { get; set; }
    #endregion
    #region 捕获的弹窗组件
    /// <summary>
    /// 获取捕获的弹窗组件
    /// </summary>
    private Modal? Modal { get; set; }
    #endregion
    #region 重写OnParametersSetAsync方法
    protected async override Task OnParametersSetAsync()
    {
        if (Modal is null || IsOpen == IsOpenCache)
            return;
        if (IsOpen)
            await Modal.Show();
        else
            await Modal.Close();
        IsOpenCache = IsOpen;
    }
    #endregion
    #endregion
}
