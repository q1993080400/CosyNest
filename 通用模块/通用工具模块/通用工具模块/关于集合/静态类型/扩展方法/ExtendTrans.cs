namespace System.Linq;

public static partial class ExtendEnumerable
{
    /*所有关于集合的转化的方法，全部放在这个部分类中，
      集合的转化指的是：API返回一个新的集合*/

    #region 关于拆分集合
    #region 按条件拆分
    /// <summary>
    /// 按照条件，将一个集合拆分成两部分，
    /// 分别是满足条件和不满足条件的部分
    /// </summary>
    /// <typeparam name="Obj">要拆分的集合元素类型</typeparam>
    /// <param name="collections">要拆分的集合</param>
    /// <param name="delegate">对集合元素进行判断的委托</param>
    /// <returns></returns>
    public static (IReadOnlyList<Obj> IsTrue, IReadOnlyList<Obj> IsFalse) Split<Obj>(this IEnumerable<Obj> collections, Func<Obj, bool> @delegate)
    {
        var (isTrue, isFalse) = (new List<Obj>(), new List<Obj>());
        foreach (var i in collections)
        {
            var list = @delegate(i) ? isTrue : isFalse;
            list.Add(i);
        }
        return (isTrue, isFalse);
    }
    #endregion
    #endregion
    #region 关于Zip
    #region 合并两个集合
    /// <summary>
    /// 合并两个集合的元素，
    /// 如果两个集合的元素数量不相等，
    /// 则缺失的元素会被默认值填补
    /// </summary>
    /// <typeparam name="ObjA">第一个集合的元素类型</typeparam>
    /// <typeparam name="ObjB">第二个集合的元素类型</typeparam>
    /// <typeparam name="Ret">返回值集合的元素类型</typeparam>
    /// <param name="collectionsA">第一个集合</param>
    /// <param name="collectionsB">第二个集合</param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    public static IEnumerable<Ret> ZipFill<ObjA, ObjB, Ret>(this IEnumerable<ObjA> collectionsA, IEnumerable<ObjB> collectionsB,
        Func<ObjA?, ObjB?, Ret> resultSelector)
    {
        using var enumeratorA = collectionsA.GetEnumerator();
        using var enumeratorB = collectionsB.GetEnumerator();
        while (true)
        {
            var hasNextA = enumeratorA.MoveNext();
            var hasNextB = enumeratorB.MoveNext();
            if ((hasNextA, hasNextB) is (false, false))
                yield break;
            #region 本地函数
            static Element? GetElement<Element>(IEnumerator<Element> enumerator, bool hasNext)
                => hasNext ? enumerator.Current : default;
            #endregion
            yield return resultSelector(GetElement(enumeratorA, hasNextA), GetElement(enumeratorB, hasNextB));
        }
    }
    #endregion
    #region 将两个集合合并为元组
    /// <summary>
    /// 将两个集合的元素合并为元组，
    /// 如果两个集合的元素数量不相等，
    /// 则缺失的元素会被默认值填补
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ZipFill{ObjA, ObjB, Ret}(IEnumerable{ObjA}, IEnumerable{ObjB}, Func{ObjA, ObjB, Ret})"/>
    public static IEnumerable<(ObjA? A, ObjB? B)> ZipFill<ObjA, ObjB>(this IEnumerable<ObjA> collectionsA, IEnumerable<ObjB> collectionsB)
        => collectionsA.ZipFill(collectionsB, static (x, y) => (x, y));
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
    #region 筛选集合中的非空对象
    /// <summary>
    /// 筛选集合中的非空对象
    /// </summary>
    /// <typeparam name="Obj">要筛选的集合元素类型</typeparam>
    /// <param name="objs">要筛选的集合元素</param>
    /// <returns></returns>
    public static IEnumerable<Obj> WhereNotNull<Obj>(this IEnumerable<Obj?> objs)
        => objs.Where(static x => x is { })!;
    #endregion
    #region 摊平嵌套集合
    /// <summary>
    /// 将一个嵌套集合摊平，然后转换为普通集合
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="list">待摊平的嵌套集合</param>
    /// <returns></returns>
    public static IEnumerable<Obj> SelectMany<Obj>(this IEnumerable<IEnumerable<Obj>> list)
        => list.SelectMany(static x => x);
    #endregion
}
