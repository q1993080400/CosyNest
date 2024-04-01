using Microsoft.Extensions.Caching.Memory;

namespace System.Performance;

/// <summary>
/// 这个接口是一个通过内存实现的缓存
/// </summary>
/// <param name="getOrCreate">当缓存中不存在这个键的时候，
/// 通过这个委托创建并获取值，它的第一个参数是键，第二个参数是缓存条目，返回值就是被缓存的值</param>
/// <inheritdoc cref="ICache{Key, Value}"/>
sealed class MemoryCacheFrancis<Key, Value>(Func<Key, ICacheEntry, Value> getOrCreate) : ICache<Key, Value>
    where Key : notnull
{
    #region 公开成员
    #region 通过键获取值
    public Value this[Key key]
        => MemoryCache.GetOrCreate(key,
            x => getOrCreate((Key)x.Key, x))!;
    #endregion
    #endregion
    #region 内部成员
    #region 内存缓存
    /// <summary>
    /// 获取内存缓存，本对象的功能就是通过它实现的
    /// </summary>
    private MemoryCache MemoryCache { get; }
        = new(new MemoryCacheOptions());
    #endregion
    #endregion 
}
