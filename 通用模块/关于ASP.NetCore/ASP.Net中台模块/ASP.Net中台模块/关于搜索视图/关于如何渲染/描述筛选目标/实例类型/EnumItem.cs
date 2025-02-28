namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录描述了枚举的名称还有值
/// </summary>
public sealed record EnumItem
{
    #region 静态成员：获取枚举的名称和值
    /// <summary>
    /// 如果<paramref name="type"/>是一个枚举，
    /// 则获取它的描述和值，否则返回一个空集合
    /// </summary>
    /// <param name="type">要获取枚举描述和值的类型</param>
    /// <returns></returns>
    public static EnumItem[] Create(Type type)
       => [.. type.GetEnumDescription().Select(static x => new EnumItem()
       {
           Describe = x.Describe,
           Value = x.Value.ToString()
       })];
    #endregion
    #region 枚举的值
    /// <summary>
    /// 获取枚举的值
    /// </summary>
    public required string Value { get; init; }
    #endregion
    #region 枚举的描述
    /// <summary>
    /// 获取枚举的描述
    /// </summary>
    public required string Describe { get; init; }
    #endregion
}
