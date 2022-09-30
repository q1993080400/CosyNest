using System.Performance;

namespace System.Maths;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个计量单位的模板，
/// 它规定了该单位的大小和类型
/// </summary>
public interface IUT : IComparable<IUT>, IEquatable<IUT>
{
    #region 说明文档
    /*问：如何确定每种计量单位的公制单位？
      答：本接口基于约定优于配置的原则，公制单位需遵守以下约定：
      1.必须是实现IUT的类型中声明的公开，静态属性，属性类型需实现IUT
      2.属性名称以Metric结尾，对大小写敏感
      3.符合条件的属性不能没有，也不能多于一个，否则会引发异常*/
    #endregion
    #region 静态成员
    #region 获取公制单位
    #region 非泛型方法
    /// <summary>
    /// 获取指定类型计量单位的公制单位
    /// </summary>
    /// <param name="utType">要获取公制单位的计量单位类型，
    /// 必须实现<see cref="IUT"/></param>
    /// <returns>获取到的公制单位</returns>
    public static IUT GetMetric(Type utType)
        => MetricCache[utType];
    #endregion
    #region 泛型方法
    /// <typeparam name="UT">要获取公制单位的类型</typeparam>
    /// <inheritdoc cref="GetMetric(Type)"/>
    public static UT GetMetric<UT>()
        where UT : IUT
        => (UT)GetMetric(typeof(UT));
    #endregion
    #endregion
    #region 公制单位的缓存字典
    /// <summary>
    /// 公制单位的缓存字典，有了这个属性以后，
    /// 只有第一次读取公制单位时，才需要使用反射获取特性
    /// </summary>
    private static ICache<Type, IUT> MetricCache { get; }
        = CreatePerformance.CacheThreshold(x =>
        {
            #region 用来检查类型的本地函数
            static bool Check(Type type)
            => typeof(IUT).IsAssignableFrom(type)!;
            #endregion
            if (!Check(x))
                throw new NotSupportedException($"{x}没有实现{nameof(IUT)}");
            var regex = /*language=regex*/"Metric$".Op().Regex();
            var pro = x.GetTypeData().Propertys.Where(x => x.IsStatic() && regex.IsMatch(x.Name) && Check(x.PropertyType) && x.IsPublic()).ToArray();
            return pro.Length switch
            {
                0 => throw new Exception($"{x}中没有符合约定的公制单位"),
                1 => pro[0].GetValue<IUT>() ?? throw new NullReferenceException("储存公制单位的静态属性返回null"),
                _ => throw new Exception($"{x}中存在多个符合约定的公制单位，它们是：{pro.Select(p => p.Name).Join("，")}")
            };
        }, 75, MetricCache);
    #endregion
    #endregion
    #region 有关单位换算
    #region 从本单位换算为公制单位
    /// <summary>
    /// 将本单位换算为为公制单位
    /// </summary>
    /// <param name="thisUnit">待换算的本单位的数量</param>
    /// <returns>换算后的公制单位的数量</returns>
    Num ToMetric(Num thisUnit);
    #endregion
    #region 从公制单位换算为本单位
    /// <summary>
    /// 将公制单位换算为本单位
    /// </summary>
    /// <param name="metricUnit">待换算的公制单位的数量</param>
    /// <returns>换算后的本单位的数量</returns>
    Num FromMetric(Num metricUnit);
    #endregion
    #region 返回本单位的大小
    /// <summary>
    /// 返回本单位的大小，
    /// 也就是1单位的本单位，等于多少公制单位
    /// </summary>
    Num Size
       => ToMetric(1);
    #endregion
    #endregion
    #region 关于本单位的信息
    #region 是否为静态单位
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表本单位为静态单位，否则为动态单位
    /// </summary>
    bool IsStatic { get; }

    /*问：什么是静态单位和动态单位？
      答：静态单位指不同单位的换算标准为一个常数，
      例如重量，面积单位等，动态单位指换算标准会随时间变化，
      例如货币汇率等*/
    #endregion
    #region 返回单位名称
    /// <summary>
    /// 获取本单位的名称
    /// </summary>
    string Name { get; }
    #endregion
    #region 返回本单位是否为公制单位
    /// <summary>
    /// 返回本单位是否为公制单位
    /// </summary>
    bool IsMetric
       => Equals(this, GetMetric(GetType()));
    #endregion
    #endregion
}
