﻿using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB;

/// <summary>
/// 本类型是使用EF实现的数据推送上下文
/// </summary>
/// <param name="dbContext">封装的数据上下文</param>
sealed class EFDataPipeToContext(DbContext dbContext) : IDataPipeToContext
{
    #region 查询数据源抽象
    public IQueryable<Data> Query<Data>()
        where Data : class
        => dbContext.Set<Data>();
    #endregion
    #region 获取所有受支持的实体类型
    public IEnumerable<Type>? EntityTypes
        => dbContext.EntityTypes();
    #endregion
    #region 添加或更新数据
    public async Task AddOrUpdate<Data>(IEnumerable<Data> datas, AddOrUpdateInfo<Data>? info = null, CancellationToken cancellation = default)
        where Data : class
    {
        info ??= new();
        if ((info, datas) is ({ UpdateBusinessProperty: true }, IEnumerable<IUpdateBusinessProperty> updateBusinessPropertys))
        {
            foreach (var item in updateBusinessPropertys)
            {
                item.UpdateBusinessProperty();
            }
        }
        var isAdd = info.IsAddData;
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
    #region 传入IQueryable
    public Task Delete<Data>(IQueryable<Data> datas, CancellationToken cancellation = default)
         where Data : class
        => datas.ExecuteDeleteAsync(cancellation);
    #endregion
    #endregion
    #region 显式保存数据
    public Task SaveChanges(CancellationToken cancellationToken = default)
         => dbContext.SaveChangesAsync(cancellationToken);
    #endregion
}
