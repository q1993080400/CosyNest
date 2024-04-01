using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个记录是渲染<see cref="BootstrapPopUp"/>的参数
/// </summary>
public sealed record RenderBootstrapPopUpInfo
{
    #region 标题
    /// <summary>
    /// 获取标题
    /// </summary>
    public required string Title { get; init; }
    #endregion
    #region 用来渲染正文部分的委托
    /// <summary>
    /// 获取用来渲染正文部分的委托
    /// </summary>
    public required RenderFragment RenderBody { get; init; }
    #endregion
    #region 用来取消弹窗的委托
    /// <summary>
    /// 获取用来取消弹窗的委托
    /// </summary>
    public required EventCallback Cancellation { get; init; }
    #endregion
    #region 用来渲染页脚的委托
    /// <summary>
    /// 用来渲染页脚的委托，
    /// 它的参数就是用来取消弹窗的委托
    /// </summary>
    public required RenderFragment<EventCallback> RenderFooter { get; init; }
    #endregion
}
