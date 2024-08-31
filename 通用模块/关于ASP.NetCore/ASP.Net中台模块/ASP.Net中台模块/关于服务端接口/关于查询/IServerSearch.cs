using System.Collections.Immutable;
using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个服务端搜索接口
/// </summary>
/// <typeparam name="Parameter">搜索方法的参数类型</typeparam>
/// <typeparam name="Obj">要搜索的对象类型</typeparam>
public interface IServerSearch<Parameter, Obj>
{
    #region 静态成员：根据搜索接口，获取异步枚举器
    #region 根据搜索接口，获取异步枚举器
    /// <summary>
    /// 根据搜索接口，获取一个异步枚举器，
    /// 它用来枚举符合条件的元素
    /// </summary>
    /// <typeparam name="Interface">服务端搜索接口的类型</typeparam>
    /// <param name="httpClient">用来发起Http请求的对象</param>
    /// <param name="generateParameter">这个委托传入页的索引，返回搜索的参数</param>
    /// <returns></returns>
    public static IAsyncEnumerable<Obj> GetAllObj<Interface>(IHttpClient httpClient, Func<int, Parameter> generateParameter)
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
    #region 根据搜索接口，获取异步枚举器，且使用排除ID进行分页
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
    /// <inheritdoc cref="GetAllObj{Interface}(IHttpClient, Func{int, Parameter})"/>
    public static async IAsyncEnumerable<Obj> GetAllObjFromExclude<Interface>
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
    #endregion
    #region 搜索对象
    /// <summary>
    /// 搜索所有对象
    /// </summary>
    /// <param name="parameter">搜索的参数</param>
    /// <returns></returns>
    [HttpMethodPost]
    Task<Obj[]> Search(Parameter parameter);
    #endregion
}
