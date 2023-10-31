using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.Design.Direct;

/// <summary>
/// 本类型是<see cref="IDirect"/>的简易实现，
/// 它直接封装了一个字典
/// </summary>
sealed class DirectSimple : IDirect
{
    #region 封装的字典
    /// <summary>
    /// 获取封装的字典，
    /// 本类型的功能就是通过它实现的
    /// </summary>
    private Dictionary<string, object?> Dictionary { get; } = [];
    #endregion
    #region 接口实现
    #region 复制字典
    public IDirect Copy(bool copyValue = true, Type? type = null)
    {
        if (type is { })
            throw new NotSupportedException($"不允许指定数据副本的类型,{nameof(type)}必须为null");
        var copy = new DirectSimple();
        if (copyValue)
            foreach (var (k, v) in this)
            {
                copy[k] = v;
            }
        return copy;
    }
    #endregion
    #region 读写属性
    public object? this[string key]
    {
        get => Dictionary[key];
        set => Dictionary[key] = value;
    }
    #endregion
    #region 是否存在键
    public bool ContainsKey(string key)
        => Dictionary.ContainsKey(key);
    #endregion
    #region 尝试获取键
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
        => Dictionary.TryGetValue(key, out value);
    #endregion
    #region 键集合
    public IEnumerable<string> Keys => Dictionary.Keys;
    #endregion
    #region 值集合
    public IEnumerable<object?> Values => Dictionary.Values;
    #endregion
    #region 键值对数量
    public int Count => Dictionary.Count;
    #endregion
    #region 枚举键值对
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => Dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    #endregion
    #region 返回Json字符串
    public string Json => throw new NotImplementedException();
    #endregion
    #endregion
}
