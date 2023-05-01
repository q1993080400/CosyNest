using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Collections.Generic;

/// <summary>
/// 该字典通过反射属性来读写值
/// </summary>
sealed class ReflectionDictionary : IRestrictedDictionary<string, object?>
{
    #region 有关反射的对象
    #region 反射目标
    /// <summary>
    /// 获取反射的目标
    /// </summary>
    private object? Target { get; }
    #endregion
    #region 反射字典
    /// <summary>
    /// 该字典通过名称索引属性对象
    /// </summary>
    private IReadOnlyDictionary<string, PropertyInfo> Properties { get; }
    #endregion
    #endregion
    #region 有关键值对集合
    #region 枚举键值对
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => Properties.Select(x => (x.Key, x.Value.GetValue(Target))).ToKV().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 枚举键集合
    public IEnumerable<string> Keys
        => this.Select(x => x.Key);
    #endregion
    #region 枚举值集合
    public IEnumerable<object?> Values
        => this.Select(x => x.Value);
    #endregion
    #region 键值对数量
    public int Count
        => Properties.Count;
    #endregion
    #endregion
    #region 有关读写和检查键值对
    #region 检查键是否存在
    public bool ContainsKey(string key)
        => Properties.ContainsKey(key);
    #endregion
    #region 尝试获取值
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
    {
        if (Properties.TryGetValue(key, out var pro))
        {
            value = pro.GetValue(Target);
            return true;
        }
        value = null;
        return false;
    }
    #endregion
    #region 读写值
    public object? this[string key]
    {
        get => Properties[key].GetValue(Target);
        set => Properties[key].SetValue(Target, value);
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="target">属性所依附的对象</param>
    /// <param name="properties">要读写值的属性</param>
    /// <returns></returns>
    public ReflectionDictionary(object? target, IEnumerable<PropertyInfo> properties)
    {
        Target = target;
        Properties = properties.ToDictionary(x => (x.Name, x), true);
    }
    #endregion
}
