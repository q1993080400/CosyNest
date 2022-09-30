using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

/// <summary>
/// 该对象可以用来适配一些自定义字典接口
/// </summary>
/// <typeparam name="Key">字典的键类型</typeparam>
/// <typeparam name="Value">字典的值类型</typeparam>
public sealed class DictionaryFit<Key, Value> :
    IDictionary<Key, Value>, IReadOnlyDictionary<Key, Value>,
    IAddOnlyDictionary<Key, Value>, IRestrictedDictionary<Key, Value>
    where Key : notnull
{
    #region 封装的字典
    /// <summary>
    /// 获取封装的字典，本对象的功能就是通过它实现的
    /// </summary>
    private IDictionary<Key, Value> Dictionary { get; }
    #endregion
    #region 关于添加和删除键值对
    #region 添加键值对
    public void Add(Key key, Value value)
        => Dictionary.Add(key, value);

    public void Add(KeyValuePair<Key, Value> item)
        => Dictionary.Add(item);
    #endregion
    #region 删除键值对
    public bool Remove(Key key)
        => Dictionary.Remove(key);

    public bool Remove(KeyValuePair<Key, Value> item)
        => Dictionary.Remove(item);
    #endregion
    #region 清空键值对
    public void Clear()
        => Dictionary.Clear();
    #endregion
    #endregion
    #region 关于访问和检查键值对
    #region 检查是否存在键值对
    public bool ContainsKey(Key key)
        => Dictionary.ContainsKey(key);

    public bool Contains(KeyValuePair<Key, Value> item)
        => Dictionary.Contains(item);
    #endregion
    #region 尝试获取值
    public bool TryGetValue(Key key, [MaybeNullWhen(false)] out Value value)
        => Dictionary.TryGetValue(key, out value);
    #endregion
    #region 通过键读写值
    public Value this[Key key]
    {
        get => Dictionary[key];
        set
        {
            if (!CanModify)
                throw new NotSupportedException("这个字典不允许修改键值对");
            Dictionary[key] = value;
        }
    }
    #endregion
    #endregion
    #region 关于集合本身
    #region 是否允许修改
    public bool IsReadOnly => Dictionary.IsReadOnly;

    public bool CanModify { get; }
    #endregion
    #region 键集合
    public ICollection<Key> Keys => Dictionary.Keys;

    IEnumerable<Key> IReadOnlyDictionary<Key, Value>.Keys => Keys;
    #endregion
    #region 值集合
    public ICollection<Value> Values => Dictionary.Values;

    IEnumerable<Value> IReadOnlyDictionary<Key, Value>.Values => Values;
    #endregion
    #region 枚举键值对
    public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
        => Dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 键值对数量
    public int Count => Dictionary.Count;
    #endregion
    #region 复制键值对
    public void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
        => Dictionary.CopyTo(array, arrayIndex);
    #endregion
    #endregion
    #region 转换为BCL自带的字典
    /// <summary>
    /// 将本字典转换为等效的BCL自带的字典
    /// </summary>
    /// <returns></returns>
    public Dictionary<Key, Value> ToBCLDictionary()
        => new(this);
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="dictionary">指定在底层使用的字典，本对象的功能就是通过它实现的，
    /// 如果为<see langword="null"/>，则使用<see cref="Dictionary{TKey, TValue}"/></param>
    /// <param name="canModify">指定是否可以修改已经存在的键值对，
    /// 注意：<see cref="IsReadOnly"/>为<see langword="true"/>时，它永远为<see langword="false"/></param>
    public DictionaryFit(IDictionary<Key, Value>? dictionary = null, bool canModify = true)
    {
        this.Dictionary = dictionary ?? new Dictionary<Key, Value>();
        this.CanModify = canModify && !IsReadOnly;
    }
    #endregion
}
