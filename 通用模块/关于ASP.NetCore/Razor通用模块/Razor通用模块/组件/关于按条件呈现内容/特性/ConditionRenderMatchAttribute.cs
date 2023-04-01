namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个特性可以声明在组件上，
/// 指示它匹配指定条件，可以被<see cref="ConditionDynamicsRender{Condition}"/>所渲染
/// </summary>
/// <typeparam name="Condition">渲染条件的类型</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ConditionRenderMatchAttribute<Condition> : ConditionRenderAttribute<Condition>
{
    #region 匹配的渲染条件
    /// <summary>
    /// 获取与之匹配的渲染条件，
    /// 如果为<see langword="null"/>，表示匹配所有条件
    /// </summary>
    public required Condition? RenderCondition { get; init; }
    #endregion
}
