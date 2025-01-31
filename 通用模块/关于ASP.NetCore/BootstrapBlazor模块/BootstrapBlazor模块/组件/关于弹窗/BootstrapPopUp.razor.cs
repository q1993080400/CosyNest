using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是底层使用Bootstrap实现的弹窗，
/// 它只要渲染，就默认打开，
/// 如果不需要打开，就应该不渲染它
/// </summary>
public sealed partial class BootstrapPopUp : ComponentBase
{
    #region 组件参数
    #region 标题
    /// <summary>
    /// 弹窗的标题
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "";
    #endregion
    #region 用来取消弹窗的委托
    /// <summary>
    /// 用来取消弹窗的委托
    /// </summary>
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
    #region 弹窗CSS样式
    /// <summary>
    /// 获取弹窗dialog组件的CSS类名
    /// </summary>
    [Parameter]
    public string? DialogCSS { get; set; }
    #endregion
    #endregion
}
