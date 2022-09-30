using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

/// <summary>
/// 表示一个单例的，没有任何字段，也不能容纳任何元素的只读字典，
/// 它专门用于低成本地提供<see cref="IReadOnlyDictionary{TKey, TValue}"/>的默认值
/// </summary>
/// <typeparam name="Key">字典的键类型</typeparam>
/// <typeparam name="Value">字典的值类型</typeparam>
sealed class ReadOnlyDictionaryEmpty<Key, Value> : IReadOnlyDictionary<Key, Value>
    where Key : notnull
{
    #region 检查是否存在指定的键
    public bool ContainsKey(Key key)
        => false;
    #endregion
    #region 通过键获取值，不引发异常
    public bool TryGetValue(Key key, [MaybeNullWhen(false)] out Value value)
    {
        value = default;
        return false;
    }
    #endregion
    #region 关于键值对集合
    #region 通过键获取值
    public Value this[Key key]
        => throw new KeyNotFoundException($"本字典的唯一目的是为{nameof(IReadOnlyDictionary<Key, Value>)}提供默认值，" +
            $"它不存在也不能添加任何元素");
    #endregion
    #region 枚举键
    public IEnumerable<Key> Keys => Array.Empty<Key>();
    #endregion
    #region 枚举值
    public IEnumerable<Value> Values => Array.Empty<Value>();
    #endregion
    #region 获取键值对数量
    public int Count => 0;
    #endregion
    #region 枚举键值对
    public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
    {
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #endregion
}
