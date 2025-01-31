using System.DataFrancis;
using System.NetFrancis;
using System.NetFrancis.Api;

using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

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
    /// 如果添加失败，则进行弹窗提示，
    /// 注意：它会首先进行客户端验证
    /// </summary>
    /// <typeparam name="Parameter">添加或修改的参数类型</typeparam>
    /// <typeparam name="Interface">用于发起修改请求的API接口的类型</typeparam>
    /// <param name="parameter">添加或修改的参数</param>
    /// <param name="serviceProvider">一个用于请求服务的对象，本服务依赖于一些其他服务</param>
    /// <returns>请求接口的结果，从它的<see cref="APIPack.IsSuccess"/>属性可以判断请求是否成功</returns>
    public static async Task<APIPackUpdate> ServerUpdate<Parameter, Interface>(Parameter parameter, IServiceProvider serviceProvider)
        where Interface : class, IServerUpdate<Parameter>
        where Parameter : notnull
    {
        #region 获取API封包的本地函数
        async Task<APIPackUpdate> GetAPIPack()
        {
            var dataVerify = serviceProvider.GetRequiredService<DataVerify>();
            var verificationResults = dataVerify(parameter);
            if (verificationResults.IsSuccess)
            {
                var strongTypeInvokeFactory = serviceProvider.GetRequiredService<IStrongTypeInvokeFactory>();
                return await strongTypeInvokeFactory.StrongType<Interface>().
                    Invoke(x => x.AddOrUpdate(parameter));
            }
            return new()
            {
                FailureReason = verificationResults.FailureReasonMessage()
            };
        }
        #endregion
        var apiPack = await GetAPIPack();
        var swalService = serviceProvider.GetRequiredService<SwalService>();
        await swalService.ShowIfFailure(apiPack);
        return apiPack;
    }
    #endregion
    #region 请求删除对象
    /// <summary>
    /// 向服务器请求删除对象，
    /// 如果删除失败，则进行弹窗提示
    /// </summary>
    /// <typeparam name="Interface">用于发起删除请求的API接口的类型</typeparam>
    /// <param name="ids">要删除的对象的ID</param>
    /// <param name="serviceProvider">一个用于请求服务的对象，本服务依赖于一些其他服务</param>
    /// <returns>一个布尔值，它指示是否删除成功</returns>
    public static async Task<bool> ServerDelete<Interface>(IEnumerable<Guid> ids, IServiceProvider serviceProvider)
        where Interface : class, IServerDelete<IEnumerable<Guid>>
    {
        var swalService = serviceProvider.GetRequiredService<SwalService>();
        if (!await swalService.ShowConfirm("确定要删除吗？"))
            return false;
        var strongTypeInvokeFactory = serviceProvider.GetRequiredService<IStrongTypeInvokeFactory>();
        var apiPack = await strongTypeInvokeFactory.StrongType<Interface>().
            Invoke(x => x.Delete(ids));
        return await swalService.ShowIfFailure(apiPack);
    }
    #endregion
}
