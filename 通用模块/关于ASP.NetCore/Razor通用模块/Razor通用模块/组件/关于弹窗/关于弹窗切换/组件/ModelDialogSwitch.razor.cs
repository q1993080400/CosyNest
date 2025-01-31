namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以用来提供切换弹窗的功能
/// </summary>
public sealed partial class ModelDialogSwitch : ComponentBase
{
    #region 组件参数
    #region 是否打开弹窗
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表弹窗处于打开状态，否则代表处于关闭状态，
    /// 注意：它仅在第一次初始化的时候生效
    /// </summary>
    [Parameter]
    public bool InitializationIsOpen { get; set; }
    #endregion
    #region 用来渲染组件的委托
    /// <summary>
    /// 获取用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    public RenderFragment<RenderModelDialogSwitchInfo> RenderComponent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 是否打开
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表弹窗处于打开状态，否则代表处于关闭状态
    /// </summary>
    private bool IsOpen { get; set; }
    #endregion
    #region 重写OnInitialized方法
    protected override void OnInitialized()
    {
        IsOpen = InitializationIsOpen;
    }
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderModelDialogSwitchInfo GetRenderInfo()
        => new()
        {
            IsOpen = IsOpen,
            SwitchOpen = () =>
            {
                IsOpen = !IsOpen;
                this.StateHasChanged();
            }
        };
    #endregion
    #endregion
}
