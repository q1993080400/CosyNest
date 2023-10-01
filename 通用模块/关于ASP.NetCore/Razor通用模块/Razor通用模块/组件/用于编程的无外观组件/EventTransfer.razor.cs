using System.Design;
using System.Reflection.Metadata;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件允许子组件向父组件注册事件
/// </summary>
public sealed partial class EventTransfer<Event> : ComponentBase, IContentComponent<RenderFragment<IEventInvoke>>
    where Event : Delegate
{
    #region 说明文档
    /*问：本组件有什么作用？
      答：本组件允许子组件向父组件注册事件，举例说明：
      如果你需要这样一个功能：
      在点击父组件的任意区域，关闭子组件（这个功能在右键菜单之中非常常见），
      那么你就可以通过本组件，让子组件注册父组件的onclick事件
    
      问：如何使用本组件？
      答：将父组件放在本组件的内部，本组件提供了两个级联参数，
      IEventSubscribe接口供子组件注册事件使用，
      IEventInvoke接口供父组件调用事件使用，
      让它们接收这两个级联参数并使用即可
    
      按照规范，这两个接口的实现应当使用弱事件，
      不需要考虑如何取消它们的注册*/
    #endregion
    #region 组件参数
    #region 级联参数名称
    /// <summary>
    /// 获取向下传递的级联参数的名称
    /// </summary>
    [Parameter]
    public string? CascadingParameterName { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<IEventInvoke> ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 封装的弱事件
    /// <summary>
    /// 获取封装的弱事件，
    /// 它可以用来注册和调用事件
    /// </summary>
    private WeakDelegate<Event> PackEvent { get; } = new();
    #endregion
    #endregion
}
