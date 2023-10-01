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
    /// <param name="pageIndex">分页索引</param>
    /// <param name="pageSize">每一页的最大数量</param>
    /// <returns></returns>
    public static IQueryable<Obj> Paging<Obj>(this IQueryable<Obj> query, int pageIndex, int pageSize)
        => query.Skip(pageSize * pageIndex).Take(pageSize);
    #endregion
}
