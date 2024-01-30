using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个通用的增删改查控制器
/// </summary>
/// <typeparam name="APIEntity">需要操作的API实体的类型，
/// 注意，它不一定，也不推荐直接使用数据库实体</typeparam>
public interface IEntityController<APIEntity>
{
    #region 添加或修改实体
    /// <summary>
    /// 添加或修改实体
    /// </summary>
    /// <param name="entities">要添加或修改的实体</param>
    /// <returns></returns>
    [HttpMethodPost]
    Task<APIPack> AddOrUpdate(APIEntity[] entities);
    #endregion
    #region 删除实体
    /// <summary>
    /// 批量删除实体
    /// </summary>
    /// <param name="ids">要删除的实体的ID</param>
    /// <returns></returns>
    [HttpMethodPost]
    Task<APIPack> Delete(Guid[] ids);
    #endregion
}
