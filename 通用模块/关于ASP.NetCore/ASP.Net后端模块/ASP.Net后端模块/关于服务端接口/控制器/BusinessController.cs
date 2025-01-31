using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个抽象类是业务控制器的基类，
/// 它为某些增删改查操作提供了模板方法
/// </summary>
/// <typeparam name="Info">API实体类的类型</typeparam>
/// <inheritdoc cref="QueryController{BusinessInterface, DBEntity}"/>
public abstract class BusinessController<BusinessInterface, DBEntity, Info> : QueryController<BusinessInterface, DBEntity>,
    IGetRenderAllFilterCondition,
    IServerUpdate<Info>,
    IServerDelete<IEnumerable<Guid>>
    where BusinessInterface : class, IGetRenderAllFilterCondition
    where DBEntity : class, IFill<DBEntity, Info>, IWithID
    where Info : class, IWithID
{
    #region 添加或更新
    public virtual Task<APIPackUpdate> AddOrUpdate(Info parameter)
        => ToolCRUD.AddOrUpdate<DBEntity, Info>(HttpContext.RequestServices, parameter);
    #endregion
    #region 执行删除操作
    public virtual Task<APIPack> Delete(IEnumerable<Guid> parameter)
        => ToolCRUD.Delete<DBEntity>(HttpContext.RequestServices, parameter);
    #endregion
}
