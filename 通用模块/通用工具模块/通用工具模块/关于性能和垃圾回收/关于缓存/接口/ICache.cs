namespace System.Performance
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视为一个缓存
    /// </summary>
    /// <typeparam name="Key">用来提取缓存的键类型</typeparam>
    /// <typeparam name="Value">被缓存的类型</typeparam>
    public interface ICache<in Key, out Value>
        where Key : notnull
    {
        #region 说明文档
        /*问：缓存和字典有何区别？
          答：它们的功能和API非常类似，但缓存为特定用途专门优化过，
          支持垃圾回收，线程安全等特性，更适应缓存的本职工作，而字典没有

          实现本接口请遵循以下规范：
          #缓存必须支持自动垃圾回收，因为缓存一般是静态对象，
          如果不支持GC，很容易造成内存泄漏，
          因此，如果需要缓存的对象不能被回收，
          那么应该使用字典，而不是缓存

          #在通过键提取缓存时，如果键不存在，不要引发异常，
          而是通过一个委托（或其他方式）获取值，
          这个委托应该在缓存对象被创建的时候传入。
          这样做的原因是作者认为，对于调用者来说，
          直接使用缓存即可，不应该关心数据是怎么来的，以及如何将数据添加到缓存
          
          #本接口的所有API都需要考虑线程安全，
          因为缓存通常是静态的，而且很容易被多个线程同时访问*/
        #endregion
        #region 提取缓存数据
        /// <summary>
        /// 通过键提取缓存中的数据
        /// </summary>
        /// <param name="key">用来提取缓存的键</param>
        /// <returns>获取到的缓存数据，如果键不存在，不会引发异常</returns>
        Value this[Key key] { get; }
        #endregion
    }
}
