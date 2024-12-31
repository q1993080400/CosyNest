using System.Collections.Immutable;
using System.DataFrancis;
using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类是有关服务端接口的工具类，
/// 这个类型主要在客户端使用
/// </summary>
public static class ToolServerInterface
{
    #region 延迟返回元素的建议数量
    private static int PlusDefaultField = 20;

    /// <summary>
    /// 在后端控制器以虚拟化的方式延迟返回元素的时候，
    /// 建议每次返回该数量的元素，
    /// 它可以和虚拟化组件的每次加载数量对齐，
    /// 获得更好的性能
    /// </summary>
    public static int PlusDefault
    {
        get => PlusDefaultField;
        set => PlusDefaultField = value >= 15 ?
            value :
            throw new NotSupportedException($"{nameof(PlusDefault)}属性的最小合法值是15，您写入的值是{value}，已被拒绝");
    }
    #endregion
    #region 根据搜索接口，获取异步枚举器
    #region 按照索引分页
    /// <summary>
    /// 根据搜索接口，获取一个异步枚举器，
    /// 它用来枚举符合条件的元素
    /// </summary>
    /// <typeparam name="Interface">服务端搜索接口的类型</typeparam>
    /// <param name="httpClient">用来发起Http请求的对象</param>
    /// <param name="generateParameter">这个委托传入页的索引，返回搜索的参数</param>
    /// <returns></returns>
    /// <inheritdoc cref="IServerSearch{Parameter, Obj}"/>
    public static IAsyncEnumerable<Obj> GetAllObj<Interface, Parameter, Obj>(IHttpClient httpClient, Func<int, Parameter> generateParameter)
        where Interface : class, IServerSearch<Parameter, Obj>
        => CreateCollection.MergePage<Obj>(
        async index =>
        {
            var parameter = generateParameter(index);
            var objs = await httpClient.StrongType<Interface>().
            Request(x => x.Search(parameter));
            return objs;
        });
    #endregion
    #region 按照排除ID进行分页
    #region 不限制要搜索的对象类型
    /// <summary>
    /// 根据搜索接口，获取一个异步枚举器，
    /// 它用来枚举符合条件的元素，
    /// 它使用排除ID模式进行分页，
    /// 不会因为实体的属性被改变而引发错乱
    /// </summary>
    /// <param name="generateParameter">这个委托传入已经枚举的实体的ID，
    /// 返回值是向后端发起请求的参数</param>
    /// <param name="generateID">这个委托传入实体，返回实体的ID</param>
    /// <returns></returns>
    /// <inheritdoc cref="GetAllObj{Interface, Parameter, Obj}(IHttpClient, Func{int, Parameter})"/>
    public static async IAsyncEnumerable<Obj> GetAllObjFromExclude<Interface, Parameter, Obj>
        (IHttpClient httpClient, Func<IEnumerable<Guid>, Parameter> generateParameter, Func<Obj, Guid> generateID)
        where Interface : class, IServerSearch<Parameter, Obj>
    {
        var ids = ImmutableHashSet<Guid>.Empty;
        while (true)
        {
            var parameter = generateParameter(ids);
            var objs = await httpClient.StrongType<Interface>().Request(x => x.Search(parameter));
            if (objs.Length is 0)
                yield break;
            foreach (var obj in objs)
            {
                yield return obj;
            }
            ids = ids.Union(objs.Select(generateID));
        }
    }
    #endregion
    #region 为IWithID优化
    /// <inheritdoc cref="GetAllObjFromExclude{Interface, Parameter, Obj}(IHttpClient, Func{IEnumerable{Guid}, Parameter}, Func{Obj, Guid})"/>
    public static IAsyncEnumerable<Obj> GetAllObjFromExclude<Interface, Parameter, Obj>
        (IHttpClient httpClient, Func<IEnumerable<Guid>, Parameter> generateParameter)
        where Interface : class, IServerSearch<Parameter, Obj>
        where Obj : IWithID
        => GetAllObjFromExclude<Interface, Parameter, Obj>(httpClient, generateParameter, x => x.ID);
    #endregion
    #endregion
    #endregion
    #region 获取搜索条件渲染描述的方法
    /// <summary>
    /// 获取一个高阶函数，
    /// 它允许通过Http请求后端的<see cref="IGetRenderAllFilterCondition"/>接口，
    /// 然后获取<see cref="RenderFilterGroup"/>
    /// </summary>
    /// <typeparam name="GetRenderAllFilterCondition">后端接口的类型</typeparam>
    /// <param name="httpClient">发起请求的Http客户端</param>
    /// <returns></returns>
    public static Func<Task<RenderFilterGroup[]>> GetConditionFunction<GetRenderAllFilterCondition>(IHttpClient httpClient)
        where GetRenderAllFilterCondition : class, IGetRenderAllFilterCondition
        => () => httpClient.StrongType<GetRenderAllFilterCondition>().
        Request(x => x.GetRenderAllFilterCondition());
    #endregion
}
