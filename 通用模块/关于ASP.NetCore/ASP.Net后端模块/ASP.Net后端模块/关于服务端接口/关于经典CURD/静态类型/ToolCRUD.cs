using System.Design;
using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类允许进行典型的CRUD操作，
/// 这个类型主要在服务端使用
/// </summary>
public static class ToolCRUD
{
    #region 直接返回所有对象
    /// <summary>
    /// 直接返回数据库中的所有对象，
    /// 务必谨慎使用这个接口
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <typeparam name="Info">API实体的类型</typeparam>
    /// <param name="serviceProvider">一个用来请求服务的对象</param>
    /// <returns></returns>
    public static async Task<Info[]> GetAll<DBEntity, Info>(IServiceProvider serviceProvider)
        where DBEntity : class, IProjection<Info>
        where Info : class
    {
        await using var pipe = serviceProvider.RequiredDataPipe();
        return [.. pipe.Query<DBEntity>().ToArray().Projection()];
    }
    #endregion
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
            return DBEntity.CreateOrFill(entity, x, pipe).Object;
        }).ToArray();
        return addOrUpdateEntity;
    }
    #endregion
    #region 添加或更新实体
    #region 可以包含复杂的参数
    /// <summary>
    /// 添加或更新实体的默认方法，
    /// 它可以用来快速实现<see cref="IServerUpdate{Parameter}"/>接口
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <typeparam name="Info">API实体的类型</typeparam>
    /// <typeparam name="Parameter">用来提交更新的参数</typeparam>
    /// <param name="serviceProvider">用来请求服务的对象</param>
    /// <param name="getInfo">通过提交更新参数获取API实体的委托</param>
    /// <returns></returns>
    public static async Task<APIPackUpdate> AddOrUpdate<DBEntity, Info, Parameter>
        (IServiceProvider serviceProvider, Parameter parameter, Func<Parameter, Info> getInfo)
        where DBEntity : class, IFill<DBEntity, Parameter>, IWithID
        where Info : class, IWithID
    {
        var info = getInfo(parameter);
        var verify = ToolWebApi.VerifyParameter<APIPackUpdate>(serviceProvider, info);
        if (verify is { })
            return verify;
        await using var pipe = serviceProvider.RequiredDataPipe();
        var data = pipe.Query<DBEntity>().FindOrDefault(info.ID);
        var (addOrUpdateData, sideEffect) = DBEntity.CreateOrFill(data, parameter, pipe);
        if (sideEffect is { })
            await sideEffect(serviceProvider);
        await pipe.Push(x => x.AddOrUpdate([addOrUpdateData]));
        return new()
        {
            ReplaceID = addOrUpdateData.ID
        };
    }
    #endregion
    #region 直接使用业务实体
    /// <param name="info">要添加或更新的API实体</param>
    /// <returns></returns>
    /// <inheritdoc cref="AddOrUpdate{DBEntity, Info, Parameter}(IServiceProvider, Parameter, Func{Parameter, Info})"/>
    public static Task<APIPackUpdate> AddOrUpdate<DBEntity, Info>(IServiceProvider serviceProvider, Info info)
        where DBEntity : class, IFill<DBEntity, Info>, IWithID
        where Info : class, IWithID
        => AddOrUpdate<DBEntity, Info, Info>(serviceProvider, info, x => x);
    #endregion
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
        => pipe.Query<DBEntity>().FindOrDefault(id)?.Projection();
    #endregion
    #region 执行删除操作
    /// <summary>
    /// 执行典型的删除操作，
    /// 如果实体类实现了<see cref="IHasDeleteSideEffect{Entity}"/>，
    /// 还会清理它
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <param name="serviceProvider">用来请求服务的对象</param>
    /// <param name="ids">要删除的所有实体的ID</param>
    /// <returns></returns>
    public static async Task<APIPack> Delete<DBEntity>(IServiceProvider serviceProvider, IEnumerable<Guid> ids)
        where DBEntity : class, IWithID
    {
        await using var pipe = serviceProvider.RequiredDataPipe();
        await pipe.Query<DBEntity>().Where(x => ids.Contains(x.ID)).
            ExecuteDeleteAndClean(pipe, serviceProvider);
        return new();
    }
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
