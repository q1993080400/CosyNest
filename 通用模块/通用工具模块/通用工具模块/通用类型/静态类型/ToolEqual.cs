namespace System;

/// <summary>
/// 这个工具类为相等比较提供帮助
/// </summary>
public static class ToolEqual
{
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
