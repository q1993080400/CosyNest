namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可用于弹窗，
/// 它只要渲染，就默认打开，
/// 如果不需要打开，就应该不渲染它
/// </summary>
public sealed partial class PopUp : ComponentBase
{
    #region 组件参数
    #region 渲染整个组件
    /// <summary>
    /// 获取用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderPopUpInfo> RenderComponent { get; set; }
    #endregion
    #region 标题
    /// <summary>
    /// 获取标题
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Title { get; set; }
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
    private RenderPopUpInfo GetRenderInfo()
        => new()
        {
            Cancellation = Cancellation,
            Title = Title,
        };
    #endregion
    #endregion
}
