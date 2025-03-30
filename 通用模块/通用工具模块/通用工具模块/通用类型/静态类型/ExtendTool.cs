using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System;

/// <summary>
/// 所有通用的扩展方法全部放在这个静态类中，通常无需专门调用
/// </summary>
public static partial class ExtendTool
{
    //本部分类专门用于储存通用的扩展方法

    #region 返回一个IComparer的取反
    /// <summary>
    /// 返回一个<see cref="IComparer{T}"/>的取反，
    /// 它可以更方便地实现倒序排序
    /// </summary>
    /// <typeparam name="Obj">要比较的对象类型</typeparam>
    /// <param name="comparable">要取反的<see cref="IComparer{T}"/></param>
    /// <returns></returns>
    public static IComparer<Obj> Negate<Obj>(this IComparer<Obj> comparable)
        => FastRealize.Comparer<Obj>((x, y) => -comparable.Compare(x, y));
    #endregion
    #region 返回延迟对象的值（可以对null使用）
    /// <summary>
    /// 返回一个延迟对象的值，
    /// 如果这个延迟对象为<see langword="null"/>，
    /// 则返回默认值
    /// </summary>
    /// <typeparam name="Obj">延迟对象的值的类型</typeparam>
    /// <param name="lazy">要返回值的延迟对象</param>
    /// <returns></returns>
    public static Obj? Value<Obj>(this Lazy<Obj>? lazy)
        => lazy is null ? default : lazy.Value;
    #endregion
    #region 浅拷贝对象
    #region 缓存方法
    /// <summary>
    /// 返回浅拷贝的缓存方法
    /// </summary>
    private static MethodInfo MemberwiseCloneCache { get; }
    = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance)!;
    #endregion
    #region 正式方法
    /// <summary>
    /// 通过反射浅拷贝对象，并返回它的副本，
    /// 如果它实现了<see cref="ICloneable{Obj}"/>，会执行深拷贝
    /// </summary>
    /// <typeparam name="Ret">拷贝的返回值类型</typeparam>
    /// <param name="obj">被拷贝的对象</param>
    /// <returns></returns>
    [return: NotNullIfNotNull(nameof(obj))]
    public static Ret? MemberwiseClone<Ret>(this Ret? obj)
        => obj switch
        {
            null => default,
            ICloneable<Ret> cloneable => cloneable.Cloneable(),
            _ => MemberwiseCloneCache.Invoke<Ret>(obj)!
        };
    #endregion
    #endregion
    #region 关于格式化
    #region 使用常用格式序列化数字
    /// <summary>
    /// 将数字序列化为常用格式
    /// </summary>
    /// <param name="num">待序列化的数字</param>
    /// <returns></returns>
    public static string FormatCommon(this decimal num)
        => num.ToString(Format.FormattedNumCommon);
    #endregion
    #region 格式化日期
    /// <summary>
    /// 将日期格式化为中国习惯的形式
    /// </summary>
    /// <param name="date">要格式化的日期</param>
    /// <param name="retentionTime">如果这个值为<see langword="true"/>，
    /// 则保留时间部分，否则放弃时间部分</param>
    /// <returns></returns>
    public static string FormatChinese(this DateTimeOffset date, bool retentionTime = true)
        => date.ToString(retentionTime ? Format.FormatChineseWithTimeText : Format.FormatChineseText);
    #endregion
    #region 将日期格式化为友好格式
    /// <summary>
    /// 将日期格式化为友好格式，类似昨天23点
    /// </summary>
    /// <param name="date">要格式化的日期</param>
    /// <returns></returns>
    public static string FormatFriendly(this DateTimeOffset date)
    {
        const string format = /*language=DateTimeFormat*/"yyyy/M/d HH:MM";
        var now = DateTimeOffset.Now;
        if (date > now)
            return date.ToString(format);
        var toDay = now.Today();
        if (date >= toDay)
            return date.ToString("HH:mm");
        if (date >= toDay.AddDays(-1))
            return date.ToString("昨天HH:mm");
        return date >= toDay.AddDays(-2) ?
            date.ToString("前天HH:mm") : date.ToString(format);
    }
    #endregion
    #region 格式化时间
    /// <summary>
    /// 将时间格式化为中国习惯的格式
    /// </summary>
    /// <param name="timeOnly">要格式化的时间</param>
    /// <returns></returns>
    public static string FormatChinese(this TimeOnly timeOnly)
        => timeOnly.ToString("H时m分");
    #endregion
    #endregion
    #region 如果一个对象为null，则引发异常
    /// <summary>
    /// 如果一个对象为<see langword="null"/>，
    /// 则引发异常，否则返回它本身
    /// </summary>
    /// <typeparam name="Obj">要检查的对象的类型</typeparam>
    /// <param name="obj">要检查的对象</param>
    /// <returns></returns>
    public static Obj ThrowIfNull<Obj>(this Obj? obj)
        where Obj : class
        => obj ?? throw new NullReferenceException($"指定的{typeof(Obj)}对象为null");
    #endregion
}
