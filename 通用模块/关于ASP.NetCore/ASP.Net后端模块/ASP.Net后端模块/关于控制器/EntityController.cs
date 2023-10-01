using System.DataFrancis;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// 本类型是一个实体控制器，
/// 它自带了增删改功能
/// </summary>
/// <typeparam name="Entity">实体的类型</typeparam>
[ApiController]
public abstract class EntityController<Entity> : ControllerBase
    where Entity : class
{
    #region 增加或修改
    /// <summary>
    /// 如果实体不存在，则增加实体，
    /// 如果存在，则更新实体
    /// </summary>
    /// <param name="entities">待添加或更新的实体</param>
    /// <param name="pipe">用来添加或更新实体的数据管道</param>
    /// <returns></returns>
    [HttpPost]
    public virtual Task AddOrUpdate([FromBody] Entity[] entities, [FromServices] IDataPipe pipe)
        => pipe.AddOrUpdate(entities);
    #endregion
    #region 删除实体
    /// <summary>
    /// 删除所有指定的实体
    /// </summary>
    /// <param name="entities">待删除的实体</param>
    /// <param name="pipe">用来删除实体的数据管道</param>
    /// <returns></returns>
    [HttpPost]
    public virtual Task Delete([FromBody] Entity[] entities, [FromServices] IDataPipe pipe)
        => pipe.Delete(entities);
    #endregion
}
