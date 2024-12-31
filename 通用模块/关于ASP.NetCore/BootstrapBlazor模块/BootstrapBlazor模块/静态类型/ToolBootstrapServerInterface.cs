using System.NetFrancis.Http;

using Microsoft.AspNetCore;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个静态类是有关服务端接口的工具类，
/// 这个类型主要在Bootstrap客户端中使用
/// </summary>
public static class ToolBootstrapServerInterface
{
    #region 请求添加或修改对象
    /// <summary>
    /// 向服务器请求添加或修改对象，
    /// 如果添加失败，则进行弹窗提示
    /// </summary>
    /// <typeparam name="Parameter">添加或修改的参数类型</typeparam>
    /// <typeparam name="Interface">用于发起修改请求的API接口的类型</typeparam>
    /// <param name="httpClient">用于发起请求的Http客户端</param>
    /// <param name="parameter">添加或修改的参数</param>
    /// <param name="swalService">一个用于进行弹窗的服务</param>
    /// <returns>一个布尔值，它指示是否添加成功</returns>
    public static async Task<bool> ServerUpdate<Parameter, Interface>(IHttpClient httpClient, Parameter parameter, SwalService swalService)
        where Interface : class, IServerUpdate<Parameter>
    {
        var apiPack = await httpClient.StrongType<Interface>().
            Request(x => x.AddOrUpdate(parameter));
        return await swalService.ShowIfFailure(apiPack);
    }
    #endregion
    #region 请求删除对象
    /// <summary>
    /// 向服务器请求删除对象，
    /// 如果删除失败，则进行弹窗提示
    /// </summary>
    /// <typeparam name="Interface">用于发起删除请求的API接口的类型</typeparam>
    /// <param name="httpClient">用于发起请求的Http客户端</param>
    /// <param name="ids">要删除的对象的ID</param>
    /// <param name="swalService">一个用于进行弹窗的服务</param>
    /// <returns>一个布尔值，它指示是否删除成功</returns>
    public static async Task<bool> ServerDelete<Interface>(IHttpClient httpClient, IEnumerable<Guid> ids, SwalService swalService)
        where Interface : class, IServerDelete<IEnumerable<Guid>>
    {
        if (!await swalService.ShowConfirm("确定要删除吗？"))
            return false;
        var apiPack = await httpClient.StrongType<Interface>().
            Request(x => x.Delete(ids));
        return await swalService.ShowIfFailure(apiPack);
    }
    #endregion
}
