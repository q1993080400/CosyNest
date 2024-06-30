using System.Reflection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个特性应用在实体类或者实体类的属性上，
/// 表示它是一个筛选条件
/// </summary>
/// <typeparam name="BusinessInterface">指示这个搜索条件所对应的业务接口，
/// 只有相同业务接口的搜索条件才会被放在一起</typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public abstract class FilterConditionAttribute<BusinessInterface> : Attribute
    where BusinessInterface : class, IGetRenderAllFilterCondition
{
    #region 公开成员
    #region 类型
    /// <summary>
    /// 描述要筛选的对象的类型，
    /// 仅当这个特性指定在类身上时，
    /// 或者与依附的属性类型不一致时，
    /// 才需要指定这个参数
    /// </summary>
    public Type? FilterType { get; init; }
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
    #endregion
    #region 抽象成员
    #region 获取渲染条件组
    /// <summary>
    /// 获取本特性所对应的渲染条件组
    /// </summary>
    /// <param name="memberInfo">本特性所依附的对象，
    /// 它可以是一个<see cref="Type"/>或<see cref="PropertyInfo"/></param>
    /// <returns></returns>
    public abstract RenderFilterGroup ConvertConditioGroup(MemberInfo memberInfo);
    #endregion
    #endregion
    #region 内部成员
    #region 获取筛选目标
    /// <summary>
    /// 根据特性所依附的成员智能获取筛选目标
    /// </summary>
    /// <param name="memberInfo">特性所依附的成员</param>
    /// <returns></returns>
    protected Type GetFilterTargets(MemberInfo memberInfo)
        => (FilterType, memberInfo) switch
        {
            ({ }, _) => FilterType,
            (null, PropertyInfo { PropertyType: { } propertyType }) => propertyType,
            _ => throw new NotSupportedException($"没有显式指定{nameof(FilterType)}，而且{memberInfo}不是属性，无法获取筛选目标")
        };
    #endregion
    #region 获取筛选对象类型
    /// <summary>
    /// 获取要筛选对象的类型
    /// </summary>
    /// <param name="type">筛选对象的类型</param>
    /// <returns></returns>
    protected static FilterObjectType GetFilterObjectType(Type type)
      => type switch
      {
          { IsEnum: true } => FilterObjectType.Enum,
          _ when type == typeof(string) => FilterObjectType.Text,
          _ when type == typeof(DateTimeOffset) || type == typeof(DateTime) => FilterObjectType.Date,
          _ when type == typeof(bool) => FilterObjectType.Bool,
          _ when type.IsNum() => FilterObjectType.Num,
          _ when Nullable.GetUnderlyingType(type) is { } underlyingType => GetFilterObjectType(underlyingType),
          _ => throw new NotSupportedException($"不能识别{type}的{nameof(FilterObjectType)}")
      };
    #endregion
    #region 获取属性访问表达式
    /// <summary>
    /// 获取属性访问表达式
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ConvertConditioGroup(MemberInfo)"/>
    protected static string GetPropertyAccess(MemberInfo memberInfo)
    => memberInfo switch
    {
        PropertyInfo { Name: { } name } => name,
        _ => throw new NotSupportedException($"{memberInfo.Name}不是属性，无法获取属性访问表达式")
    };
    #endregion
    #endregion
}
