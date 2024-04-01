using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 让后端的控制器实现这个接口，
/// 它可以告诉前端如何渲染搜索视图
/// </summary>
public interface IGetRenderAllFilterCondition
{
    #region 静态方法：获取搜索条件渲染描述的方法
    /// <summary>
    /// 获取一个高阶函数，
    /// 它允许通过Http请求后端的<see cref="IGetRenderAllFilterCondition"/>接口，
    /// 然后获取<see cref="RenderAllFilterCondition"/>
    /// </summary>
    /// <typeparam name="GetRenderAllFilterCondition">后端接口的类型</typeparam>
    /// <param name="httpClient">发起请求的Http客户端</param>
    /// <returns></returns>
    public static Func<Task<RenderConditionGroup[]>> GetConditionFunction<GetRenderAllFilterCondition>(IHttpClient httpClient)
        where GetRenderAllFilterCondition : class, IGetRenderAllFilterCondition
        => () => httpClient.Request<GetRenderAllFilterCondition, RenderConditionGroup[]>(x => x.GetRenderAllFilterCondition());
    #endregion
    #region 根据业务，获取搜索条件渲染描述
    /// <summary>
    /// 获取一个对象，它描述如何渲染本业务的搜索条件
    /// </summary>
    /// <returns></returns>
    Task<RenderConditionGroup[]> GetRenderAllFilterCondition();
    #endregion
}
