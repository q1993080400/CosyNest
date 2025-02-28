using System.Linq.Expressions;

namespace System.Linq;

public static partial class ExtendEnumerable
{
    //所有关于表达式的方法全部放在这个分部类中

    #region 分页
    /// <summary>
    /// 执行分页操作，并返回分页结果
    /// </summary>
    /// <typeparam name="Obj">分页的元素类型</typeparam>
    /// <param name="query">要执行分页的数据源</param>
    /// <param name="pageSize">每一页的最大数量</param>
    /// <param name="pageIndex">分页索引</param>
    /// <returns></returns>
    public static IQueryable<Obj> Paging<Obj>(this IQueryable<Obj> query, int pageSize, int pageIndex)
        => query.Skip(pageSize * pageIndex).Take(pageSize);
    #endregion
    #region 外部差集
    /// <summary>
    /// 对集合A和集合B生成键，
    /// 取A集合键减B集合键的差集，
    /// 然后返回键对应的值
    /// </summary>
    /// <typeparam name="ListAlEment">集合A的元素类型</typeparam>
    /// <typeparam name="ListBlEment">集合B的元素类型</typeparam>
    /// <typeparam name="Key">键的类型</typeparam>
    /// <param name="listA">集合A</param>
    /// <param name="listB">集合B</param>
    /// <param name="getListAKey">从集合A获取键的委托</param>
    /// <param name="getListBKey">从集合B获取键的表达式</param>
    /// <returns></returns>
    public static IEnumerable<ListAlEment> ExceptExternal<ListAlEment, ListBlEment, Key>
        (this IEnumerable<ListAlEment> listA, IQueryable<ListBlEment> listB,
        Func<ListAlEment, Key> getListAKey, Expression<Func<ListBlEment, Key>> getListBKey)
    {
        var keys = listA.Select(getListAKey).ToHashSet();
        var intersect = listB.Select(getListBKey).Intersect(keys).ToArray();
        return [.. listA.ExceptBy(intersect, getListAKey)];
    }
    #endregion
}
