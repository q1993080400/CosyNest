namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是一个渲染描述中间对象，
/// 它可以简化动态生成渲染描述
/// </summary>
public abstract record FilterConditionInfo
{
    #region 类型
    /// <summary>
    /// 描述要筛选的对象的类型
    /// </summary>
    public required FilterObjectType FilterObjectType { get; init; }
    #endregion
    #region 枚举的值
    /// <summary>
    /// 如果要筛选枚举，
    /// 则这个集合描述枚举可能的值和描述
    /// </summary>
    public IReadOnlyCollection<EnumItem> EnumItem { get; init; } = [];
    #endregion
    #region 描述
    /// <summary>
    /// 对要筛选的对象的描述文本
    /// </summary>
    public required string Describe { get; init; }
    #endregion
    #region 渲染顺序
    /// <summary>
    /// 获取渲染顺序，它以升序排列
    /// </summary>
    public int Order { get; init; }
    #endregion
    #region 是否可排序
    /// <summary>
    /// 是否可排序
    /// </summary>
    public bool CanSort { get; init; }
    #endregion
    #region 排除查询条件
    /// <summary>
    /// 如果这个值不为<see langword="null"/>，
    /// 表示当这个条件的值等于这个字面量的时候，
    /// 排除这个查询条件
    /// </summary>
    public string? ExcludeFilter { get; init; }
    #endregion
    #region 是否为虚拟筛选
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示它是一个虚拟筛选，不直接映射到某个具体的属性上
    /// </summary>
    public bool IsVirtually { get; init; }
    #endregion
    #region 抽象成员：获取渲染条件组
    /// <summary>
    /// 获取本对象所对应的渲染条件组
    /// </summary>
    /// <returns></returns>
    public abstract RenderFilterGroup ConvertConditioGroup();
    #endregion
}
