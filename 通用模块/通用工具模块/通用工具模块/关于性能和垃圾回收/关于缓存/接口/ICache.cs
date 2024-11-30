using System.Diagnostics.CodeAnalysis;

namespace System.Performance;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以被视为一个缓存
/// </summary>
/// <typeparam name="Key">用来提取缓存的键类型</typeparam>
/// <typeparam name="Value">被缓存的类型</typeparam>
public interface ICache<Key, Value>
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
    #region 尝试获取元素
    /// <summary>
    /// 尝试获取元素，并返回是否找到了元素
    /// </summary>
    /// <param name="key">元素的键</param>
    /// <param name="value">如果找到了元素，
    /// 元素的值会被初始化为这个变量</param>
    /// <returns></returns>
    bool TryGetValue(Key key, [NotNullWhen(true)] out Value? value);
    #endregion
    #region 显式设置元素
    /// <summary>
    /// 显式设置元素
    /// </summary>
    /// <param name="key">元素的键</param>
    /// <param name="value">元素的值</param>
    void SetValue(Key key, Value value);
    #endregion
}
