using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

public static partial class ExtendEnumerable
{
    /*所有关于集合的判断与查找的方法，全部放在这个部分类中，
      判断与查找指的是：API返回某一个存在于集合中的元素，
      或是其他单个对象，而不是返回一个新的集合*/

    #region 有关包含关系
    #region 返回一个集合与另一个集合的包含关系
    /// <summary>
    /// 判断集合A相对于集合B的包含关系
    /// </summary>
    /// <typeparam name="Obj">要比较的集合的元素类型</typeparam>
    /// <param name="collectionA">集合A</param>
    /// <param name="collectionB">集合B</param>
    /// <param name="isRelational">如果这个值为<see langword="true"/>，
    /// 表示判断基于关系模型，不会考虑重复元素和元素的顺序</param>
    /// <returns></returns>
    public static CollectionContains IsSupersetOf<Obj>(this IEnumerable<Obj> collectionA, IEnumerable<Obj> collectionB, bool isRelational = false)
    {
        #region 遵循关系模型
        CollectionContains FollowRelational()
        {
            var a = collectionA.ToHashSet();
            var b = collectionB.ToHashSet();
            #region 本地函数
            static bool Fun(IEnumerable<Obj> a, IEnumerable<Obj> b)
                => a.All(x => b.Contains(x));
            #endregion
            return a.Count.CompareTo(b.Count) switch
            {
                0 => Fun(a, b) ? CollectionContains.Equal : CollectionContains.NotMatter,
                > 0 => Fun(b, a) ? CollectionContains.Superset : CollectionContains.NotMatter,
                < 0 => Fun(a, b) ? CollectionContains.Subset : CollectionContains.NotMatter
            };
        }
        #endregion
        #region 不遵循关系模型
        CollectionContains NotRelational()
        {
            var (a, b) = (collectionA.GetEnumerator(), collectionB.GetEnumerator());
            CollectionContains Get(bool aHas, bool bHas)
                => (aHas, bHas) switch
                {
                    (false, false) => CollectionContains.Equal,
                    (true, false) => CollectionContains.Superset,
                    (false, true) => CollectionContains.Subset,
                    _ => Equals(a.Current, b.Current) ? Get(a.MoveNext(), b.MoveNext()) : CollectionContains.NotMatter
                };
            return Get(a.MoveNext(), b.MoveNext());
        }
        #endregion
        return isRelational ? FollowRelational() : NotRelational();
    }
    #endregion
    #region 判断包含关系是否为子集
    /// <summary>
    /// 判断某一包含关系是否为子集
    /// </summary>
    /// <param name="collectionContains">待判断的包含关系</param>
    /// <returns>如果<paramref name="collectionContains"/>为
    /// <see cref="CollectionContains.Equal"/>或<see cref="CollectionContains.Subset"/>，
    /// 则返回<see langword="true"/>，否则返回<see langword="false"/></returns>
    public static bool IsSubset(this CollectionContains collectionContains)
        => collectionContains is
        CollectionContains.Equal or CollectionContains.Subset;
    #endregion
    #endregion
    #region 关于索引和元素位置
    #region 关于Int形式的索引
    #region 按照元素返回索引
    /// <summary>
    /// 在任意泛型集合中，根据元素返回索引，不需要实现<see cref="IList{T}"/>，
    /// 如果没有找到，返回-1
    /// </summary>
    /// <typeparam name="Obj">泛型集合的类型</typeparam>
    /// <param name="collection">元素所在的泛型集合</param>
    /// <param name="obj">要检查索引的元素</param>
    /// <param name="optimization">如果这个值为<see langword="true"/>，
    /// 则当<paramref name="collection"/>实现<see cref="IList{T}"/>时，调用<see cref="IList{T}.IndexOf(T)"/>以提高性能，
    /// 如果本方法在<see cref="IList{T}.IndexOf(T)"/>中调用，请传入<see langword="false"/>以避免无限递归异常</param>
    /// <returns></returns>
    public static int BinarySearch<Obj>(this IEnumerable<Obj> collection, Obj obj, bool optimization = true)
    {
        if (collection is IList<Obj> c && optimization)
            return c.IndexOf(obj);
        foreach (var (e, index) in collection.Index())
        {
            if (Equals(e, obj))
                return index;
        }
        return -1;
    }
    #endregion
    #region 封装集合的元素和索引
    /// <summary>
    /// 封装一个集合的元素和索引，并返回一个新集合
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collection">待转换的集合</param>
    /// <returns></returns>
    public static IEnumerable<(Obj Elements, int Index)> Index<Obj>(this IEnumerable<Obj> collection)
    {
        var index = 0;
        foreach (var obj in collection)
        {
            yield return (obj, index++);
        }
    }
    #endregion
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
    public static (int Begin, int End) GetOffsetAndEnd(this Range range, int length)
    {
        var (begin, len) = range.GetOffsetAndLength(length);
        return (begin, begin + len - 1);
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
    public static (int Begin, int End) GetOffsetAndEnd<Obj>(this Range range, IEnumerable<Obj> list)
    {
        var (b, e) = range;
        if (!b.IsFromEnd && !e.IsFromEnd)
            return (b.Value, e.Value);
        var count = list.Count();           //即便开始和结束索引都是倒着数的，也只需要计算一次元素数量
        return (b.GetOffset(count), e.GetOffset(count));
    }
    #endregion
    #endregion
    #region 返回是否为确定范围
    /// <summary>
    /// 返回一个范围是否为确定范围，
    /// 也就是它的<see cref="Range.End"/>是否从集合开头数起，
    /// 确定范围不会随着集合元素的增减而发生变化
    /// </summary>
    /// <param name="r">待确定的范围</param>
    /// <returns></returns>
    public static bool IsAccurate(this Range r)
        => !r.End.IsFromEnd;
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
    /// 将一个<see cref="System.Index"/>转换为<see cref="Range"/>，
    /// 它仅选取集合中的一个元素
    /// </summary>
    /// <param name="index">待转换的<see cref="System.Index"/></param>
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
    public static Obj? ElementAt<Obj>(this IEnumerable<Obj> objs, Index index, LazyPro<Obj>? indexOut)
    {
        try
        {
            return objs.ElementAt(index);
        }
        catch (Exception e) when (e is ArgumentNullException or ArgumentOutOfRangeException)
        {
            return indexOut;
        }
    }
    #endregion
    #endregion
    #region 返回首位元素
    /// <summary>
    /// 传入一个集合，然后返回一个元组，
    /// 它的项分别是集合的第一个元素，剩下的元素（该枚举器只能枚举一次），以及集合中是否存在元素
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collections">待返回元素的集合</param>
    /// <param name="check">在集合没有任何元素的情况下，
    /// 如果这个值为<see langword="true"/>，则引发异常，否则返回默认值</param>
    /// <returns></returns>
    public static (Obj First, IEnumerable<Obj> Other, bool HasElements) First<Obj>(this IEnumerable<Obj> collections, bool check)
    {
        var enumerator = collections.GetEnumerator();
        var hasElements = enumerator.MoveNext();
        var first = hasElements ? enumerator.Current :
            check ? throw new Exception("该集合没有任何元素") : default!;
        var isUsed = false;
        #region 本地函数
        IEnumerable<Obj> Fun()
        {
            if (isUsed)
                throw new NotSupportedException(@"不允许遍历该集合两次");
#pragma warning disable IDE0059                 //这个赋值仍然有用，该警告是分析器的疏忽
            isUsed = true;
#pragma warning restore
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            enumerator.Dispose();
        }
        #endregion
        return (first, Fun(), hasElements);
    }
    #endregion
    #endregion
    #region 关于累加
    #region 聚合函数
    /// <summary>
    /// 将集合中所有相邻的元素聚合起来，并返回一个新集合，
    /// 如果集合的元素小于2，则返回一个空集合
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="collections">要聚合的集合</param>
    /// <param name="delegate">这个委托的第一个参数是两个相邻元素的左边，
    /// 第二个元素是相邻元素的右边，返回值是聚合后的新元素</param>
    /// <returns></returns>
    public static IEnumerable<Ret> Polymerization<Obj, Ret>(this IEnumerable<Obj> collections, Func<Obj, Obj, Ret> @delegate)
    {
        var (left, other, _) = collections.First(false);
        foreach (var right in other)
        {
            yield return @delegate(left, right);
            left = right;
        }
    }
    #endregion
    #region 关于Aggregate
    #region 带转换，且可以访问种子的累加
    /// <summary>
    /// 将一个集合的元素转化为另一种类型，
    /// 在转换的时候，可以访问一个种子作为参考
    /// </summary>
    /// <typeparam name="Source">原始集合的元素类型</typeparam>
    /// <typeparam name="Seed">种子的类型</typeparam>
    /// <typeparam name="Ret">返回值的类型</typeparam>
    /// <param name="collections">待转换的原始集合</param>
    /// <param name="initial">种子的初始值</param>
    /// <param name="delegate">这个委托的参数是集合元素和当前的种子，
    /// 并返回一个元组，分别是转换后的新元素，以及经过累加后的新种子</param>
    /// <returns></returns>
    public static IEnumerable<Ret> AggregateSelect<Source, Seed, Ret>(this IEnumerable<Source> collections, Seed initial, Func<Source, Seed, (Ret, Seed)> @delegate)
    {
        foreach (var item in collections)
        {
            var (retItem, newSeed) = @delegate(item, initial);
            initial = newSeed;
            yield return retItem;
        }
    }
    #endregion
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
        => collections.Distinct(text, (x, y) => $"{x}({y})");
    #endregion
    #endregion
    #endregion
    #region 返回集合是否不为null且存在元素
    /// <summary>
    /// 如果集合不为<see langword="null"/>且存在至少一个元素，
    /// 则返回<see langword="true"/>，否则返回<see langword="false"/>
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="objs">要判断的集合</param>
    /// <returns></returns>
    public static bool AnyAndNotNull<Obj>([NotNullWhen(true)] this IEnumerable<Obj>? objs)
        => objs?.Any() ?? false;
    #endregion
    #region 返回唯一符合条件的元素，或默认值
    #region 无测试条件
    /// <summary>
    /// 返回集合中唯一的元素，
    /// 如果集合为空，则返回默认值
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
#region 集合包含关系枚举
/// <summary>
/// 这个枚举指示两个集合之间的包含关系
/// </summary>
public enum CollectionContains
{
    /// <summary>
    /// 表示真超集
    /// </summary>
    Superset,
    /// <summary>
    /// 表示真子集
    /// </summary>
    Subset,
    /// <summary>
    /// 表示两个集合完全等价
    /// </summary>
    Equal,
    /// <summary>
    /// 表示两个集合之间没有任何包含关系
    /// </summary>
    NotMatter
}
#endregion