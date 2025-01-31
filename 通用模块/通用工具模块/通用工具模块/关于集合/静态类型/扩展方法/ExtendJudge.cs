namespace System.Linq;

public static partial class ExtendEnumerable
{
    /*所有关于集合的判断与查找的方法，全部放在这个部分类中，
      判断与查找指的是：API返回某一个存在于集合中的元素，
      或是其他单个对象，而不是返回一个新的集合*/

    #region 关于索引，范围和元素位置
    #region 封装集合的元素和索引
    /// <summary>
    /// 封装一个集合的元素和索引，并返回一个新集合
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collection">待转换的集合</param>
    /// <returns>一个元组，它的项分别是元素，索引，以及是否为最后一个元素</returns>
    public static IEnumerable<(int Index, Obj Elements, bool IsLast)> PackIndex<Obj>(this IEnumerable<Obj> collection)
    {
        var index = -1;
        using var enumerator = collection.GetEnumerator();
        if (!enumerator.MoveNext())
            yield break;
        var current = enumerator.Current;
        while (true)
        {
            index++;
            var hasNext = enumerator.MoveNext();
            if (hasNext)
            {
                yield return (index, current, false);
                current = enumerator.Current;
            }
            else
            {
                yield return (index, current, true);
                yield break;
            }
        }
    }
    #endregion
    #region 关于Range
    #region 解构范围
    /// <summary>
    /// 将范围解构为开始和结束索引
    /// </summary>
    /// <param name="r">待解构的范围</param>
    /// <param name="begin">开始索引</param>
    /// <param name="end">结束索引</param>
    public static void Deconstruct(this Range r, out Index begin, out Index end)
    {
        begin = r.Start;
        end = r.End;
    }
    #endregion
    #region 返回范围的开始和结束
    #region 传入集合的长度
    /// <summary>
    /// 返回范围的开始和结束
    /// </summary>
    /// <param name="range">待返回开始和结束的范围</param>
    /// <param name="length">集合元素的数量</param>
    /// <returns></returns>
    public static (int Start, int End) GetStartAndEnd(this Range range, int length)
    {
        var (start, len) = range.GetOffsetAndLength(length);
        return (start, start + len - 1);
    }
    #endregion
    #region 传入集合
    /// <summary>
    /// 根据一个集合的元素数量，计算范围的开始和结束
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="range">待返回开始和结束的范围</param>
    /// <param name="list">用来计算元素数量的集合</param>
    /// <returns></returns>
    public static (int Start, int End) GetStartAndEnd<Obj>(this Range range, IEnumerable<Obj> list)
        => range.GetStartAndEnd(list.Count());
    #endregion
    #endregion
    #region 返回是否为确定范围
    /// <summary>
    /// 返回一个范围是否为确定范围，
    /// 也就是它的<see cref="Range.End"/>是否从集合开头数起，
    /// 确定范围不会随着集合元素的增减而发生变化
    /// </summary>
    /// <param name="range">待确定的范围</param>
    /// <returns></returns>
    public static bool IsAccurate(this Range range)
        => !range.End.IsFromEnd;
    #endregion
    #endregion
    #region 关于Index
    #region 根据集合计算索引
    /// <summary>
    /// 根据集合，计算出索引的实际值
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="index">待计算的索引</param>
    /// <param name="collection">索引所指向的集合</param>
    /// <returns></returns>
    public static int GetOffset<Obj>(this Index index, IEnumerable<Obj> collection)
        => index.IsFromEnd ? index.GetOffset(collection.Count()) : index.Value;
    #endregion
    #region 转换为Range
    /// <summary>
    /// 将一个<see cref="Index"/>转换为<see cref="Range"/>，
    /// 它仅选取集合中的一个元素
    /// </summary>
    /// <param name="index">待转换的<see cref="Index"/></param>
    /// <returns></returns>
    public static Range ToRange(this Index index)
    {
        if (index.Equals(^0))
            throw new ArgumentException($"{index}为^0，它本身不在集合中，不能为它创建范围");
        var i = index.Value + (index.IsFromEnd ? -1 : 1);
        return new Range(index, new Index(i, index.IsFromEnd));
    }
    #endregion
    #region 根据索引返回元素，不会引发异常
    /// <summary>
    /// 根据索引返回元素，不会引发异常
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="objs">待返回元素的集合</param>
    /// <param name="index">元素的索引</param>
    /// <param name="indexOut">如果索引非法，则返回这个延迟对象的值</param>
    /// <returns></returns>
    public static Obj? ElementAt<Obj>(this IEnumerable<Obj> objs, Index index, Lazy<Obj>? indexOut)
    {
        try
        {
            return objs.ElementAt(index);
        }
        catch (Exception e) when (e is ArgumentNullException or ArgumentOutOfRangeException)
        {
            return indexOut.Value();
        }
    }
    #endregion
    #endregion
    #region 按条件寻找索引
    /// <summary>
    /// 寻找集合中第一个符合条件的元素，
    /// 并返回它的索引，如果集合中不存在符合条件的元素，则返回-1
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="objs">元素的集合</param>
    /// <param name="test">用来测试集合中的元素是否符合要求的委托</param>
    /// <returns></returns>
    public static int IndexOf<Obj>(this IEnumerable<Obj> objs, Func<Obj, bool> test)
    {
        foreach (var (index, obj) in objs.Index())
        {
            if (test(obj))
                return index;
        }
        return -1;
    }
    #endregion
    #region 按元素寻找索引
    /// <summary>
    /// 按照元素寻找索引，
    /// 如果集合中有多个相同的元素，则返回第一个元素的索引，
    /// 如果没有这个元素，则返回-1
    /// </summary>
    /// <param name="element">要寻找索引的元素</param>
    /// <returns></returns>
    /// <inheritdoc cref="IndexOf{Obj}(IEnumerable{Obj}, Func{Obj, bool})"/>
    public static int IndexOf<Obj>(this IEnumerable<Obj> objs, Obj element)
        => objs.IndexOf(x => Equals(x, element));
    #endregion
    #endregion
    #region 关于重复元素
    #region 返回集合中是否存在重复元素
    /// <summary>
    /// 如果一个集合中存在重复的元素，
    /// 则返回<see langword="true"/>，否则返回<see langword="false"/>
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">要检查的集合</param>
    /// <returns></returns>
    public static bool AnyRepeat<Obj>(this IEnumerable<Obj> collections)
    {
        var hash = new HashSet<Obj>();
        foreach (var item in collections)
        {
            if (hash.Add(item))
                return true;
        }
        return false;
    }
    #endregion
    #region 改变一个元素，直到集合中没有重复的元素
    #region 可适用于任何集合
    /// <summary>
    /// 如果一个元素存在于某个集合中，则不断地改变它，
    /// 直到集合中没有这个元素为止，
    /// 这个方法可以为向某些不可重复的集合中添加元素提供方便
    /// </summary>
    /// <typeparam name="Ret">元素类型</typeparam>
    /// <param name="collections">要检查的集合</param>
    /// <param name="obj">要检查的元素</param>
    /// <param name="change">如果集合中存在这个元素，则执行这个委托，
    /// 返回一个新元素，委托的第一个参数是原始元素，第二个参数是尝试的次数，从2开始</param>
    /// <returns></returns>
    public static Ret Distinct<Ret>(this IEnumerable<Ret> collections, Ret obj, Func<Ret, int, Ret> change)
    {
        var hash = collections.ToHashSet();
        var repeat = 2;
        var obj2 = obj;
        while (hash.Contains(obj))
        {
            obj = change(obj2, repeat++);
        }
        return obj;
    }

    /*repeat从2开始的原因在于：
      在某些情况下能够比较方便的重命名重复元素，
      例如：在Excel中发现了重复的工作表，
      这时就可以直接将其命名为工作表（2）*/
    #endregion
    #region 仅适用于String集合
    /// <summary>
    /// 如果一个<see cref="string"/>存在于某个集合中，则不断地改变它，
    /// 直到集合中没有这个<see cref="string"/>为止，
    /// 改变的方法为将原始文本转化为Text(1)，Text(2)的形式
    /// </summary>
    /// <param name="collections">文本所在的集合</param>
    /// <param name="text">要检查的文本</param>
    /// <returns></returns>
    public static string Distinct(this IEnumerable<string> collections, string text)
        => collections.Distinct(text, static (x, y) => $"{x}({y})");
    #endregion
    #endregion
    #endregion
    #region 返回唯一符合条件的元素，或默认值
    #region 无测试条件
    /// <summary>
    /// 返回集合中唯一的元素，
    /// 如果集合为空，或者存在多个元素，则返回默认值
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="objs">要返回唯一元素的集合</param>
    /// <returns></returns>
    /// <inheritdoc cref="SingleOrDefaultSecure{Obj}(IEnumerable{Obj}, Func{Obj, bool})"/>
    public static Obj? SingleOrDefaultSecure<Obj>(this IEnumerable<Obj> objs)
    {
        using var enumerator = objs.GetEnumerator();
        if (!enumerator.MoveNext())
            return default;
        var current = enumerator.Current;
        return enumerator.MoveNext() ? default : current;
    }
    #endregion
    #region 具有测试条件
    /// <summary>
    /// 返回集合中唯一符合条件的元素，
    /// 如果集合为空，或者存在多个符合条件的元素，
    /// 则返回默认值
    /// </summary>
    /// <param name="func">用来测试集合中每个元素的委托</param>
    /// <inheritdoc cref="SingleOrDefaultSecure{Obj}(IEnumerable{Obj})"/>
    /// <returns></returns>
    public static Obj? SingleOrDefaultSecure<Obj>(this IEnumerable<Obj> objs, Func<Obj, bool> func)
        => objs.Where(func).SingleOrDefaultSecure();
    #endregion
    #endregion
}
