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
    /// 通过反射浅拷贝对象，并返回它的副本
    /// </summary>
    /// <typeparam name="Ret">拷贝的返回值类型</typeparam>
    /// <param name="obj">被拷贝的对象</param>
    /// <returns></returns>
    [return: NotNullIfNotNull(nameof(obj))]
    public static Ret? MemberwiseClone<Ret>(this Ret? obj)
    {
        if (obj is null)
            return default;
        return MemberwiseCloneCache.Invoke<Ret>(obj)!;
    }
    #endregion
    #endregion
    #region 使用常用格式序列化数字
    /// <summary>
    /// 将数字序列化为常用格式
    /// </summary>
    /// <param name="num">待序列化的数字</param>
    /// <returns></returns>
    public static string FormatCommon(this decimal num)
        => num.ToString(Tool.FormattedNumCommon);
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
