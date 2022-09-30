namespace System.Linq;

public static partial class ExtenIEnumerable
{
    /*所有有关字典的扩展方法，全部放在这个部分类中*/

    #region 关于索引器
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
    public static (bool Exist, Value? Value) TryGetValue<Key, Value>(this IEnumerable<KeyValuePair<Key, Value>> dictionary, Key key, LazyPro<Value>? noFound = default)
    {
        if (dictionary is IDictionary<Key, Value> or IReadOnlyDictionary<Key, Value>)
        {
            return dictionary.To<dynamic>().TryGetValue(key, out Value value) ?
             (true, value) : (false, noFound);
        }
        throw new TypeUnlawfulException(dictionary, typeof(IDictionary<Key, Value>), typeof(IReadOnlyDictionary<Key, Value>));
    }
    #endregion
    #endregion
    #endregion
    #region 关于字典转换
    #region 适配字典
    /// <summary>
    /// 将字典适配为<see cref="DictionaryFit{Key, Value}"/>
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    /// <param name="dictionary">要适配的字典</param>
    /// <param name="canModify">指定是否可以修改已经存在的键值对，
    /// 注意：<see cref="ICollection{T}.IsReadOnly"/>为<see langword="true"/>时，它永远为<see langword="false"/></param>
    /// <returns></returns>
    public static DictionaryFit<Key, Value> FitDictionary<Key, Value>(this IDictionary<Key, Value> dictionary, bool canModify = true)
        where Key : notnull
        => new(dictionary, canModify);
    #endregion
    #region 将集合转换为字典
    #region 将任意集合转换为字典
    /// <summary>
    /// 从集合中提取键和值，并将集合转换为字典
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collection">待转换的集合</param>
    /// <param name="delegate">这个委托传入集合的元素，并返回一个元组，它的项分别是字典的键和值</param>
    /// <param name="checkRepetition">如果这个值为<see langword="true"/>，在向字典添加重复键的时候会报错，否则会覆盖旧有键的值</param>
    /// <returns></returns>
    public static DictionaryFit<Key, Value> ToDictionary<Key, Value, Obj>(this IEnumerable<Obj> collection, Func<Obj, (Key, Value)> @delegate, bool checkRepetition)
        where Key : notnull
    {
        var dict = new DictionaryFit<Key, Value>();
        foreach (var item in collection)
        {
            var (k, v) = @delegate(item);
            if (checkRepetition)
                dict.Add(k, v);
            else dict[k] = v;
        }
        return dict;
    }
    #endregion
    #region 将键值对集合直接转换为字典
    /// <summary>
    /// 将键值对集合直接转换为字典
    /// </summary>
    /// <typeparam name="Key">键的类型</typeparam>
    /// <typeparam name="Value">值的类型</typeparam>
    /// <param name="collection">要转换为字典的键值对集合</param>
    /// <param name="checkRepetition">如果这个值为<see langword="true"/>，在向字典添加重复键的时候会报错，否则会覆盖旧有键的值</param>
    /// <returns></returns>
    public static DictionaryFit<Key, Value> ToDictionary<Key, Value>(this IEnumerable<KeyValuePair<Key, Value>> collection, bool checkRepetition)
        where Key : notnull
        => collection.ToDictionary(x => (x.Key, x.Value), checkRepetition);
    #endregion
    #region 将元组集合直接转换为字典
    /// <summary>
    /// 将一个元组集合直接转换为字典
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    /// <param name="collection">待转换的元组集合</param>
    /// <param name="checkRepetition">如果这个值为<see langword="true"/>，在向字典添加重复键的时候会报错，否则会覆盖旧有键的值</param>
    /// <returns></returns>
    public static DictionaryFit<Key, Value> ToDictionary<Key, Value>(this IEnumerable<(Key, Value)> collection, bool checkRepetition)
        where Key : notnull
        => collection.ToDictionary(x => (x.Item1, x.Item2), checkRepetition);
    #endregion
    #endregion
    #endregion
    #region 关于键值对
    #region 将元组转换为键值对
    /// <summary>
    /// 将一个二元组转换为键值对
    /// </summary>
    /// <typeparam name="Key">键值对的键类型</typeparam>
    /// <typeparam name="Value">键值对的值类型</typeparam>
    /// <param name="tupts">待转换的元组</param>
    /// <returns></returns>
    public static KeyValuePair<Key, Value> ToKV<Key, Value>(this (Key, Value) tupts)
        where Key : notnull
        => new(tupts.Item1, tupts.Item2);
    #endregion
    #region 批量转换元组为键值对
    /// <summary>
    /// 将二元组集合转换为键值对集合
    /// </summary>
    /// <typeparam name="Key">键值对的键类型</typeparam>
    /// <typeparam name="Value">键值对的值类型</typeparam>
    /// <param name="collection">待转换的二元组集合</param>
    /// <returns></returns>
    public static IEnumerable<KeyValuePair<Key, Value>> ToKV<Key, Value>(this IEnumerable<(Key, Value)> collection)
        where Key : notnull
        => collection.Select(ToKV);
    #endregion
    #region 批量解构键值对
    /// <summary>
    /// 批量将键值对解构为元组
    /// </summary>
    /// <typeparam name="Key">键值对的键类型</typeparam>
    /// <typeparam name="Value">键值对的值类型</typeparam>
    /// <param name="collection">要解构的键值对集合</param>
    /// <returns></returns>
    public static IEnumerable<(Key Key, Value Value)> ToTupts<Key, Value>(this IEnumerable<KeyValuePair<Key, Value>> collection)
        => collection.Select(x => (x.Key, x.Value));
    #endregion
    #endregion
}
