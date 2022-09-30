namespace System;

/// <summary>
/// 这个工具类为相等比较提供帮助
/// </summary>
public static class ToolEqual
{
    #region 比较两个对象的值相等性
    /// <summary>
    /// 通过反射比较两个对象的值，比较的内容包括：
    /// 所有属性和字段，无论访问级别
    /// </summary>
    /// <param name="a">要比较的第一个对象</param>
    /// <param name="b">要比较的第二个对象</param>
    /// <returns></returns>
    public static bool IsEquivalent(object? a, object? b)
        => JudgeNull(a, b) ??
        a!.GetType() == b!.GetType() &&
        a.GetTypeData().Fields.All(f => f.IsStatic ||
            Equals(f.GetValue(a), f.GetValue(b)));

    /*注释：
      问：为什么这个方法只比较字段？
      答：如果一个属性需要保存状态，那么C#在声明属性时，
      实际上也声明了一个私有字段，因此只要保证所有字段的值相同，
      就能保证两个对象的状态相同
      另外，本方法不比较静态字段，
      因为静态字段与实例对象的状态无关*/
    #endregion
    #region 判断两个对象是否为null
    /// <summary>
    /// 如果两个对象均为<see langword="null"/>或引用相等，返回<see langword="true"/>，
    /// 只有一个为<see langword="null"/>，返回<see langword="false"/>，
    /// 都不为<see langword="null"/>，返回<see langword="null"/>
    /// </summary>
    /// <param name="a">要比较的第一个对象</param>
    /// <param name="b">要比较的第二个对象</param>
    /// <returns></returns>
    public static bool? JudgeNull(object? a, object? b)
    {
        if (a == b)
            return true;
        return a is null || b is null ? false : null;
    }
    #endregion
    #region 使用多个对象生成哈希值
    /// <summary>
    /// 使用多个对象生成一个哈希值
    /// </summary>
    /// <param name="objs">用于计算哈希值的对象</param>
    /// <returns>最后生成的哈希值，
    /// 如果<paramref name="objs"/>中的所有元素全部相同，
    /// 则生成的哈希值也相同</returns>
    public static int CreateHash(params object[] objs)
    {
        var hash = new HashCode();
        foreach (var item in objs)
            hash.Add(item);
        return hash.ToHashCode();
    }
    #endregion
}
