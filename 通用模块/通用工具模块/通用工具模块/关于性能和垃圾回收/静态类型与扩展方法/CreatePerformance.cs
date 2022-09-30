namespace System.Performance;

/// <summary>
/// 这个静态类可以用来帮助创建和性能有关的对象
/// </summary>
public static class CreatePerformance
{
    #region 创建缓存
#pragma warning disable IDE0060
    #region 使用阈值GC算法
    /// <summary>
    /// 创建一个缓存，
    /// 当键不存在时，它将键值对添加到缓存，
    /// 并且支持阈值垃圾回收算法
    /// </summary>
    /// <typeparam name="Key">缓存的键类型</typeparam>
    /// <typeparam name="Value">缓存的值类型</typeparam>
    /// <param name="getValue">当键不存在时，通过这个委托获取值，并将其添加到缓存中</param>
    /// <param name="threshold">当缓存元素达到这个阈值时，开始垃圾回收</param>
    /// <param name="infer">这个参数的唯一作用就是推断泛型类型，
    /// 如果已经显式指定，则可以不填</param>
    /// <returns></returns>
    public static ICache<Key, Value> CacheThreshold<Key, Value>(Func<Key, Value> getValue, int threshold, ICache<Key, Value>? infer = default)
        where Key : notnull
        => new CacheThresholds<Key, Value>(getValue, threshold);
    #endregion
#pragma warning restore IDE0060
    #endregion
}
