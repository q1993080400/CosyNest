namespace System.Linq;

public static partial class ExtendEnumerable
{
    /*所有有关字典的扩展方法，全部放在这个部分类中*/

    #region 安全索引
    #region 如果不存在键，获取值并将其添加到字典中
    /// <summary>
    /// 根据键在字典中查找值，如果键不存在，
    /// 则通过委托获取一个值，并将其添加到字典中
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    /// <param name="dictionary">需要进行判断的字典</param>
    /// <param name="key">指定的键</param>
    /// <param name="newValue">如果键不存在，则通过这个委托获取值，并将其添加到字典中，委托的参数就是刚才的键</param>
    /// <returns>返回值是一个元组，第一个项是键是否存在，第二个项是值</returns>
    public static (bool Exist, Value Value) TrySetValue<Key, Value>(this IDictionary<Key, Value> dictionary, Key key, Func<Key, Value> newValue)
        where Key : notnull
    {
        if (dictionary.TryGetValue(key, out var values))
            return (true, values);
        var @new = newValue(key);
        dictionary.Add(key, @new);
        return (false, @new);
    }
    #endregion
    #region 如果不存在键，返回指定值
    /// <summary>
    /// 根据键在字典中查找值，并返回一个元组，
    /// 第一个元素指示键是否存在，如果存在，第二个元素是找到的值，
    /// 如果不存在，第二个元素是一个指定的默认值
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    /// <param name="dictionary">这个参数必须是一个只读字典或可变字典</param>
    /// <param name="key">用来提取值的键，如果它为<see langword="null"/>，则默认该键不存在</param>
    /// <param name="noFound">如果键不存在，则通过这个延迟对象返回默认值</param>
    /// <returns></returns>
    public static (bool Exist, Value? Value) TryGetValue<Key, Value>(this IEnumerable<KeyValuePair<Key, Value>> dictionary, Key key, Lazy<Value>? noFound = default)
        => dictionary switch
        {
            IDictionary<Key, Value> d => d.TryGetValue(key, out Value? value) ? (true, value) : (false, noFound.Value()),
            IReadOnlyDictionary<Key, Value> d => d.TryGetValue(key, out Value? v) ? (true, v) : (false, noFound.Value()),
            _ => throw new NotSupportedException($"{dictionary.GetType()}既不是一个字典，也不是一个只读字典"),
        };
    #endregion
    #endregion
}
