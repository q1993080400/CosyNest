using System.Design;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 本类型是使用EF实现的数据管道
/// </summary>
/// <param name="dbContext">封装的数据上下文</param>
sealed class EFDataPipe(DbContext dbContext) : AutoRelease, IDataPipe
{
    #region 接口实现
    #region 查询数据源抽象
    public IQueryable<Data> Query<Data>()
        where Data : class
        => dbContext.Set<Data>();
    #endregion
    #region 添加或更新数据
    public async Task AddOrUpdate<Data>(IEnumerable<Data> datas, Func<Data, bool>? isAdd = null, CancellationToken cancellation = default)
        where Data : class
    {
        if (isAdd is null)
        {
            dbContext.UpdateRange(datas);
            return;
        }
        var (add, update) = datas.Split(isAdd);
        await dbContext.AddRangeAsync(add, cancellation);
        dbContext.UpdateRange(update);
    }
    #endregion
    #region 删除数据
    #region 按照实体
    public Task Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation = default)
        where Data : class
    {
        dbContext.RemoveRange(datas);
        return Task.CompletedTask;
    }
    #endregion
    #region 按照条件
    public Task Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation = default)
        where Data : class
        => Query<Data>().Where(expression).ExecuteDeleteAsync(cancellation);
    #endregion
    #endregion
    #endregion
    #region 抽象类实现
    #region 释放对象
    protected override void DisposeRealize()
    {
        dbContext.SaveChanges();
        dbContext.Dispose();
    }
    #endregion
    #endregion 
}
