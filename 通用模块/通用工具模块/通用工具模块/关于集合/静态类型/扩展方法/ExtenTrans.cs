using System.Collections;
using System.Maths;

namespace System.Linq;

public static partial class ExtenIEnumerable
{
    /*所有关于集合的转化的方法，全部放在这个部分类中，
      集合的转化指的是：API返回一个新的集合*/

    #region 关于排序
    #region 不指定比较器
    /// <summary>
    /// 对一个泛型集合进行排序，
    /// 由于该集合的元素自身就是<see cref="IComparable{T}"/>，
    /// 因此它不需要指定比较器
    /// </summary>
    /// <inheritdoc cref="Sort{Obj}(IEnumerable{Obj}, IComparer{Obj}, bool)"/>
    public static Obj[] Sort<Obj>(this IEnumerable<Obj> collections, bool isAscending = true)
        where Obj : IComparable<Obj>
        => (isAscending ? collections.Order() : collections.OrderDescending()).ToArray();
    #endregion
    #region 指定比较器
    /// <summary>
    /// 通过指定比较器，对一个泛型集合进行排序
    /// </summary>
    /// <typeparam name="Obj">泛型集合的类型</typeparam>
    /// <param name="collections">要排序的泛型集合</param>
    /// <param name="comparable">用于比较的比较器，
    /// 如果为<see langword="null"/>，则使用默认比较器</param>
    /// <param name="isAscending">如果这个值为<see langword="true"/>，则升序排列，为<see langword="false"/>，则降序排列</param>
    /// <returns></returns>
    public static Obj[] Sort<Obj>(this IEnumerable<Obj> collections, IComparer<Obj> comparable, bool isAscending = true)
        => collections.Sort(x => x, isAscending, comparable);
    #endregion
    #region 指定键和比较器
    /// <inheritdoc cref="SortMultiple{Obj, Key}(IEnumerable{Obj}, Func{Obj, Key}, bool, Func{Obj, object}[])"/>
    /// <inheritdoc cref="Sort{Obj}(IEnumerable{Obj}, IComparer{Obj}?, bool)"/>
    public static Obj[] Sort<Obj, Key>(this IEnumerable<Obj> collections, Func<Obj, Key> @delegate, bool isAscending = true, IComparer<Key>? comparable = null)
        => isAscending ?
        collections.OrderBy(@delegate, comparable).ToArray() :
        collections.OrderByDescending(@delegate, comparable).ToArray();
    #endregion
    #region 指定多个键
    /// <summary>
    /// 通过指定键，对一个泛型集合进行排序
    /// </summary>
    /// <typeparam name="Key">提取的键类型</typeparam>
    /// <param name="delegate">从集合中提取，用于排序的键的委托</param>
    /// <param name="secondary">这个数组可通过委托获取次要排序条件，前面的优先级更高</param>
    /// <inheritdoc cref="Sort{Obj}"/>
    /// <returns></returns>
    public static Obj[] SortMultiple<Obj, Key>(this IEnumerable<Obj> collections, Func<Obj, Key> @delegate, bool isAscending = true, params Func<Obj, object>[] secondary)
    {
        var list = isAscending ? collections.OrderBy(@delegate) : collections.OrderByDescending(@delegate);
        foreach (var item in secondary)
        {
            list = isAscending ? list.ThenBy(item) : list.ThenByDescending(item);
        }
        return list.ToArray();
    }
    #endregion
    #region 将集合洗牌
    /// <summary>
    /// 打乱一个集合的顺序，并返回一个新的集合
    /// </summary>
    /// <typeparam name="RetList">集合中的元素类型</typeparam>
    /// <param name="collections">要打乱顺序的集合</param>
    /// <param name="rand">用来生成随机数的对象，如果为<see langword="null"/>，则使用一个默认对象</param>
    /// <returns></returns>
    public static RetList[] SortRand<RetList>(this IEnumerable<RetList> collections, IRandom? rand = null)
    {
        rand ??= CreateBaseMath.RandomShared;
        return collections.Select(x => (x, rand.Rand())).ToArray().
            OrderBy(x => x.Item2).Select(x => x.x).ToArray();
    }
    #endregion
    #endregion
    #region 关于并集
    #region 生成并返回多个集合的并集
    #region 以多个元素为参数
    /// <summary>
    /// 返回一个集合和多个元素的并集
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">要形成并集的集合</param>
    /// <param name="distinct">如果这个值为<see langword="true"/>，还会去除重复的元素</param>
    /// <param name="elements">要合并的元素</param>
    /// <returns></returns>
    public static IEnumerable<Obj> Union<Obj>(this IEnumerable<Obj> collections, bool distinct, params Obj[] elements)
        => collections.Union(distinct, new[] { elements });
    #endregion
    #region 以多个集合为参数
    /// <summary>
    /// 生成并返回多个集合的并集
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">第一个集合</param>
    /// <param name="distinct">如果这个值为<see langword="true"/>，还会去除重复的元素</param>
    /// <param name="other">要合并的其他集合，可以是一个，也可以是多个</param>
    /// <returns></returns>
    public static IEnumerable<Obj> Union<Obj>(this IEnumerable<Obj> collections, bool distinct, params IEnumerable<Obj>[] other)
    {
        #region 本地函数
        IEnumerable<Obj> Fun()
        {
            foreach (var i in collections)
                yield return i;
            foreach (var list in other)
                foreach (var i in list)
                    yield return i;
        }
        #endregion
        return distinct ? Fun().Distinct() : Fun();
    }
    #endregion
    #region 以一个嵌套集合作为参数
    /// <summary>
    /// 生成并返回一个嵌套集合中所有子集合的并集，
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">一个包含所有要合并元素的嵌套集合</param>
    /// <param name="distinct">如果这个值为<see langword="true"/>，还会去除重复的元素</param>
    /// <returns></returns>
    public static IEnumerable<Obj> UnionNesting<Obj>(this IEnumerable<IEnumerable<Obj>> collections, bool distinct = false)
    {
        #region 本地函数
        IEnumerable<Obj> Fun()
        {
            foreach (var list in collections)
                foreach (var i in list)
                    yield return i;
        }
        #endregion
        return distinct ? Fun().Distinct() : Fun();
    }
    #endregion
    #endregion
    #endregion
    #region 关于拆分集合
    #region 简单拆分
    /// <summary>
    /// 将一个集合拆分，并返回拆分后的集合
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collections">要拆分的集合</param>
    /// <param name="size">指示集合拆分的规模，它的含义由<paramref name="numIsCount"/>决定</param>
    /// <param name="numIsCount">如果这个值为<see langword="true"/>，则<paramref name="size"/>参数指的是每个子集合最多有多少个元素，
    /// 如果这个值为<see langword="false"/>，则<paramref name="size"/>参数指的是应该将父集合拆分成多少个子集合</param>
    /// <returns></returns>
    public static IEnumerable<Obj[]> Chunk<Obj>(this IEnumerable<Obj> collections, int size, bool numIsCount)
    {
        ExceptionIntervalOut.Check(1, null, size);
        var arry = collections.ToArray();
        var len = arry.Length;
        var count = numIsCount ? size : (len + len % size) / size;
        return arry.Chunk(count);
    }
    #endregion
    #region 按索引拆分
    /// <summary>
    /// 按照指定的索引拆分集合
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collections">待拆分的集合</param>
    /// <param name="index">这些索引指示拆分集合的位置</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<Obj>> Split<Obj>(this IEnumerable<Obj> collections, params int[] index)
    {
        using var enumerator = collections.GetEnumerator();
        var list = new LinkedList<Obj>();
        for (int i = 0, arryIndex = 0; true; i++)
        {
            var notObj = !enumerator.MoveNext();
            if (notObj || i == index[arryIndex])
            {
                yield return list;
                if (notObj)
                    yield break;
                list = new LinkedList<Obj>();
                arryIndex = Math.Min(index.Length - 1, arryIndex + 1);
            }
            list.AddLast(enumerator.Current);
        }
    }

    /*说明文档：
      假设有集合{1，2，3，4，5}，
      Index参数填入{1，3}，
      那么函数会返回3个集合，分别是
      {1}
      {2,3}
      {4,5}*/
    #endregion
    #region 按条件拆分
    /// <summary>
    /// 按照条件，将一个集合拆分成两部分，
    /// 分别是满足条件和不满足条件的部分
    /// </summary>
    /// <typeparam name="Obj">要拆分的集合元素类型</typeparam>
    /// <param name="collections">要拆分的集合</param>
    /// <param name="delegate">对集合元素进行判断的委托</param>
    /// <returns></returns>
    public static (IEnumerable<Obj> IsTrue, IEnumerable<Obj> IsFalse) Split<Obj>(this IEnumerable<Obj> collections, Func<Obj, bool> @delegate)
    {
        var (isTrue, isFalse) = (new LinkedList<Obj>(), new LinkedList<Obj>());
        foreach (var i in collections)
        {
            var list = @delegate(i) ? isTrue : isFalse;
            list.AddLast(i);
        }
        return (isTrue, isFalse);
    }
    #endregion
    #endregion
    #region 关于Zip
    #region 合并两个集合
    /// <summary>
    /// 合并两个集合的对应元素，与<see cref="Enumerable.Zip{TFirst, TSecond, TResult}(IEnumerable{TFirst}, IEnumerable{TSecond}, Func{TFirst, TSecond, TResult})"/>不同的是，
    /// 如果两个集合的元素数量不相等，
    /// 则会用一个默认值来填补缺失的元素
    /// </summary>
    /// <typeparam name="ObjA">第一个集合的元素类型</typeparam>
    /// <typeparam name="ObjB">第二个集合的元素类型</typeparam>
    /// <typeparam name="Ret">结果集合的元素类型</typeparam>
    /// <param name="collectionsA">要合并的第一个集合</param>
    /// <param name="collectionsB">要合并的第二个集合</param>
    /// <param name="delegate">这个委托传入两个集合的元素，并返回结果</param>
    /// <param name="delegateA">如果第一个集合的元素数量比另一个集合小，则缺失的元素通过这个委托生成</param>
    /// <param name="delegateB">如果第二个集合的元素数量比另一个集合小，则缺失的元素通过这个委托生成</param>
    /// <returns></returns>
    public static IEnumerable<Ret> ZipFill<ObjA, ObjB, Ret>(this IEnumerable<ObjA> collectionsA, IEnumerable<ObjB> collectionsB,
        Func<ObjA?, ObjB?, Ret> @delegate, Func<ObjA>? delegateA = null, Func<ObjB>? delegateB = null)
    {
        #region 本地函数
        IEnumerable<Ret> Fun(IEnumerable<ObjA?> a, IEnumerable<ObjB?> b)
            => a!.Zip(b!, @delegate);
        #endregion
        var (ac, bc) = (collectionsA.Count(), collectionsB.Count());
        if (ac == bc)
            return Fun(collectionsA, collectionsB);
        return ac < bc ?
        Fun(collectionsA.Fill(bc, delegateA), collectionsB) :
        Fun(collectionsA, collectionsB.Fill(ac, delegateB));

    }
    #endregion
    #region 将两个集合合并为元组
    /// <summary>
    /// 将两个集合的元素合并为元组
    /// </summary>
    /// <typeparam name="ObjA">第一个集合的元素类型</typeparam>
    /// <typeparam name="ObjB">第二个集合的元素类型</typeparam>
    /// <param name="collectionsA">要合并的第一个集合</param>
    /// <param name="collectionsB">要合并的第二个集合</param>
    /// <param name="truncated">在两个集合元素数量不相等的情况下，
    /// 如果这个值为<see langword="true"/>，则丢弃多余的元素，为<see langword="false"/>，则将不足的元素用默认值填补</param>
    /// <returns></returns>
    public static IEnumerable<(ObjA? A, ObjB? B)> Zip<ObjA, ObjB>(this IEnumerable<ObjA> collectionsA, IEnumerable<ObjB> collectionsB, bool truncated)
    {
        static (ObjA?, ObjB?) Fun(ObjA? x, ObjB? y)
            => (x, y);
        return truncated ? collectionsA.Zip(collectionsB, Fun) : collectionsA.ZipFill(collectionsB, Fun);
    }
    #endregion
    #endregion
    #region 填补集合的元素
    /// <summary>
    /// 将一个集合的元素填补至指定的数量，
    /// 如果该集合的元素数量大于应返回的元素数量，
    /// 则只返回集合前面的元素
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">待填补的集合</param>
    /// <param name="count">填补后的集合应有的元素数量</param>
    /// <param name="newElements">如果元素需要填补，则通过这个委托获取新元素，
    /// 如果为<see langword="null"/>，则返回默认值</param>
    /// <returns></returns>
    public static IEnumerable<Obj?> Fill<Obj>(this IEnumerable<Obj> collections, int count, Func<Obj>? newElements = null)
    {
        foreach (var e in collections)
        {
            if (count-- > 0)
                yield return e;
            else break;
        }
        while (count-- > 0)
            yield return newElements is null ? default! : newElements();
    }
    #endregion
    #region 将非泛型集合转化为泛型集合
    #region 转换为泛型迭代器
    /// <summary>
    /// 将非泛型迭代器转换为类型参数为<see cref="object"/>的泛型迭代器
    /// </summary>
    /// <param name="collections">待转换的非泛型迭代器</param>
    /// <returns></returns>
    public static IEnumerable<object> OfType(this IEnumerable collections)
    {
        foreach (var item in collections)
        {
            yield return item;
        }
    }

    /*问：此处为什么不使用OfType<object>？
      答：OfType这个函数需要小心使用，因为它会过滤掉null值，
      这不是本函数的本意*/
    #endregion
    #region 将转换为List
    /// <summary>
    /// 将任意集合转换为<see cref="List{T}"/>
    /// </summary>
    /// <typeparam name="Obj"><see cref="List{T}"/>元素的类型</typeparam>
    /// <param name="collections">要转换的集合，可以是最低级的<see cref="IEnumerable"/></param>
    /// <returns></returns>
    public static List<Obj> ToList<Obj>(this IEnumerable collections)
        => collections.OfType().Cast<Obj>().ToList();
    #endregion
    #region 转换为数组
    /// <summary>
    /// 将任意集合转换为数组
    /// </summary>
    /// <typeparam name="Obj">数组的元素类型</typeparam>
    /// <param name="collections">待转换的集合</param>
    /// <returns></returns>
    public static Obj[] ToArray<Obj>(this IEnumerable collections)
        => collections.OfType().Cast<Obj>().ToArray();
    #endregion
    #region 转换为二维数组
    /// <summary>
    /// 将一个集合转换为二维数组，
    /// 如果该集合的元素数量与二维数组的容量不同，
    /// 则多余的部分会被丢弃，缺失的部分用默认值填补
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collections">待转换的集合</param>
    /// <param name="row">二维数组的行数量</param>
    /// <param name="column">二维数组的列数量</param>
    /// <returns></returns>
    public static Obj[,] ToArray<Obj>(this IEnumerable<Obj> collections, int row, int column)
    {
        var array = new Obj[row, column];
        using var enumerator = collections.GetEnumerator();
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                if (!enumerator.MoveNext())
                    return array;
                array[r, c] = enumerator.Current;
            }
        }
        return array;
    }
    #endregion
    #endregion
    #region 如果处于DeBug状态下，则将集合转换为数组
    /// <summary>
    /// 如果处于DeBug状态下，则将集合转换为数组，
    /// 否则将其原路返回
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="objs">待转换的集合</param>
    /// <returns></returns>
    public static IEnumerable<Obj> ToArrayIfDeBug<Obj>(this IEnumerable<Obj> objs)
    {
#if DEBUG
        return objs.ToArray();
#else
        return objs;
#endif
    }

    /*问：这个方法有什么意义？
      答：在测试阶段，经常需要将延迟迭代的迭代器转换为数组，
      这是因为数组可以很方便地查看它的所有元素，且不存在任何性能损失，
      但是在实际运行阶段，这个操作会造成不必要的性能损失，
      因此作者声明了这个方法，它可以避免这种问题
    
      请注意，这个方法不能替代ToArray，
      只有在一种情况下可以且应当使用本方法，
      那就是调用ToArray的目的是为了缓存迭代器的元素，为调试提供方便*/
    #endregion
}
