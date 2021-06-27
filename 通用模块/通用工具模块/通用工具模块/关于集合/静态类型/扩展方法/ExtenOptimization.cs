using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class ExtenIEnumerable
    {
        /*这个分部类专门储存为特定集合类型优化过的扩展方法，
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
        #region 为ICollection<T>优化
        #region 合并ICollection<T>
        /// <summary>
        /// 取多个<see cref="ICollection{T}"/>的并集
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
            foreach (var item in collectionOther)
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
        #region 对异步集合执行Linq查询
        /// <summary>
        /// 对一个异步集合执行Linq查询，并返回单个结果
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <typeparam name="Obj">异步集合的元素类型</typeparam>
        /// <param name="collections">待查询的异步集合</param>
        /// <param name="fun">用来执行查询的函数，它的参数是一个等效的同步集合</param>
        /// <returns></returns>
        public static Task<Ret> Linq<Ret, Obj>(this IAsyncEnumerable<Obj> collections, Func<IEnumerable<Obj>, Ret> fun)
            => Task.Run(() => fun(collections.Fit()));
        #endregion
    }
}
