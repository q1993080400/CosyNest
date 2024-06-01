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
    #region 访问表达式
    /// <summary>
    /// 属性访问表达式，
    /// 通过它可以访问要查询或排序的属性，
    /// 如果为<see langword="null"/>，
    /// 且这个特性被应用在属性身上，则自动获取，
    /// 对于虚拟筛选条件，它作为标识这个筛选条件的ID
    /// </summary>
    public string? PropertyAccess { get; init; }
    #endregion
    #region 类型
    /// <summary>
    /// 描述要筛选的对象的类型，
    /// 仅当这个特性指定在类身上时，
    /// 或者与依附的属性类型不一致时，
    /// 才需要指定这个参数
    /// </summary>
    public FilterObjectType FilterObjectType { get; init; }
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
    #region 是否虚拟筛选
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示它是一个虚拟筛选条件，
    /// 它包含比较复杂的逻辑，
    /// 不映射到具体的一个实体类属性上
    /// </summary>
    public bool IsVirtually { get; init; }
    #endregion
    #endregion
    #region  抽象成员
    #region 获取渲染条件组
    /// <summary>
    /// 获取本特性所对应的渲染条件组
    /// </summary>
    /// <param name="memberInfo">本特性所依附的对象，
    /// 它可以是一个<see cref="Type"/>或<see cref="PropertyInfo"/></param>
    /// <returns></returns>
    public abstract RenderConditionGroup ConvertConditioGroup(MemberInfo memberInfo);
    #endregion
    #endregion
    #region 内部成员
    #region 获取筛选对象类型
    /// <summary>
    /// 获取要筛选对象的类型
    /// </summary>
    /// <param name="type">原始的筛选类型</param>
    /// <returns></returns>
    /// <inheritdoc cref="ConvertConditioGroup(MemberInfo)"/>
    private static protected FilterObjectType GetFilterObjectType(FilterObjectType type, MemberInfo memberInfo)
      => (type, memberInfo) switch
      {
          (not FilterObjectType.None, _) => type,
          (_, PropertyInfo { PropertyType: { } propertyType }) =>
          propertyType switch
          {
              { IsEnum: true } => FilterObjectType.Enum,
              _ when propertyType == typeof(DateTimeOffset) => FilterObjectType.Date,
              _ when propertyType == typeof(bool) => FilterObjectType.Bool,
              _ when propertyType.IsNum() => FilterObjectType.Num,
              var pt => Type.GetTypeCode(pt) switch
              {
                  TypeCode.String => FilterObjectType.Text,
                  TypeCode.DateTime => FilterObjectType.Date,
                  var typeCode => throw new NotSupportedException($"不能识别{typeCode}的{nameof(type)}")
              }
          },
          (var filterObjectType, _) => filterObjectType
      };
    #endregion
    #region 获取属性访问表达式
    /// <summary>
    /// 获取属性访问表达式
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ConvertConditioGroup(MemberInfo)"/>
    private protected string GetPropertyAccess(MemberInfo memberInfo)
     => (PropertyAccess, memberInfo) switch
     {
         ({ } propertyAccess, _) => propertyAccess,
         (null, PropertyInfo { Name: { } name }) => name,
         _ => throw new NotSupportedException($"{memberInfo.Name}不是属性，" +
             $"而且没有显式指定{nameof(PropertyAccess)}，无法获取属性访问表达式")
     };
    #endregion
    #region 获取枚举的名称和值
    /// <summary>
    /// 如果<paramref name="memberInfo"/>是一个枚举，
    /// 则获取它的描述和值，否则返回一个空集合
    /// </summary>
    /// <param name="memberInfo">要获取枚举描述和值的类型成员</param>
    /// <returns></returns>
    private protected static EnumItem[] GetEnumItem(MemberInfo memberInfo)
    {
        var type = memberInfo switch
        {
            PropertyInfo { PropertyType: { } pt } => pt,
            Type t => t,
            var member => throw new NotSupportedException($"{member}既不是一个属性也不是一个类型，无法识别它")
        };
        return type.GetEnumDescription().Select(x => new EnumItem()
        {
            Describe = x.Describe,
            Value = x.Value.ToString()
        }).ToArray();
    }
    #endregion
    #endregion
}
