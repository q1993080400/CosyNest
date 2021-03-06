using System.Collections.Generic;

namespace System.Linq
{
    public static partial class ExtenIEnumerable
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
                if (a.Count > b.Count)
                    return b.All(x => a.Contains(x)) ? CollectionContains.Superset : CollectionContains.NotMatter;
                return a.All(x => b.Contains(x)) ?
                    (a.Count < b.Count ? CollectionContains.Subset : CollectionContains.Equal) :
                    CollectionContains.NotMatter;
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
            foreach (var (e, index, _) in collection.PackIndex())
            {
                if (Equals(e, obj))
                    return index;
            }
            return -1;
        }
        #endregion
        #region 封装集合的元素和索引
        /// <summary>
        /// 封装一个集合的元素，索引，和元素数量，并返回一个新集合
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="collection">待转换的集合</param>
        /// <param name="getCount">如果这个值为<see langword="true"/>，则会获取集合元素的数量，但是这会影响性能，
        /// 如果为<see langword="false"/>，则不会这样做，返回的元组的Count字段为-1</param>
        /// <returns></returns>
        public static IEnumerable<(Obj Elements, int Index, int Count)> PackIndex<Obj>(this IEnumerable<Obj> collection, bool getCount = false)
        {
            var index = 0;
            var count = getCount ? collection.Count() : -1;
            foreach (var obj in collection)
            {
                yield return (obj, index++, count);
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
        #region 按范围返回元素
        /// <summary>
        /// 按照范围，返回集合中的元素
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="collection">待返回元素的集合</param>
        /// <param name="range">返回元素的范围</param>
        /// <param name="throw">在范围非法的时候，如果这个值为<see langword="true"/>，会抛出异常，否则会返回一个空数组</param>
        /// <returns></returns>
        public static Obj[] ElementAt<Obj>(this IEnumerable<Obj> collection, Range range, bool @throw = true)
        {
            try
            {
                return (collection is Obj[] arry ? arry : collection.ToArray())[range];
            }
            catch (ArgumentOutOfRangeException) when (!@throw)
            {
                return Array.Empty<Obj>();
            }
        }
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
        #region 按索引返回元素
        /// <summary>
        /// 按照索引返回元素，不需要实现<see cref="IList{T}"/>
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="collection">要返回元素的集合</param>
        /// <param name="index">用来提取元素的索引</param>
        /// <param name="throw">在索引非法的时候，如果这个值为<see langword="true"/>，则抛出异常，否则会返回一个默认值</param>
        /// <param name="crossed">在索引非法时，会通过这个延迟对象获取默认返回值</param>
        /// <returns></returns>
        public static Obj? ElementAt<Obj>(this IEnumerable<Obj> collection, Index index, bool @throw = true, LazyPro<Obj>? crossed = null)
        {
            try
            {
                return collection is IList<Obj> l ?
                    l[index] : collection.ElementAt(index.GetOffset(collection));
            }
            catch (ArgumentOutOfRangeException) when (!@throw)
            {
                return crossed;
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
            #region 本地函数
            IEnumerable<Obj> Fun()
            {
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
        #region 关于Sum
        #region 可以对任何对象使用
        /// <summary>
        /// 返回一个序列的和，可以对任何对象使用，
        /// 只要它重载了加法运算符， 或使用委托指定了计算加法的方式
        /// </summary>
        /// <typeparam name="Obj">集合元素和返回值的类型</typeparam>
        /// <param name="collections">要计算总和的集合</param>
        /// <param name="add">这个委托指定了计算加法的方式，
        /// 如果为<see langword="null"/>，则尝试调用自带的加法运算符</param>
        /// <returns></returns>
        public static Obj Sum<Obj>(this IEnumerable<Obj> collections, Func<Obj, Obj, Obj>? add = null)
        {
            var arry = collections.ToArray();
            add ??= new Func<Obj, Obj, Obj>((x, y) => (dynamic?)x + y);
            return arry.Length == 1 ? arry[0] : arry.Aggregate(add);
        }
        #endregion
        #endregion
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
            var (left, other, hasElements) = collections.First(false);
            if (hasElements)
            {
                foreach (var right in other)
                {
                    yield return @delegate(left, right);
                    left = right;
                }
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
                var (retIten, newSeed) = @delegate(item, initial);
                initial = newSeed;
                yield return retIten;
            }
        }
        #endregion
        #endregion
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
        #region 返回一个集合的极限
        /// <summary>
        /// 返回一个集合的极限，也就是根据一个函数，
        /// 从元素中提取出键，然后返回键最大或者最小的元素
        /// </summary>
        /// <typeparam name="Ret">集合元素的类型，也是返回值类型</typeparam>
        /// <typeparam name="Key">键的类型，方法会比较键的大小，而不是<typeparamref name="Ret"/>的大小</typeparam>
        /// <param name="collections">要返回极限的集合</param>
        /// <param name="delegate">从元素中提取键的函数</param>
        /// <param name="returnMax">如果这个值为<see langword="true"/>，返回集合的最大值，为<see langword="false"/>，返回最小值</param>
        /// <param name="comparison">用来比较键的比较器，如果为<see langword="null"/>，则默认为该类型的默认比较器</param>
        /// <returns></returns>
        public static Ret Limit<Ret, Key>(this IEnumerable<Ret> collections, Func<Ret, Key> @delegate, bool returnMax, Func<Key, Key, int>? comparison = null)
        {
            comparison ??= Comparer<Key>.Default.Compare;
            return collections.Aggregate((x, y) =>
            comparison(
                @delegate(x), @delegate(y)) > 0 == returnMax ?         //根据x是否比y大，以及需要最大值或最小值，获取正确的结果
                x : y);
        }
        /*注释：算法为：
          每次检查集合中的两个元素，
          返回其中较大的一个，
          不断迭代直到集合末尾
           
          虽然也可以直接排序，
          然后取排序集合的第一位或最后一位，
          但是这个算法只需要遍历集合一次，
          性能有巨大的提高*/
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
}
