using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 这个类型是支持事务的<see cref="IDataPipe"/>实现，
/// 只要将它一直传递下去，就可以保证事务一致性
/// </summary>
sealed class EFDataPipeTransaction : IDataPipeDB, IDisposable
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
        if ((DbContext, DBTransaction) is ({ }, { }))
        {
            try
            {
                await DbContext.SaveChangesAsync();
                await DBTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await DBTransaction.RollbackAsync();
                throw;
            }
            finally
            {
                Dispose();
            }
        }
    }
    #endregion
    #region 释放对象
    public void Dispose()
    {
        DBTransaction?.Dispose();
        DbContext?.Dispose();
    }
    #endregion
    #region 执行查询
    public IQueryable<Data> Query<Data>()
        where Data : class, IData
         => CreateDbContextFromEntityType(typeof(Data)).Set<Data>();
    #endregion
    #region 添加或更新数据
    public Task AddOrUpdate<Data>(IEnumerable<Data> datas, Func<Guid, bool>? specifyPrimaryKey, CancellationToken cancellation)
         where Data : class, IData
    {
        datas = datas.ToArray();
        var db = CreateDbContextFromEntityType(typeof(Data));
        EFDataPipe.Track(db, datas, specifyPrimaryKey);
        return Task.CompletedTask;
    }
    #endregion 
    #region 删除数据
    #region 按照实体
    public Task Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
         where Data : class, IData
    {
        var db = CreateDbContextFromEntityType(typeof(Data));
        var dbSet = db.Set<Data>();
        dbSet.RemoveRange(datas);
        return Task.CompletedTask;
    }
    #endregion
    #region 按照条件
    public async Task Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation)
         where Data : class, IData
    {
        await Query<Data>().Where(expression).ExecuteDeleteAsync(cancellation);
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
    #region 事务对象
    /// <summary>
    /// 获取数据库事务对象
    /// </summary>
    private IDbContextTransaction? DBTransaction { get; set; }
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
    {
        if (DbContext is { })
            return DbContext;
        DbContext = CreateDbContext(entityType);
        DBTransaction = DbContext.Database.BeginTransaction();
        return DbContext;
    }
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
