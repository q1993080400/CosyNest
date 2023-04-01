namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件可以根据不同的条件呈现不同的内容，
/// 而条件通过依赖注入获得
/// </summary>
/// <inheritdoc cref="ConditionRender{Condition}"/>
public sealed class ConditionInject<Condition> : ConditionRender<Condition>
{
    #region 获取呈现的条件
    [Inject]
    private Condition? GetConditionField { get; set; }

    protected override Condition? GetCondition => GetConditionField;
    #endregion
}
