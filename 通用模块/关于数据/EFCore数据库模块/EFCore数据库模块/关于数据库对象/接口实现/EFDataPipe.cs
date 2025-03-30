using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 本类型是使用EF实现的数据管道
/// </summary>
/// <param name="dbContext">封装的数据上下文</param>
sealed class EFDataPipe(DbContext dbContext) : IDataPipe
{
    #region 查询数据源抽象
    public IQueryable<Data> Query<Data>()
        where Data : class
        => dbContext.Set<Data>();
    #endregion
    #region 执行推送上下文
    #region 无返回值
    public async Task Push(Func<IDataPipeToContext, Task> execute)
    {
        await execute(new EFDataPipeToContext(dbContext));
        await dbContext.SaveChangesAsync();
    }
    #endregion
    #region 有返回值
    public async Task<Obj> Push<Obj>(Func<IDataPipeToContext, Task<Obj>> execute)
    {
        var @return = await execute(new EFDataPipeToContext(dbContext));
        await dbContext.SaveChangesAsync();
        return @return;
    }
    #endregion
    #endregion
    #region 获取所有受支持的实体类型
    public IEnumerable<Type>? EntityTypes
        => dbContext.EntityTypes();
    #endregion
    #region 释放对象
    public ValueTask DisposeAsync()
        => dbContext.DisposeAsync();
    #endregion
}
