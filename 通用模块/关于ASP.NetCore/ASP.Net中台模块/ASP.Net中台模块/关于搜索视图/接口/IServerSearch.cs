using System.DataFrancis;
using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个服务端搜索接口
/// </summary>
/// <typeparam name="Obj">要搜索的对象类型</typeparam>
public interface IServerSearch<Obj>
{
    #region 静态方法：根据搜索接口，获取异步枚举器
    /// <summary>
    /// 根据搜索接口，获取一个异步枚举器，
    /// 它用来枚举符合条件的元素
    /// </summary>
    /// <typeparam name="Interface">服务端搜索接口的类型</typeparam>
    /// <param name="httpClient">用来发起Http请求的对象</param>
    /// <param name="dataFilter">用来筛选数据的对象</param>
    /// <returns></returns>
    public static IAsyncEnumerable<Obj> GetAllObj<Interface>(IHttpClient httpClient, DataFilterDescription dataFilter)
        where Interface : class, IServerSearch<Obj>
        => CreateCollection.MergePage<Obj>(
        async index =>
        {
            var info = new WebSearchInfo()
            {
                FilterCondition = dataFilter,
                Index = index
            };
            var objs = await httpClient.Request<Interface, Obj[]>(x => x.Search(info));
            return objs;
        });
    #endregion
    #region 搜索对象
    /// <summary>
    /// 搜索所有对象
    /// </summary>
    /// <param name="searchInfo">搜索的参数</param>
    /// <returns></returns>
    [HttpMethodPost]
    Task<Obj[]> Search(WebSearchInfo searchInfo);
    #endregion
}
