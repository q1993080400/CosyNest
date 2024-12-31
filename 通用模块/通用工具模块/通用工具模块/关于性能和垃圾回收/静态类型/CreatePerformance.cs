namespace System.Performance;

/// <summary>
/// 这个静态类可以用来帮助创建和性能有关的对象
/// </summary>
public static class CreatePerformance
{
    #region 创建内存缓存
    /// <summary>
    /// 创建一个内存中的缓存，注意：
    /// 它不支持缓存过期，因此只适合缓存永远不会改变和删除的数据
    /// </summary>
    /// <inheritdoc cref="MemoryCacheFrancis{Key, Value}"/>
    /// <inheritdoc cref="MemoryCacheFrancis{Key, Value}.MemoryCacheFrancis(Func{Key, Value})"/>
    public static ICache<Key, Value> MemoryCache<Key, Value>(Func<Key, Value> getOrCreate)
        where Key : notnull
        => new MemoryCacheFrancis<Key, Value>(getOrCreate);
    #endregion
}
