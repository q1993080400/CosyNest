namespace System;

/// <summary>
/// 这个静态类使用泛型数学来进行数学运算，
/// 它具有更低的耦合
/// </summary>
public static class MathBroad
{
    #region 返回比较小的一方
    /// <summary>
    /// 返回两个对象中比较小的一方
    /// </summary>
    /// <typeparam name="Num">要比较的对象的类型</typeparam>
    /// <param name="a">要比较的第一个对象</param>
    /// <param name="b">要比较的第二个对象</param>
    /// <returns></returns>
    public static Num Min<Num>(Num a, Num b)
        where Num : IComparable<Num>
    {
        var compare = a.CompareTo(b);
        return compare <= 0 ? a : b;
    }
    #endregion
}
