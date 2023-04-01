namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个特性可以声明在组件上，
/// 指示它匹配一个条件，可以被<see cref="ConditionDynamicsRender{Condition}"/>所渲染
/// </summary>
/// <typeparam name="Condition">渲染条件的类型</typeparam>
public abstract class ConditionRenderAttribute<Condition> : Attribute
{

}
