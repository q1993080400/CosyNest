namespace Microsoft.AspNetCore;

/// <summary>
/// 让后端的控制器实现这个接口，
/// 它可以告诉前端如何渲染搜索视图
/// </summary>
public interface IGetRenderAllFilterCondition
{
    #region 根据业务，获取搜索条件渲染描述
    /// <summary>
    /// 获取一个对象，它描述如何渲染本业务的搜索条件
    /// </summary>
    /// <returns></returns>
    Task<RenderFilterGroup[]> GetRenderAllFilterCondition();
    #endregion
}
