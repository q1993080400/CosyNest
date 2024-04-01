namespace System.DataFrancis;

/// <summary>
/// 这个类型是描述实体数据条件的基类
/// </summary>
public abstract record DataCondition
{
    #region 属性访问表达式
    /// <summary>
    /// 获取属性访问表达式，
    /// 它决定应该访问实体类的什么属性，支持递归
    /// </summary>
    public required string PropertyAccess { get; init; }
    #endregion
    #region 是否虚拟筛选
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示它是一个虚拟筛选条件，
    /// 它包含比较复杂的逻辑，
    /// 不映射到具体的一个实体类属性上
    /// </summary>
    public required bool IsVirtually { get; init; }
    #endregion
}
