namespace System.Performance;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以被视为一个缓存
/// </summary>
/// <typeparam name="Key">用来提取缓存的键类型</typeparam>
/// <typeparam name="Value">被缓存的类型</typeparam>
public interface ICache<in Key, out Value>
    where Key : notnull
{
    #region 提取缓存数据
    /// <summary>
    /// 通过键提取缓存中的数据
    /// </summary>
    /// <param name="key">用来提取缓存的键</param>
    /// <returns>获取到的缓存数据，如果键不存在，不会引发异常</returns>
    Value this[Key key] { get; }
    #endregion
}
