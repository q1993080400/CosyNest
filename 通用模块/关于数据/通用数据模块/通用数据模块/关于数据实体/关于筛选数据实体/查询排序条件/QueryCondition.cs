namespace System.DataFrancis;

/// <summary>
/// 这个记录描述查询实体的条件
/// </summary>
/// <typeparam name="Obj">查询实体的类型</typeparam>
public sealed record QueryCondition<Obj> : DataCondition<Obj>
{
    #region 逻辑运算符
    /// <summary>
    /// 获取用来比较实体的逻辑运算符
    /// </summary>
    public required LogicalOperator LogicalOperator { get; init; }
    #endregion
    #region 用来比较的值
    /// <summary>
    /// 获取用来和实体类属性进行比较的值
    /// </summary>
    public required object? CompareValue { get; init; }
    #endregion
}
