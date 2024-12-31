using System.Design;
using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类允许进行典型的CRUD操作，
/// 这个类型主要在服务端使用
/// </summary>
public static class ToolCRUD
{
    #region 返回添加或更新时，被改变的实体
    /// <summary>
    /// 执行典型的添加或更新操作，
    /// 并返回已经被改变的所有实体，
    /// 它们可以被直接保存到数据库，
    /// 也可以进行进一步的处理
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <typeparam name="Info">API实体的类型</typeparam>
    /// <param name="pipe">查询数据源抽象</param>
    /// <param name="info">API实体</param>
    /// <returns></returns>
    public static DBEntity[] GetAddOrUpdateChangeEntity<DBEntity, Info>(IDataPipeFromContext pipe, IEnumerable<Info> info)
        where DBEntity : class, IFill<DBEntity, Info>, IWithID
        where Info : class, IWithID
    {
        var ids = info.Select(x => x.ID).ToArray();
        var entityDictionary = pipe.Query<DBEntity>().
            Where(x => ids.Contains(x.ID)).ToDictionary(x => x.ID, x => x);
        var addOrUpdateEntity = info.Select(x =>
        {
            var entity = entityDictionary.GetValueOrDefault(x.ID);
            return DBEntity.CreateOrFill(entity, x).Object;
        }).ToArray();
        return addOrUpdateEntity;
    }
    #endregion
    #region 添加或更新实体
    /// <summary>
    /// 添加或更新实体的默认方法，
    /// 它可以用来快速实现<see cref="IServerUpdate{Parameter}"/>接口
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <typeparam name="Info">API实体的类型</typeparam>
    /// <param name="serviceProvider">用来请求服务的对象</param>
    /// <param name="info">要添加或更新的API实体</param>
    /// <returns></returns>
    public static async Task<APIPack> AddOrUpdate<DBEntity, Info>(IServiceProvider serviceProvider, Info info)
        where DBEntity : class, IFill<DBEntity, Info>, IWithID
        where Info : class, IWithID
    {
        var verify = ToolWebApi.VerifyParameter<APIPack>(serviceProvider, info);
        if (verify is { })
            return verify;
        await using var pipe = serviceProvider.RequiredDataPipe();
        var id = info.ID;
        var data = pipe.Query<DBEntity>().FirstOrDefault(x => x.ID == id);
        var (addOrUpdateData, sideEffect) = DBEntity.CreateOrFill(data, info);
        if (sideEffect is { })
            await sideEffect(serviceProvider);
        await pipe.Push(x => x.AddOrUpdate([addOrUpdateData]));
        return new();
    }
    #endregion
    #region 根据ID寻找实体
    /// <summary>
    /// 根据ID，寻找实体，
    /// 并将其转换为API对象返回，
    /// 如果没有找到，则为<see langword="null"/>
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <typeparam name="Info">API实体的类型</typeparam>
    /// <param name="pipe">查询数据源抽象</param>
    /// <param name="id">实体的ID</param>
    /// <returns></returns>
    public static Info? Find<DBEntity, Info>(IDataPipeFromContext pipe, Guid id)
        where DBEntity : class, IProjection<Info>, IWithID
        where Info : class, IWithID
        => pipe.Query<DBEntity>().FirstOrDefault(x => x.ID == id)?.Projection();
    #endregion
    #region 执行删除操作
    #region 通用方法
    /// <summary>
    /// 执行典型的删除操作
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <param name="serviceProvider">用来请求服务的对象</param>
    /// <param name="ids">要删除的所有实体的ID</param>
    /// <returns></returns>
    public static async Task<APIPack> Delete<DBEntity>(IServiceProvider serviceProvider, IEnumerable<Guid> ids)
        where DBEntity : class, IWithID
    {
        await using var pipe = serviceProvider.RequiredDataPipe();
        await pipe.Push(pipe => pipe.Delete<DBEntity>(x => ids.Contains(x.ID)));
        return new();
    }
    #endregion
    #region 为IClean特化
    /// <summary>
    /// 执行典型删除操作的普通方法，
    /// 它为<see cref="IClean{Entity}"/>接口进行优化，
    /// 在执行删除操作以后，还会进行清理操作
    /// </summary>
    /// <inheritdoc cref="Delete{DBEntity}(IServiceProvider, IEnumerable{Guid})"/>
    public static async Task<APIPack> DeleteClean<DBEntity>(IServiceProvider serviceProvider, IEnumerable<Guid> ids)
        where DBEntity : class, IWithID, IClean<DBEntity>
    {
        try
        {
            await using var pipe = serviceProvider.RequiredDataPipe();
            await pipe.Push(async pipe =>
            {
                var delete = pipe.Query<DBEntity>().Where(x => ids.Contains(x.ID)).ToArray();
                await pipe.Delete(delete);
                try
                {
                    await DBEntity.Clean(delete, serviceProvider);
                }
                catch (Exception ex)    //不抛出异常，保证成功删除后，数据库不会因此回滚
                {
                    ex.Log(serviceProvider);
                }
            });
            return new();
        }
        catch (Exception ex)
        {
            ex.Log(serviceProvider);
            return new()
            {
                FailureReason = ex.Message,
            };
        }
    }
    #endregion
    #endregion
    #region 更新部分属性
    /// <summary>
    /// 更新实体的部分属性，
    /// 这个方法可以用来帮助实现<see cref="IServerUpdatePart{Parameter}.UpdateProperty(Parameter)"/>
    /// </summary>
    /// <typeparam name="Entity">要更新属性的实体类型</typeparam>
    /// <param name="info">描述要更新的属性的参数</param>
    /// <param name="update">这个委托用来更新单个属性</param>
    /// <param name="pipe">推送更新的数据管道对象</param>
    /// <returns></returns>
    public static async Task<APIPack> UpdatePart<Entity>(ServerUpdateEntityInfo info, Func<UpdatePartInfo<Entity>, Task> update, IDataPipe pipe)
        where Entity : class, IWithID
    {
        var updatePropertyInfo = info.UpdatePropertyInfo;
        if (updatePropertyInfo.Count is 0)
            return new();
        return await pipe.Push<APIPack>(async pipe =>
        {
            var id = info.ID;
            var entity = pipe.Query<Entity>().FirstOrDefault(x => x.ID == id);
            if (entity is null)
                return new();
            var type = typeof(Entity);
            #region 用于更新的本地函数
            Task DefaultUpdate(Entity entity, ServerUpdatePropertyInfo updateInfo)
            {
                var propertyName = updateInfo.PropertyName;
                var property = type.GetProperty(propertyName) ??
                    throw new NotSupportedException($"未能找到名为{propertyName}的实体属性");
                property.SetValue(entity, updateInfo.Value.To(property.PropertyType));
                return Task.CompletedTask;
            }
            #endregion
            foreach (var updateProperty in updatePropertyInfo)
            {
                var updatePartInfo = new UpdatePartInfo<Entity>()
                {
                    Model = entity,
                    DefaultUpdate = DefaultUpdate,
                    UpdatePropertyInfo = updateProperty,
                    DataPipe = pipe,
                };
                await update(updatePartInfo);
            }
            await pipe.AddOrUpdate([entity]);
            return new();
        });
    }
    #endregion
}
