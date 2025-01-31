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
    #region 批量删除元素
    /// <summary>
    /// 批量删除元素
    /// </summary>
    /// <typeparam name="Obj">要批量删除元素的集合的元素类型</typeparam>
    /// <param name="list">要批量删除元素的集合</param>
    /// <param name="predicate">用来判断是否删除某元素的委托</param>
    public static void RemoveAll<Obj>(this IList<Obj> list, Func<Obj, bool> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i]))
            {
                list.RemoveAt(i--);
            }
        }
    }
    #endregion
    #region 替换元素
    #region 按照条件替换
    /// <summary>
    /// 替换集合内第一个符合条件的元素，
    /// 并返回是否替换成功
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="list">待替换的集合</param>
    /// <param name="test">用来测试集合中的元素是否符合要求的委托</param>
    /// <param name="newElement">要替代旧元素的新元素</param>
    /// <returns></returns>
    public static bool Replace<Obj>(this IList<Obj> list, Func<Obj, bool> test, Obj newElement)
    {
        if (list.Count is 0)
            return false;
        var index = list.IndexOf(test);
        if (index < 0)
            return false;
        list.RemoveAt(index);
        list.Insert(index, newElement);
        return true;
    }
    #endregion
    #region 按照元素替换
    /// <summary>
    /// 替换集合内的元素，并返回是否替换成功，
    /// 如果有多个相同的元素，则只替换第一个
    /// </summary>
    /// <param name="oldElement">要替换的旧元素</param>
    /// <returns></returns>
    /// <inheritdoc cref="Replace{Obj}(IList{Obj}, Func{Obj, bool}, Obj)"/>
    public static bool Replace<Obj>(this IList<Obj> list, Obj oldElement, Obj newElement)
        => list.Replace(x => Equals(x, oldElement), newElement);
    #endregion 
    #endregion
}
