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
    /// <param name="refresh">如果这个对象不为<see langword="null"/>，则在修改成功以后，还会刷新它</param>
    public static async Task<APIPackUpdate> ServerUpdate<Parameter, Interface>
        (Parameter parameter, IServiceProvider serviceProvider, IRefresh? refresh = null)
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
        if ((apiPack.IsSuccess, refresh) is (true, { }))
            await refresh.Refresh();
        return apiPack;
    }
    #endregion
    #region 请求删除对象
    #region 对象不实现IWithObjectID
    /// <summary>
    /// 向服务器请求删除对象，
    /// 如果删除失败，则进行弹窗提示
    /// </summary>
    /// <typeparam name="Interface">用于发起删除请求的API接口的类型</typeparam>
    /// <param name="ids">要删除的对象的ID</param>
    /// <param name="serviceProvider">一个用于请求服务的对象，本服务依赖于一些其他服务</param>
    /// <returns>一个布尔值，它指示是否删除成功</returns>
    /// <param name="refresh">如果这个对象不为<see langword="null"/>，则在修改成功以后，还会刷新它</param>
    /// <returns>删除是否成功</returns>
    public static async Task<bool> ServerDelete<Interface>(IEnumerable<Guid> ids, IServiceProvider serviceProvider, IRefresh? refresh = null)
        where Interface : class, IServerDelete<IEnumerable<Guid>>
    {
        var swalService = serviceProvider.GetRequiredService<SwalService>();
        if (!await swalService.ShowConfirm("确定要删除吗？"))
            return false;
        var strongTypeInvokeFactory = serviceProvider.GetRequiredService<IStrongTypeInvokeFactory>();
        var apiPack = await strongTypeInvokeFactory.StrongType<Interface>().
            Invoke(x => x.Delete(ids));
        var isSuccess = await swalService.ShowIfFailure(apiPack);
        if ((isSuccess, refresh) is (true, { }))
            await refresh.Refresh();
        return isSuccess;
    }
    #endregion
    #region 对象实现了IWithObjectID
    /// <summary>
    /// 如果一个对象是在客户端内存中临时创建的草稿，
    /// 则直接删除它，否则向服务器请求将它删除
    /// </summary>
    /// <typeparam name="Obj">要删除的对象的类型</typeparam>
    /// <param name="deleteObj">要删除的对象</param>
    /// <param name="list">容纳要删除对象的内存容器</param>
    /// <inheritdoc cref="ServerDelete{Interface}(IEnumerable{Guid}, IServiceProvider, IRefresh?)"/>
    public static async Task<bool> ServerDelete<Interface, Obj>(Obj deleteObj, IServiceProvider serviceProvider, IList<Obj> list, IRefresh? refresh = null)
        where Interface : class, IServerDelete<IEnumerable<Guid>>
        where Obj : class, IWithID, IWithObjectID
    {
        var isNew = deleteObj.IsNew();
        #region 返回删除是否成功的委托
        async Task<bool> Delete()
        {
            if (isNew)
                return true;
            Guid[] id = [deleteObj.ID];
            return await ServerDelete<Interface>(id, serviceProvider, refresh);
        }
        #endregion
        var isSuccess = await Delete();
        if (isSuccess && isNew)
            list.RemoveAll(x => x.ObjectID == deleteObj.ObjectID);
        return isSuccess;
    }
    #endregion
    #endregion
}
