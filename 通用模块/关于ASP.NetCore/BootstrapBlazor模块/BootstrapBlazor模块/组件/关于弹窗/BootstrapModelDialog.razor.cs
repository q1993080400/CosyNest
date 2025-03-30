using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是底层使用Bootstrap实现的弹窗，
/// 它不使用Web原生的dialog标签，
/// 在特殊情况下，可以避免某些弹出组件被遮盖的问题
/// </summary>
public sealed partial class BootstrapModelDialog : ComponentBase
{
    #region 组件参数
    #region 标题
    /// <summary>
    /// 获取弹窗的标题
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Title { get; set; }
    #endregion
    #region 弹窗大小
    /// <summary>
    /// 这个参数允许显式控制弹窗的大小
    /// </summary>
    [Parameter]
    public Size Size { get; set; } = Size.ExtraExtraLarge;
    #endregion
    #region 是否滚动
    /// <summary>
    /// 获取在长度超出限制的时候，弹窗主体是否滚动，
    /// 如果弹窗会因为增加元素而扩张，
    /// 请务必将这个属性设置为<see langword="true"/>
    /// </summary>
    [Parameter]
    public bool IsScrolling { get; set; } = true;
    #endregion
    #region 渲染弹窗主体的方法
    /// <summary>
    /// 获取渲染弹窗主体的方法
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment RenderBody { get; set; }
    #endregion
    #region 渲染弹窗页脚的方法
    /// <summary>
    /// 获取渲染弹窗页脚的方法
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment RenderFooter { get; set; }
    #endregion
    #endregion
}
