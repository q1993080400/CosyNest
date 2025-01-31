namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 当用户看见这个组件内部的媒体时，自动播放它，
/// 当用户看不见时，自动停止播放，
/// 这个效果对组件内部的任何媒体都有效
/// </summary>
public sealed partial class VisiblePlay : ComponentBase
{
    #region 组件参数
    #region 用来渲染整个组件的委托
    /// <summary>
    /// 获取用来渲染整个组件的委托，
    /// 它的参数是一个ID，
    /// 必须把这个ID赋值给子内容中的一个元素，
    /// 否则本组件无法正常工作
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<string> RenderComponent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 组件的ID
    /// <summary>
    /// 获取组件的ID
    /// </summary>
    private string ID { get; } = CreateASP.JSObjectName();
    #endregion
    #region 重写OnParametersSetAsync
    protected override async Task OnParametersSetAsync()
    {
        await JSWindow.InvokeVoidAsync("ObserveVisiblePlay", ID);
    }
    #endregion
    #endregion
}
