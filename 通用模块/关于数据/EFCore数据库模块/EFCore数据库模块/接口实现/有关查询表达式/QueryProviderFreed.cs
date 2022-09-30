using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 这个类型是<see cref="IQueryProvider"/>的实现，
/// 它可以用来传递<see cref="DbContext"/>的引用，
/// 确保它最后会被释放
/// </summary>
sealed class QueryProviderFreed : IQueryProvider
{
    #region 公开成员
    #region 创建查询
    public IQueryable CreateQuery(Expression expression)
        => CreateQuery<object>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        var queryable = Provider.CreateQuery<TElement>(expression);
        var dbProperty = Queryable.GetTypeData().PropertyDictionary[nameof(QueryableFreed<object>.DB)].Single();
        var queryableFreed = new QueryableFreed<TElement>(queryable, dbProperty.GetValue<DbContext>(Queryable)!);
        dbProperty.SetValue(Queryable, null);
        return queryableFreed;
    }
    #endregion
    #region 执行查询
    public object? Execute(Expression expression)
        => Provider.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
        => Provider.Execute<TResult>(expression);
    #endregion
    #endregion
    #region 内部成员
    #region 封装的表达式提供者对象
    /// <summary>
    /// 获取封装的表达式提供者对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IQueryProvider Provider { get; }
    #endregion
    #region 封装的表达式对象
    /// <summary>
    /// 获取封装的表达式对象，
    /// 本对象通过它来传递<see cref="DbContext"/>的引用
    /// </summary>
    public IOrderedQueryable Queryable { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="provider">表达式提供者对象，
    /// 本对象的功能就是通过它实现的</param>
    /// <param name="queryable">封装的表达式对象，
    /// 本对象通过它来传递<see cref="DbContext"/>的引用</param>
    public QueryProviderFreed(IQueryProvider provider, IOrderedQueryable queryable)
    {
        Provider = provider;
        Queryable = queryable;
    }
    #endregion
}
