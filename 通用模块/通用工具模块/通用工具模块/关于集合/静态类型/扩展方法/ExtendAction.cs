using System.Collections;

namespace System.Linq;

/// <summary>
/// 所有关于集合的扩展方法全部放在这个类中，通常无需专门调用
/// </summary>
public static partial class ExtendEnumerable
{
    /*所有关于集合的操作的方法，全部放在这个部分类中，
      操作指的是：API不返回任何值，或者改变了集合的状态*/

    #region 如果集合不存在任何元素，则抛出异常
    /// <summary>
    /// 如果集合为<see langword="null"/>或不存在任何元素，则抛出一个异常，
    /// 否则直接返回这个集合本身
    /// </summary>
    /// <param name="collections">待检查的集合</param>
    /// <param name="describe">对集合的描述，
    /// 这个参数可以更清楚地告诉调试者，这个集合有什么作用</param>
    public static Collections AnyCheck<Collections>(this Collections? collections, string? describe = null)
        where Collections : IEnumerable
    {
        ArgumentNullException.ThrowIfNull(collections);
        foreach (var _ in collections)
        {
            return collections;
        }
        throw new ArgumentException(describe + "集合没有任何元素");
    }
    #endregion
    #region 关于遍历
    #region 遍历泛型集合
    /// <summary>
    /// 遍历一个泛型集合
    /// </summary>
    /// <typeparam name="Obj">要遍历的元素类型</typeparam>
    /// <param name="collections">要遍历的泛型集合</param>
    /// <param name="delegate">对每个元素执行的委托</param>
    public static void ForEach<Obj>(this IEnumerable<Obj> collections, Action<Obj> @delegate)
    {
        foreach (Obj i in collections)
            @delegate(i);
    }
    #endregion
    #region 拆分遍历
    /// <summary>
    /// 分别遍历一个迭代器的第一个元素和其他元素
    /// </summary>
    /// <typeparam name="Obj">迭代器的元素类型</typeparam>
    /// <param name="collections">待遍历的迭代器</param>
    /// <param name="first">用来遍历第一个元素的委托，
    /// 它的第一个参数是待遍历的元素，第二个参数就是<paramref name="other"/>的柯里化形式，参数传入第一个元素</param>
    /// <param name="other">用来遍历其他元素的委托</param>
    public static void ForEachSplit<Obj>(this IEnumerable<Obj> collections, Action<Obj, Action> first, Action<Obj> other)
    {
        var (first2, other2, hasElements) = collections.First(false);
        if (hasElements)
        {
            first(first2, () => other(first2));
            other2.ForEach(other);
        }
    }
    #endregion
    #region 遍历一个二维数组
    /// <summary>
    /// 遍历一个二维数组
    /// </summary>
    /// <typeparam name="Obj">二维数组的元素类型</typeparam>
    /// <param name="array">要遍历的二维数组</param>
    /// <param name="delegate">遍历二维数组的委托，
    /// 它的参数分别是数组的列号，行号，以及数组对应的元素</param>
    /// <exception cref="AggregateException"><paramref name="array"/>不是二维数组</exception>
    public static void ForEach<Obj>(this Obj[,] array, Action<int, int, Obj> @delegate)
    {
        var len = array.GetLength();
        if (len.Length is not 2)
            throw new AggregateException($"仅支持遍历二维数组，但是的数组维度为{len.Length}");
        for (int col = 0, maxCol = len[0]; col < maxCol; col++)
        {
            for (int row = 0, maxRow = len[1]; row < maxRow; row++)
            {
                @delegate(col, row, array[col, row]);
            }
        }
    }
    #endregion
    #endregion
    #region 关于添加或删除元素
    #region 关于添加元素
    #region 批量添加元素
    /// <summary>
    /// 向一个集合中批量添加元素，由于C#7.2的新语法，
    /// 这个方法可以让集合能够直接用另一个集合进行初始化
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">要添加元素的集合</param>
    /// <param name="objs">被添加进集合的元素</param>
    public static void Add<Obj>(this ICollection<Obj> collections, IEnumerable<Obj> objs)
        => objs.ForEach(collections.Add);

    /*“可以用集合初始化集合”指的是：
        new List<int>()
        {
            new List<int>{1}
        };
    */
    #endregion
    #endregion
    #endregion
    #region 替换元素
    /// <summary>
    /// 替换一个集合中的元素，
    /// 如果存在多个元素，则替换第一个元素
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="objs">要替换的集合</param>
    /// <param name="old">旧元素</param>
    /// <param name="new">新元素</param>
    /// <returns>是否替换成功</returns>
    public static bool Replace<Obj>(this IList<Obj> objs, Obj old, Obj @new)
    {
        var index = objs.IndexOf(old);
        if (index < 0)
            return false;
        objs.RemoveAt(index);
        objs.Insert(index, @new);
        return true;
    }
    #endregion
}
