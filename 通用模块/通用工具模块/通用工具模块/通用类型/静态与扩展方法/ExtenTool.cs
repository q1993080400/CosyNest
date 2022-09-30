using System.Maths;

namespace System;

/// <summary>
/// 所有通用的扩展方法全部放在这个静态类中，通常无需专门调用
/// </summary>
public static partial class ExtenTool
{
    //本部分类专门用于储存通用的扩展方法

    #region 关于枚举
    #region 枚举枚举的所有位域
    /// <summary>
    /// 枚举一个枚举的所有位域，
    /// 即便该枚举没有<see cref="FlagsAttribute"/>特性，也不受影响
    /// </summary>
    /// <typeparam name="Obj">待返回位域的枚举类型</typeparam>
    /// <param name="obj">待返回位域的枚举</param>
    /// <returns></returns>
    public static IEnumerable<Obj> AllFlag<Obj>(this Obj obj)
        where Obj : Enum
        => ToolBit.AllFlag(obj.To<int>()).
        Select(x => (Obj)Enum.ToObject(typeof(Obj), x.Power));
    #endregion
    #region 从枚举中删除位域
    /// <summary>
    /// 从枚举中删除位域
    /// </summary>
    /// <typeparam name="Obj">待删除位域的枚举类型</typeparam>
    /// <param name="obj">待删除位域的枚举</param>
    /// <param name="remove">要删除的枚举位域</param>
    /// <returns></returns>
    public static Obj RemoveFlag<Obj>(this Obj obj, params Obj[] remove)
        where Obj : Enum
    {
        var bit = ToolBit.RemoveFlag(obj.To<int>(), remove.Select(x => x.To<int>()).ToArray());
        return (Obj)Enum.ToObject(typeof(Obj), bit);
    }
    #endregion
    #endregion
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
}
