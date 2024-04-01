namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录表示成组的渲染筛选条件
/// </summary>
public sealed record RenderConditionGroup
{
    #region 类型
    /// <summary>
    /// 描述要筛选的对象的类型
    /// </summary>
    public required FilterObjectType FilterObjectType { get; init; }
    #endregion
    #region 排序
    /// <summary>
    /// 获取渲染顺序，它以升序排列
    /// </summary>
    public required int Order { get; init; }
    #endregion
    #region 描述
    /// <summary>
    /// 对要筛选的对象的描述文本
    /// </summary>
    public required string Describe { get; init; }
    #endregion
    #region 第一查询条件
    /// <summary>
    /// 获取第一查询条件，它是主要的查询条件
    /// </summary>
    public required RenderQueryCondition FirstQueryCondition { get; init; }
    #endregion
    #region 第二查询条件
    /// <summary>
    /// 获取第二查询条件，如果它不为<see langword="null"/>，按照规范，
    /// 表示应该渲染一个类似大于<see cref="FirstQueryCondition"/>，
    /// 且小于<see cref="SecondQueryCondition"/>的查询条件
    /// </summary>
    public required RenderQueryCondition? SecondQueryCondition { get; init; }
    #endregion
    #region 排序条件
    /// <summary>
    /// 获取一个描述如何渲染排序条件的对象，
    /// 如果为<see langword="null"/>，
    /// 表示不渲染排序条件
    /// </summary>
    public required RenderSortCondition? SortCondition { get; init; }
    #endregion
    #region 是否存在自定义默认值
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示存在自定义默认值，需要进行处理
    /// </summary>
    public bool HasDefaultValue
        => FirstQueryCondition.DefaultValue is { } ||
        SecondQueryCondition is { DefaultValue: { } };
    #endregion
}
