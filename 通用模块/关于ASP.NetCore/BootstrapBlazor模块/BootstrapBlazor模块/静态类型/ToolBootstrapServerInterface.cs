using System.DataFrancis;
using System.Linq.Expressions;
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
    #region 请求执行某操作
    /// <summary>
    /// 向服务端接口请求某些操作，
    /// 并返回结果，如果失败，它还会弹窗提示
    /// </summary>
    /// <typeparam name="APIPack">返回的响应结果类型</typeparam>
    /// <typeparam name="Interface">服务端接口的类型</typeparam>
    /// <param name="serviceProvider">用于请求服务的对象</param>
    /// <param name="invoke">这个表达式树指示如何调用服务端接口</param>
    /// <returns></returns>
    /// <inheritdoc cref="ExtendBootstrapBlazor.ShowIfFailure(SwalService, APIPack, string?)"/>
    public static async Task<APIPack> ServerInvoke<APIPack, Interface>
        (IServiceProvider serviceProvider,
        Expression<Func<Interface, Task<APIPack>>> invoke,
        string? successMessage = null)
        where Interface : class
        where APIPack : System.NetFrancis.Api.APIPack, new()
    {
        #region 获取API封包的本地函数
        async Task<APIPack> GetAPIPack()
        {
            var parameter = invoke is
            {
                Body: MethodCallExpression
                {
                    Arguments: { Count: > 0 } expressionArguments
                }
            } ?
            expressionArguments.Select(x => x.CalValue()).WhereNotNull().ToArray() : null;
            if (parameter is { })
            {
                var dataVerify = serviceProvider.GetRequiredService<DataVerify>();
                var verifications = parameter.Select(x => dataVerify(x)).
                    Where(x => !x.IsSuccess).ToArray();
                var verification = new VerificationResults()
                {
                    Data = parameter,
                    FailureReason = [.. verifications.SelectMany(x => x.FailureReason)]
                };
                if (!verification.IsSuccess)
                    return new()
                    {
                        FailureReason = verification.FailureReasonMessage()
                    };
            }
            var strongTypeInvokeFactory = serviceProvider.GetRequiredService<IStrongTypeInvokeFactory>();
            return await strongTypeInvokeFactory.StrongType<Interface>().Invoke(invoke);
        }
        #endregion
        var apiPack = await GetAPIPack();
        var swalService = serviceProvider.GetRequiredService<SwalService>();
        await swalService.ShowIfFailure(apiPack, successMessage);
        return apiPack;
    }
    #endregion
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
    /// <returns>请求接口的结果，从它的<see cref="APIPack.Success"/>属性可以判断请求是否成功</returns>
    /// <inheritdoc cref="ServerInvoke{APIPack, Interface}(IServiceProvider, Expression{Func{Interface, Task{APIPack}}}, string?)"/>
    public static Task<APIPackUpdate> ServerUpdate<Parameter, Interface>
        (Parameter parameter, IServiceProvider serviceProvider, string? successMessage = null)
        where Interface : class, IServerUpdate<Parameter>
        where Parameter : notnull
        => ServerInvoke<APIPackUpdate, Interface>(serviceProvider, x => x.AddOrUpdate(parameter), successMessage);
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
    /// <param name="confirmMessage">在删除前进行确认的文本，它询问用户是否删除</param>
    /// <returns>一个布尔值，它指示是否删除成功</returns>
    /// <inheritdoc cref="ServerInvoke{APIPack, Interface}(IServiceProvider, Expression{Func{Interface, Task{APIPack}}}, string?)"/>
    public static async Task<APIPack> ServerDelete<Interface>(IEnumerable<Guid> ids,
        IServiceProvider serviceProvider, string? confirmMessage = null, string? successMessage = null)
        where Interface : class, IServerDelete<IEnumerable<Guid>>
    {
        var swalService = serviceProvider.GetRequiredService<SwalService>();
        if (!await swalService.ShowConfirm(confirmMessage ?? "确定要删除吗？"))
            return new()
            {
                FailureReason = "用户取消删除"
            };
        return await ServerInvoke<APIPack, Interface>(serviceProvider, x => x.Delete(ids), successMessage ?? "删除成功");
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
    /// <inheritdoc cref="ServerDelete{Interface}(IEnumerable{Guid}, IServiceProvider, string?, string?)"/>
    public static async Task<APIPack> ServerDelete<Interface, Obj>(Obj deleteObj,
        IServiceProvider serviceProvider, IList<Obj> list, string? confirmMessage = null, string? successMessage = null)
        where Interface : class, IServerDelete<IEnumerable<Guid>>
        where Obj : class, IWithID, IWithObjectID
    {
        var isNew = deleteObj.IsNew();
        #region 返回删除是否成功的委托
        async Task<APIPack> Delete()
        {
            if (isNew)
                return new();
            Guid[] id = [deleteObj.ID];
            return await ServerDelete<Interface>(id, serviceProvider, confirmMessage, successMessage);
        }
        #endregion
        var apiPack = await Delete();
        var isSuccess = apiPack.Success;
        if (isSuccess && isNew)
            list.RemoveAll(x => x.ObjectID == deleteObj.ObjectID);
        return apiPack;
    }
    #endregion
    #endregion
}
