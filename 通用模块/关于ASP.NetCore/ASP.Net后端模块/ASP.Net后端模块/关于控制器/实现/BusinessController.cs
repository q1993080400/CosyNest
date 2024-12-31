using System.Design;
using System.NetFrancis.Api;

using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个抽象类是业务控制器的基类，
/// 它为某些增删改查操作提供了模板方法
/// </summary>
/// <typeparam name="BusinessInterface">业务接口的类型</typeparam>
/// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
/// <typeparam name="Info">API实体类的类型</typeparam>
public abstract class BusinessController<BusinessInterface, DBEntity, Info> : ApiController,
    IGetRenderAllFilterCondition,
    IServerUpdate<Info>,
    IServerDelete<IEnumerable<Guid>>
    where BusinessInterface : class, IGetRenderAllFilterCondition
    where DBEntity : class, IFill<DBEntity, Info>, IWithID
    where Info : class, IWithID
{
    #region 获取搜索条件
    public virtual Task<RenderFilterGroup[]> GetRenderAllFilterCondition()
    {
        var renderCondition = ToolSearchViewer.GetRenderCondition<DBEntity, BusinessInterface>();
        return Task.FromResult(renderCondition);
    }
    #endregion
    #region 添加或更新
    public virtual Task<APIPack> AddOrUpdate(Info parameter)
        => ToolCRUD.AddOrUpdate<DBEntity, Info>(HttpContext.RequestServices, parameter);
    #endregion
    #region 执行删除操作
    public virtual Task<APIPack> Delete(IEnumerable<Guid> parameter)
        => ToolCRUD.Delete<DBEntity>(HttpContext.RequestServices, parameter);
    #endregion
}
