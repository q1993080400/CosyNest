using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace System.Performance;

/// <summary>
/// 这个接口是一个通过内存实现的缓存
/// </summary>
/// <param name="getOrCreate">当缓存中不存在这个键的时候，
/// 通过这个委托创建并获取值，它的参数是键，返回值就是被缓存的值</param>
/// <inheritdoc cref="ICache{Key, Value}"/>
sealed class MemoryCacheFrancis<Key, Value>(Func<Key, Value> getOrCreate) : ICache<Key, Value>
    where Key : notnull
{
    #region 公开成员
    #region 通过键获取值
    public Value this[Key key]
    {
        get
        {
            if (MemoryCache.TryGetValue(key, out var value))
                return value;
            value = getOrCreate(key);
            SetValue(key, value);
            return value;
        }
    }
    #endregion
    #region 尝试获取元素
    public bool TryGetValue(Key key, [NotNullWhen(true)] out Value? value)
    {
        var exist = MemoryCache.TryGetValue(key, out var objValue);
        value = objValue is Value v ? v : default;
        return exist;
    }
    #endregion
    #region 显式设置元素
    public void SetValue(Key key, Value value)
         => MemoryCache = MemoryCache.SetItem(key, value);
    #endregion
    #endregion
    #region 内部成员
    #region 内存缓存
    /// <summary>
    /// 获取内存缓存，本对象的功能就是通过它实现的
    /// </summary>
    private ImmutableDictionary<Key, Value> MemoryCache { get; set; }
        = ImmutableDictionary<Key, Value>.Empty;
    #endregion
    #endregion
}
