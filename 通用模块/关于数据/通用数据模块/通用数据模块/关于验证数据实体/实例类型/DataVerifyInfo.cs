using System.Performance;
using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来创建<see cref="DataVerify"/>的参数
/// </summary>
public sealed record DataVerifyInfo
{
    #region 用来获取需要验证的属性的委托
    #region 正式属性
    /// <summary>
    /// 这个委托传入实体类的类型，
    /// 返回所有需要验证的属性
    /// </summary>
    public Func<Type, IEnumerable<PropertyInfo>> GetVerifyPropertys { get; init; } = GetVerifyPropertiesDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// <see cref="GetVerifyPropertys"/>的默认值，
    /// 它通过检查属性中是否存在<see cref="RenderDataAttribute"/>特性来判断是否需要验证
    /// </summary>
    /// <param name="type">实体类的类型</param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetVerifyPropertiesDefault(Type type)
        => GetVerifyPropertysCache[type];
    #endregion
    #region 缓存
    /// <summary>
    /// 这个属性是<see cref="GetVerifyPropertiesDefault(Type)"/>方法的缓存
    /// </summary>
    private static ICache<Type, IEnumerable<PropertyInfo>> GetVerifyPropertysCache { get; }
    = CreatePerformance.MemoryCache<Type, IEnumerable<PropertyInfo>>(
        type => [.. type.GetPropertyInfoAlmighty(true).Where(x => x.IsDefined<RenderDataAttribute>())]);
    #endregion
    #endregion
}
