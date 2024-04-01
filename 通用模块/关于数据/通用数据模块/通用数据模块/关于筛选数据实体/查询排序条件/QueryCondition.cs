using System.Text.Json;

namespace System.DataFrancis;

/// <summary>
/// 这个记录描述查询实体的条件
/// </summary>
public sealed record QueryCondition : DataCondition
{
    #region 逻辑运算符
    /// <summary>
    /// 获取用来比较实体的逻辑运算符
    /// </summary>
    public required LogicalOperator LogicalOperator { get; init; }
    #endregion
    #region 用来比较的值
    private object? CompareValueField { get; set; }

    /// <summary>
    /// 获取用来和实体类属性进行比较的值
    /// </summary>
    public required object? CompareValue
    {
        get => CompareValueField;
        init => CompareValueField = (value is JsonElement e ? e.Deserialize() : value) switch
        {
            { } v when !v.GetType().IsCommonType()
            => throw new NotSupportedException($"{v}不是基本类型，不能作为查询条件中用来进行比较的值"),
            var v => v
        };
    }
    #endregion
}
