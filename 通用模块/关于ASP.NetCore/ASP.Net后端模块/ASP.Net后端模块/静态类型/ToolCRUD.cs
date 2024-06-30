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
}
