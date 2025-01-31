namespace System.Performance;

/// <summary>
/// 这个静态类可以用来帮助创建和性能有关的对象
/// </summary>
public static class CreatePerformance
{
    #region 创建内存缓存
    #region 直接创建
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
    #region 创建只能显式添加值的缓存
    /// <summary>
    /// 创建一个内存中的缓存，注意：
    /// 它不支持缓存过期，因此只适合缓存永远不会改变和删除的数据，
    /// 而且它不能根据键自动获取缓存项，需要手动将键值对添加到缓存，才能正常访问
    /// </summary>
    /// <inheritdoc cref="MemoryCacheFrancis{Key, Value}"/>
    /// <inheritdoc cref="MemoryCacheFrancis{Key, Value}.MemoryCacheFrancis(Func{Key, Value})"/>
    public static ICache<Key, Value> MemoryCache<Key, Value>()
        where Key : notnull
        => MemoryCache<Key, Value>(_ => throw new NotSupportedException("不支持自动获取缓存项，你需要手动将它添加到缓存"));
    #endregion
    #endregion
}
