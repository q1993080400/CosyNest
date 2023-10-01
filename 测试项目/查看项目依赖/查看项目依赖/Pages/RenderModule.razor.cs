using Microsoft.AspNetCore.Components;

namespace ViewDependencies;

/// <summary>
/// 这个组件被用来渲染单个模块
/// </summary>
public sealed partial class RenderModule : ComponentBase
{
    #region 组件参数
    #region 要渲染的模块
    /// <summary>
    /// 获取要渲染的模块
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Module Module { get; set; }
    #endregion
    #region 点击模块时触发的事件
    /// <summary>
    /// 点击模块时触发的事件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public EventCallback OnClick { get; set; }
    #endregion
    #endregion
}
