using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 这个类型是支持事务的<see cref="IDataPipe"/>实现，
/// 只要将它一直传递下去，就可以保证事务一致性
/// </summary>
sealed class EFDataPipeTransaction : IDataPipe, IDisposable
{
    #region 公开成员
    #region 执行事务
    public Task Transaction(Func<IDataPipe, Task> transaction)
        => throw new NotImplementedException("本框架不支持递归事务，所以无法使用本方法");

    public Task<Obj> Transaction<Obj>(Func<IDataPipe, Task<Obj>> transaction)
        => throw new NotImplementedException("本框架不支持递归事务，所以无法使用本方法");
    #endregion
    #region 保存事务
    /// <summary>
    /// 调用这个方法可以保存事务
    /// </summary>
    /// <returns></returns>
    public async Task Save()
    {
        var task = DbContext?.SaveChangesAsync();
        if (task is { })
            await task;
    }
    #endregion
    #region 释放对象
    public void Dispose()
        => DbContext?.Dispose();
    #endregion
    #region 执行查询
    IQueryable<Data> IDataPipeFrom.Query<Data>()
        => CreateDbContextFromEntityType(typeof(Data)).Set<Data>();
    #endregion
    #region 添加或更新数据
    Task<IEnumerable<Data>> IDataPipeTo.AddOrUpdate<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
    {
        datas = datas.ToArray();
        var db = CreateDbContextFromEntityType(typeof(Data));
        var dbSet = db.Set<Data>();
        dbSet.UpdateRange(datas);
        return Task.FromResult(datas);
    }
    #endregion
    #region 删除数据
    #region 按照实体
    Task IDataPipeTo.Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
    {
        var db = CreateDbContextFromEntityType(typeof(Data));
        var dbSet = db.Set<Data>();
        dbSet.RemoveRange(datas);
        return Task.CompletedTask;
    }
    #endregion
    #region 按照条件
    Task IDataPipeTo.Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation)
    {
        var db = CreateDbContextFromEntityType(typeof(Data));
        var dbSet = db.Set<Data>();
        dbSet.RemoveRange(dbSet.Where(expression));
        return Task.CompletedTask;
    }
    #endregion 
    #endregion
    #endregion
    #region 内部成员
    #region 创建数据库上下文的工厂
    /// <summary>
    /// 这个工厂方法用于创建数据库上下文，
    /// 它的参数就是请求的实体类的类型
    /// </summary>
    private Func<Type, DbContextFrancis> CreateDbContext { get; }
    #endregion
    #region 根据实体类型创建数据上下文
    /// <summary>
    /// 获取缓存的数据库上下文
    /// </summary>
    private DbContext? DbContext { get; set; }

    /// <summary>
    /// 根据实体类的类型，获取数据库上下文
    /// </summary>
    /// <param name="entityType">实体类的类型</param>
    /// <returns></returns>
    private DbContext CreateDbContextFromEntityType(Type entityType)
        => DbContext ??= CreateDbContext(entityType);
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="createDbContext">这个工厂方法用于创建数据库上下文，
    /// 它的参数就是请求的实体类的类型</param>
    public EFDataPipeTransaction(Func<Type, DbContextFrancis> createDbContext)
    {
        CreateDbContext = createDbContext;
    }
    #endregion
}
