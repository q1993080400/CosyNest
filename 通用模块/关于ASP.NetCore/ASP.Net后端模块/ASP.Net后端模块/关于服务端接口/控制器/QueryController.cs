using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个抽象类是查询控制器的基类，
/// 它提供了搜索条件渲染方式的模板方法
/// </summary>
/// <typeparam name="BusinessInterface">业务接口的类型</typeparam>
/// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
public abstract class QueryController<BusinessInterface, DBEntity> : ApiController, IGetRenderAllFilterCondition
    where BusinessInterface : class, IGetRenderAllFilterCondition
    where DBEntity : class
{
    #region 获取搜索条件
    public virtual Task<RenderFilterGroup[]> GetRenderAllFilterCondition()
        => ToolSearchViewer.GetRenderCondition<DBEntity, BusinessInterface>().ToTask();
    #endregion
}
