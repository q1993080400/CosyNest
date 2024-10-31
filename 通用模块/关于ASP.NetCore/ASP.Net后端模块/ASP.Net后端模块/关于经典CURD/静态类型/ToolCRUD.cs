using System.DataFrancis;
using System.Design;
using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类允许进行典型的CRUD操作
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
            return DBEntity.CreateOrFill(entity, x);
        }).ToArray();
        return addOrUpdateEntity;
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
    /// <summary>
    /// 执行典型的删除操作
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <param name="pipe">推送数据源抽象</param>
    /// <param name="ids">要删除的所有实体的ID</param>
    /// <returns></returns>
    public static async Task<APIPack> Delete<DBEntity>(IDataPipeToContext pipe, IEnumerable<Guid> ids)
        where DBEntity : class, IWithID
    {
        await pipe.Delete<DBEntity>(x => ids.Contains(x.ID));
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
