using System.Collections.Immutable;

namespace System.Performance;

/// <summary>
/// 这个类型是<see cref="ICache{Key, Value}"/>的实现，
/// 它通过阈值进行垃圾回收
/// </summary>
/// <typeparam name="Key">缓存的键类型</typeparam>
/// <typeparam name="Value">缓存的值类型</typeparam>
/// <remarks>
/// 使用指定的添加键委托和垃圾回收阈值初始化缓存
/// </remarks>
/// <param name="getValue">在键不存在的时候，会通过这个委托获取值</param>
/// <param name="threshold">当字典的元素数量达到这个阈值的时候，会进行一次GC</param>
sealed class CacheThresholds<Key, Value>(Func<Key, Value> getValue, int threshold) : ICache<Key, Value>
    where Key : notnull
{
    #region 关于字典
    #region 封装的字典
    /// <summary>
    /// 获取封装的字典，本对象的功能就是通过它实现的
    /// </summary>
    private ImmutableDictionary<Key, UseValue> Dictionary { get; set; }
        = ImmutableDictionary<Key, UseValue>.Empty;
    #endregion
    #region 键不存在时获取值的委托
    /// <summary>
    /// 在读取值的时候，如果键不存在，
    /// 则会执行这个委托获取返回值，
    /// </summary>
    private Func<Key, Value> NotExist { get; } = getValue;
    #endregion
    #region 提取缓存数据
    #region 隐式提取
    public Value this[Key key]
        => GetValue(key, NotExist);
    #endregion
    #region 显式提取
    #region 提取缓存数据，如果不存在，则将其添加
    public Value TryAddValue(Key key, Func<Value> getValue)
        => GetValue(key, _ => getValue());
    #endregion
    #region 基础方法
    /// <summary>
    /// 获取缓存数据的基础方法
    /// </summary>
    /// <inheritdoc cref="ICache{Key, Value}.TryAddValue(Key, Func{Value})"/>
    private Value GetValue(Key key, Func<Key, Value> getValue)
    {
        if (Dictionary.TryGetValue(key, out var value))
        {
            value.UseCount++;
            return value.Value;
        }
        else
        {
            var v = getValue(key);
            Dictionary = Dictionary.SetItem(key, new UseValue(v));
            GC();
            return v;
        }
    }
    #endregion
    #endregion
    #endregion
    #endregion
    #region 关于垃圾回收
    #region 垃圾回收阈值
    /// <summary>
    /// 获取垃圾回收阈值， 
    /// 当字典的元素达到这个阈值后，将会对字典进行一次垃圾回收
    /// </summary>
    private int Threshold { get; } = Math.Max(75, threshold);                      //回收阈值最低为75
    #endregion
    #region 指示应该回收字典的前半部分还是后半部分
    /// <summary>
    /// 如果这个属性为<see langword="true"/>，代表应该回收字典的前半部分，
    /// 为<see langword="false"/>，代表应该回收后半部分
    /// </summary>
    private bool GCFront { get; set; } = true;

    /*注释：
      1.需要这个属性的原因在于：
      C#的字典不是完全意义上的堆栈结构，
      换句话来说，如果字典前面的一部分被移除，那么在继续添加元素时，
      新元素会填补前面的空位，而不是出现在旧元素的最后
      这导致了在进行垃圾回收时，
      如果是奇数次数，则先添加元素的在字典的前面，
      如果是偶数次数，则先添加的元素在后面，
      假如不加判断，会错误地将新元素移除，降低缓存命中率*/
    #endregion
    #region 执行垃圾回收
    /// <summary>
    /// 在向缓存字典添加元素时，会调用这个方法以开始垃圾回收
    /// </summary>
    private void GC()
    {
        #region 本地函数
        static int Fun(KeyValuePair<Key, UseValue> kv)
            => kv.Value.UseCount;
        #endregion
        var cache = Dictionary;
        if (cache.Count < Threshold)                      //如果元素数量没有达到阈值，则不进行回收
            return;
        var needGC = (int)(Threshold * 0.75);                                     //计算出需要垃圾回收的元素数量
        var list = cache.Take(GCFront ? ..needGC : needGC..).ToArray();           //只回收最先被添加到字典的四分之三元素
        var avg = list.Average(Fun);                                                                                    //计算出键被使用的平均次数
        var removeKey = list.Where(x => Fun(x) <= avg).Select(x => x.Key).ToArray();         //移除从未被使用，或使用次数低于平均值的元素
        cache = cache.RemoveRange(removeKey);
        cache.ForEach(x => x.Value.UseCount = 0);
        Dictionary = cache;
        GCFront = !GCFront;
    }

    #endregion
    #endregion
    #region 辅助类型
    /// <summary>
    /// 辅助类型，封装了值和值的使用次数
    /// </summary>
    private record UseValue(Value Value)
    {
        #region 获取值或设置的使用次数
        /// <summary>
        /// 获取或设置值的使用次数
        /// </summary>
        public int UseCount { get; set; }
        #endregion
    }
    #endregion
}
