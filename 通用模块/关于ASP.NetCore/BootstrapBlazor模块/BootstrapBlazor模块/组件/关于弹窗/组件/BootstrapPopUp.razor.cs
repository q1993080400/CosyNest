using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是底层使用Bootstrap实现的弹窗
/// </summary>
public sealed partial class BootstrapPopUp : ComponentBase
{
    #region 组件参数
    #region 是否开启
    /// <summary>
    /// 获取是否开启
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsOpen { get; set; }
    #endregion
    #region 渲染整个组件
    /// <summary>
    /// 获取用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    public RenderFragment<RenderBootstrapPopUpInfo>? RenderComponent { get; set; }
    #endregion
    #region 标题
    /// <summary>
    /// 获取标题
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Title { get; set; }
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
    /// 用来渲染页脚的委托，
    /// 它的参数就是用来取消弹窗的委托
    /// </summary>
    [Parameter]
    public RenderFragment<EventCallback>? RenderFooter { get; set; }
    #endregion
    #region 用来取消弹窗的委托
    /// <summary>
    /// 获取用来取消弹窗的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public EventCallback Cancellation { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderBootstrapPopUpInfo GetRenderInfo()
    {
        #region 用来渲染页脚的委托
        RenderFragment RenderFooter(EventCallback cancellation)
            => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "compactCentered");
                builder.OpenComponent<Button>(2);
                builder.AddComponentParameter(3, nameof(Button.Text), "取消");
                var onClick = CreateRazor.EventCallbackNotRefresh<MouseEventArgs>(() => cancellation.InvokeAsync());
                builder.AddComponentParameter(4, nameof(Button.OnClick), onClick);
                builder.CloseComponent();
                builder.CloseElement();
            };
        #endregion
        return new()
        {
            RenderBody = RenderBody,
            Cancellation = Cancellation,
            Title = Title,
            RenderFooter = this.RenderFooter ?? RenderFooter
        };
    }
    #endregion
    #endregion
}
