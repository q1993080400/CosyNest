﻿using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 这个类型是使用EFCore实现的数据管道
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="createDbContext">这个工厂方法用于创建数据库上下文，
/// 它的参数就是请求的实体类的类型</param>
sealed class EFDataPipe(Func<Type, DbContextFrancis> createDbContext) : IDataPipeDBWithJoin
{
    #region 说明文档
    /*问：使用本类型，而不是直接使用EFCore有什么好处？
      答：好处如下：
    
      1.完全屏蔽有关数据源的一切细节，从外界看来，
      本类型除了能够提供和管理数据以外，没有其他的特征

      2.自动管理DbContext的生命周期，无需显式释放
    
      3.能够根据实体类的类型自动查找DBSet，
      这可以让调用者不需要关心数据是从哪里来的，
      只需声明自己想要什么类型的数据就可以了，减轻心智负担，
      修改和删除数据的时候也能够享受到这个好处，不需要考虑数据在哪个表中，
      同时，它支持显式指定主键
    
      4.设计更加合理，使用AddOrUpdate替代Add和Update，
      使调用者不需要关心数据到底是添加还是修改
    
      5.所有成员都是纯函数，没有线程安全问题，
      这使得本类型可以作为单例服务，不需要反复创建和回收
    
      6.适配了本框架的IDataPipe接口，它可以和本框架的有关类型更好地协同工作
    
      7.可以和其他管道连接起来，作为更进一步的抽象，例如，
      你可以在数据库管道的前面放置一个缓存管道，
      只有缓存中不存在的数据才会向数据库请求
      */
    #endregion
    #region 公开成员
    #region 查询实体
    public IQueryable<Data> Query<Data>()
        where Data : class
    {
        var db = CreateDbContext(typeof(Data));
        return db.Set<Data>();
    }
    #endregion
    #region 添加或更新实体
    #region 说明文档
    /*注意事项：
      根据约定，除非specifyPrimaryKey参数为true，
      否则存在ID的实体一律视为更新，不存在ID的实体一律视为添加，而不管它们是不是真的存在于数据库中，
      因此，在不要随便显式写入实体的ID，
      也不要使用除了Guid以外的其他类型作为ID*/
    #endregion
    #region 正式方法
    public async Task AddOrUpdate<Data>(IEnumerable<Data> datas, Func<Guid, bool>? specifyPrimaryKey, CancellationToken cancellation)
        where Data : class
    {
        datas = datas.ToArray();
        var db = CreateDbContext(typeof(Data));
        ToolEFDataPipe.Track(db, datas, specifyPrimaryKey);
        await db.SaveChangesAsync(cancellation);
    }
    #endregion
    #endregion
    #region 删除实体
    #region 按照实体
    public async Task Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
        where Data : class
    {
        var db = CreateDbContext(typeof(Data));
        var dbSet = db.Set<Data>();
        dbSet.RemoveRange(datas);
        await db.SaveChangesAsync(cancellation);
    }
    #endregion
    #region 按照条件
    public async Task Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation)
        where Data : class
    {
        await Query<Data>().Where(expression).ExecuteDeleteAsync(cancellation);
    }
    #endregion
    #endregion
    #region 执行事务
    #region 无返回值
    public async Task Transaction(Func<IDataPipe, Task> transaction)
    {
        using var dataPipe = new EFDataPipeTransaction(CreateDbContext);
        await transaction(dataPipe);
        await dataPipe.Save();
    }
    #endregion
    #region 有返回值
    public async Task<Obj> Transaction<Obj>(Func<IDataPipe, Task<Obj>> transaction)
    {
        using var dataPipe = new EFDataPipeTransaction(CreateDbContext);
        var @return = await transaction(dataPipe);
        await dataPipe.Save();
        return @return;
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
    private Func<Type, DbContextFrancis> CreateDbContext { get; } = createDbContext;
    #endregion
    #endregion
}
