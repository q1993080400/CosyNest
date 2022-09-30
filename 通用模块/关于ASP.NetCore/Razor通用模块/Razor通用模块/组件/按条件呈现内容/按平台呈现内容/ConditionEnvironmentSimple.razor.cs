using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件使用约定先于配置的方法，
/// 根据不同的平台呈现不同的内容
/// </summary>
public sealed partial class ConditionEnvironmentSimple : ComponentBase
{
    #region 说明文档
    /*本组件遵循的约定是：
      假设GetCondition.HardwareType返回的常量字符串是PC，
      Prefix属性为Prefix，
      则如果Assembly中找到了实现IComponent，
      且名称为PrefixPC的组件类型，则呈现它，
      否则去找名称为PrefixOther的组件，
      这个后缀具有更低优先级，但是可以匹配任何平台，
      如果还找不到，则不呈现任何内容*/
    #endregion
    #region 组件参数
    #region 组件所在的程序集
    /// <summary>
    /// 获取要呈现的组件所在的程序集
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Assembly Assembly { get; set; }
    #endregion
    #region 组件的前缀
    /// <summary>
    /// 获取要呈现的组件的前缀
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Prefix { get; set; }
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
