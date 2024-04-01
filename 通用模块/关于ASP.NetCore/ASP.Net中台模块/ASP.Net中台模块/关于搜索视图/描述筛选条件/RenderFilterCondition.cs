namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录描述如何渲染单个筛选条件
/// </summary>
public abstract record RenderFilterCondition
{
    #region 访问表达式
    /// <summary>
    /// 属性访问表达式，
    /// 通过它可以访问要查询或排序的属性
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
