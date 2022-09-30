using System.Collections;
using System.Design;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;


namespace System.DataFrancis.DB.EF;

/// <summary>
/// 本类型是<see cref="IOrderedQueryable{T}"/>的实现，
/// 它不仅提供了查询表达式的功能，
/// 还能够自动释放封装的<see cref="DbContext"/>对象
/// </summary>
/// <typeparam name="Data">实体类的类型</typeparam>
sealed class QueryableFreed<Data> : AutoRelease, IOrderedQueryable<Data>
{
    #region 重要说明
    /*问：本类型通过什么逻辑释放DbContext对象？
      答：假设有一个以下结构的表达式链条：
    
      A>>B>>C
    
      则：C被回收时，DbContext会被释放
    
      问：本类型的早期版本在遍历完元素后释放DbContext，
      为什么现在不这样了？
      答：这是因为，不完全遍历元素的情况非常常见，
      例如使用First方法的情形，这种情况下，会产生内存泄露*/
    #endregion
    #region 公开成员
    #region 枚举元素
    public IEnumerator<Data> GetEnumerator()
    {
        foreach (Data item in (IEnumerable)this)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => Queryable.GetEnumerator();
    #endregion
    #region 元素类型
    public Type ElementType => Queryable.ElementType;
    #endregion
    #region 表达式
    public Expression Expression => Queryable.Expression;
    #endregion
    #region 表达式提供者
    public IQueryProvider Provider { get; }
    #endregion
    #endregion
    #region 内部成员
    #region 封装的表达式对象
    /// <summary>
    /// 获取封装的表达式对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IQueryable Queryable { get; }
    #endregion
    #region 封装的数据上下文对象
    /// <summary>
    /// 获取封装的数据上下文对象，
    /// 当表达式链条上的最后一个节点被回收时，
    /// 它会被释放
    /// </summary>
    internal DbContext DB { get; set; }
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
        => DB?.Dispose();
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="queryable">表达式对象，
    /// 本对象的功能就是通过它实现的</param>
    /// <param name="db">数据上下文对象，
    /// 当遍历完所有数据后，它会被自动释放</param>
    public QueryableFreed(IQueryable queryable, DbContext db)
    {
        Queryable = queryable;
        DB = db;
        Provider = new QueryProviderFreed(queryable.Provider, this);
    }
    #endregion
}
