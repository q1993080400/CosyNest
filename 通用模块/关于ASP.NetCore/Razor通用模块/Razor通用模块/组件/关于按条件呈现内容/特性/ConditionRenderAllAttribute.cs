namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个特性可以声明在组件上，
/// 指示它可以被<see cref="ConditionDynamicsRender{Condition}"/>所渲染，
/// 在所有其他条件不匹配的时候，匹配这个条件
/// </summary>
/// <typeparam name="Condition">渲染条件的类型</typeparam>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ConditionRenderAllAttribute<Condition> : ConditionRenderAttribute<Condition>
{

}
