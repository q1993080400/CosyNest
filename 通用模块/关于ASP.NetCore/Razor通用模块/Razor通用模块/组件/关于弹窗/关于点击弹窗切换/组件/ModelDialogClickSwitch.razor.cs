namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个特殊的切换弹窗组件，
/// 它在点击指定元素的时候弹出弹窗，
/// 然后在点击其他任何地方的时候关闭弹窗
/// </summary>
public sealed partial class ModelDialogClickSwitch : ComponentBase
{
    #region 组件参数
    #region 用来渲染组件的委托
    /// <summary>
    /// 获取用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderModelDialogClickSwitchInfo> RenderComponent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 弹窗的状态
    /// <summary>
    /// 获取是否开启弹窗
    /// </summary>
    private bool IsOpen { get; set; }
    #endregion
    #region 获取渲染状态
    /// <summary>
    /// 获取本组件的渲染状态
    /// </summary>
    /// <returns></returns>
    private RenderModelDialogClickSwitchInfo GetRenderInfo()
        => new()
        {
            IsOpen = IsOpen,
            Close = () =>
            {
                IsOpen = false;
                this.StateHasChanged();
            },
            Switch = () =>
            {
                IsOpen = !IsOpen;
                this.StateHasChanged();
            }
        };
    #endregion
    #endregion
}
