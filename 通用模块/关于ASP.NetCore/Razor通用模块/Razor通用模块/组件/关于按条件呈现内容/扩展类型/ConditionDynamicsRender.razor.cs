using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件能够通过不同的条件，
/// 动态渲染不同类型的组件
/// </summary>
/// <typeparam name="Condition">渲染条件的类型</typeparam>
public sealed partial class ConditionDynamicsRender<Condition> : ComponentBase
{
    #region 组件参数
    #region 渲染条件
    /// <summary>
    /// 获取或设置渲染条件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Condition RenderCondition { get; set; }
    #endregion
    #region 组件所在的程序集
    /// <summary>
    /// 获取要呈现的组件所在的程序集
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Assembly Assembly { get; set; }
    #endregion
    #region 根据条件和程序集返回组件类型的委托
    /// <summary>
    /// 这个委托的第一个参数是呈现条件，
    /// 第二个参数是组件所在的程序集，
    /// 返回值是最终呈现的组件类型
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Condition, Assembly, Type> GetComponentType { get; set; }
    #endregion
    #region 传递给组件的参数
    /// <summary>
    /// 获取应该传递到呈现后的组件的参数
    /// </summary>
    [Parameter]
    public IDictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    #endregion
    #endregion
}
