namespace System.Linq;

public static partial class ExtendEnumerable
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
            list.AddLast(enumerator.Current);
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
            list.AddLast(enumerator.Current);
        }
        return (list, false);
    }
    #endregion
    #endregion
    #endregion
}
