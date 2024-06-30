using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是底层使用Bootstrap实现的弹窗
/// </summary>
public sealed partial class BootstrapPopUp : ComponentBase
{
    #region 组件参数
    #region 是否开启
    /// <inheritdoc cref="PopUp.IsOpen"/>
    [Parameter]
    [EditorRequired]
    public bool IsOpen { get; set; }
    #endregion
    #region 标题
    /// <inheritdoc cref="PopUp.Title"/>
    [Parameter]
    [EditorRequired]
    public string Title { get; set; }
    #endregion
    #region 用来取消弹窗的委托
    /// <inheritdoc cref="PopUp.Cancellation"/>
    [Parameter]
    [EditorRequired]
    public EventCallback Cancellation { get; set; }
    #endregion
    #region 用来渲染正文部分的委托
    /// <summary>
    /// 获取用来渲染正文部分的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment RenderBody { get; set; }
    #endregion
    #region 用来渲染页脚的委托
    /// <summary>
    /// 获取用来渲染页脚的委托，
    /// 它的参数就是用来关闭弹窗的委托
    /// </summary>
    [Parameter]
    public RenderFragment<EventCallback>? RenderFooter { get; set; }
    #endregion
    #endregion
}
