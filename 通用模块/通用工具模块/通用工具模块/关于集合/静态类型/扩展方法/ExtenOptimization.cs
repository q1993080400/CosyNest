namespace System.Linq;

public static partial class ExtenIEnumerable
{
    /*这个分部类专门声明为特定集合类型优化过的扩展方法，
      它们耦合更强，但是具有更高的性能，或者具有特定集合类型特有的功能*/

    #region 为数组优化
    #region 返回所有维度的长度
    /// <summary>
    /// 返回数组中所有维度的长度
    /// </summary>
    /// <param name="array">要返回所有维度长度的数组</param>
    /// <returns></returns>
    public static int[] GetLength(this Array array)
        => Enumerable.Range(0, array.Rank).Select(array.GetLength).ToArray();
    #endregion
    #region 将集合分页
    /// <summary>
    /// 将集合分页，并返回特定页的元素
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="objs">待分页的集合</param>
    /// <param name="pageIndex">页的索引，从0开始</param>
    /// <param name="pageSize">每一页的元素数量</param>
    /// <returns></returns>
    public static Obj[] Page<Obj>(this Obj[] objs, int pageIndex, int pageSize)
    {
        var maxIndex = (objs.Length + objs.Length % pageSize) / pageSize;
        var first = pageIndex * pageSize;
        return pageIndex switch
        {
            var i when i == maxIndex => objs[first..],
            var i when i >= 0 && i < maxIndex => objs[first..(first + pageSize)],
            _ => Array.Empty<Obj>(),
        };
    }
    #endregion
    #endregion
    #region 为ICollection<T>优化
    #region 合并ICollection<T>
    /// <summary>
    /// 取多个<see cref="ICollection{T}"/>的并集，
    /// 不会去除重复的元素
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collection">原始集合</param>
    /// <param name="collectionOther">要合并的其他集合</param>
    /// <returns></returns>
    public static Obj[] Union<Obj>(this ICollection<Obj> collection, params ICollection<Obj>[] collectionOther)
    {
        var len = collection.Count + collectionOther.Sum(x => x.Count);
        var newArray = new Obj[len];
        var pos = 0;
        foreach (var item in collectionOther.Prepend(collection))
        {
            item.CopyTo(newArray, pos);
            pos += item.Count;
        }
        return newArray;
    }
    #endregion
    #endregion
    #region 为IList<T>优化
    #region 比较IList<T>的值相等性
    /// <summary>
    /// 比较<see cref="IList{T}"/>的值相等性
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collectionA">要比较的第一个集合</param>
    /// <param name="collectionB">要比较的第二个集合</param>
    /// <returns>如果两个集合的长度和对应索引的元素完全一致，
    /// 则返回<see langword="true"/>，否则返回<see langword="false"/></returns>
    public static bool SequenceEqual<Obj>(this IList<Obj> collectionA, IList<Obj> collectionB)
    {
        var len = collectionA.Count;
        if (len != collectionB.Count)
            return false;
        for (int i = 0; i < len; i++)
        {
            if (!Equals(collectionA[i], collectionB[i]))
                return false;
        }
        return true;
    }
    #endregion
    #endregion
    #region 为Queue<T>优化
    #region 在末尾添加元素
    /// <summary>
    /// 在队列的末尾添加元素，
    /// 如果队列的元素数量大于等于<paramref name="maxCount"/>，
    /// 则还会首先在开头移除元素
    /// </summary>
    /// <typeparam name="Obj">队列的元素类型</typeparam>
    /// <param name="queue">要操作的队列</param>
    /// <param name="obj">要添加的元素</param>
    /// <param name="maxCount">最大元素数量</param>
    public static void Enqueue<Obj>(this Queue<Obj> queue, Obj obj, int maxCount)
    {
        if (queue.Count >= maxCount)
            queue.Dequeue();
        queue.Enqueue(obj);
    }
    #endregion
    #endregion
    #region 为ILookup优化
    #region 尝试获取元素
    /// <summary>
    /// 尝试获取一个<see cref="ILookup{TKey, TElement}"/>的元素，
    /// 如果键不存在，不会引发异常
    /// </summary>
    /// <param name="lookup">要获取元素的键值对集合</param>
    /// <param name="key">用来获取元素的键</param>
    /// <returns>一个元组，它的项分别是是否存在元素，以及找到的元素集合，
    /// 如果不存在元素，返回空集合</returns>
    /// <inheritdoc cref="ILookup{TKey, TElement}"/>
    public static (bool Exist, IEnumerable<TElement> Value) TryGetValue<TKey, TElement>(this ILookup<TKey, TElement> lookup, TKey key)
        where TKey : notnull
    {
        if (lookup.Contains(key))
        {
            return (true, lookup[key]);
        }
        return (false, Array.Empty<TElement>());
    }
    #endregion
    #endregion
    #region 为IEnumerator优化
    #region 批量枚举元素
    #region 同步版本
    /// <summary>
    /// 批量枚举一个枚举器的元素
    /// </summary>
    /// <typeparam name="Obj">枚举器的元素类型</typeparam>
    /// <param name="enumerator">待批量枚举的枚举器</param>
    /// <param name="count">要枚举的元素数量</param>
    /// <returns>一个元组，它的元素分别是批量枚举到的元素，以及是否已经枚举到末尾</returns>
    public static (IEnumerable<Obj> Element, bool ToEnd) MoveRange<Obj>(this IEnumerator<Obj> enumerator, int count)
    {
        var list = new LinkedList<Obj>();
        for (int i = 0; i < count; i++)
        {
            if (!enumerator.MoveNext())
                return (list, true);
            list.Add(enumerator.Current);
        }
        return (list, false);
    }
    #endregion
    #region 异步版本
    /// <inheritdoc cref="MoveRange{Obj}(IEnumerator{Obj}, int)"/>
    public static async Task<(IEnumerable<Obj> Element, bool ToEnd)> MoveRange<Obj>(this IAsyncEnumerator<Obj> enumerator, int count)
    {
        var list = new LinkedList<Obj>();
        for (int i = 0; i < count; i++)
        {
            if (!await enumerator.MoveNextAsync())
                return (list, true);
            list.Add(enumerator.Current);
        }
        return (list, false);
    }
    #endregion
    #endregion
    #endregion
}
