using System.NetFrancis.Http;

using Microsoft.AspNetCore;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个静态类声明了一些为Bootstrap特化的增删改查方法
/// </summary>
public static class ToolBootstrapCRUD
{
    #region 删除对象
    /// <summary>
    /// 执行删除方法，
    /// 并弹窗提示是否成功
    /// </summary>
    /// <typeparam name="Interface">服务端删除接口的类型</typeparam>
    /// <param name="httpClient">发起请求的Http客户端对象</param>
    /// <param name="ids">要删除的元素的ID</param>
    /// <param name="swalService">用于弹窗提示的对象</param>
    /// <param name="refresh">在删除成功后，执行这个委托以刷新组件</param>
    /// <param name="prompt">如果这个值为<see langword="true"/>，则在删除前先提示</param>
    /// <returns></returns>
    public static async Task<bool> Delete<Interface>(IHttpClient httpClient,
        IEnumerable<Guid> ids, SwalService swalService,
        Func<Task> refresh, bool prompt = true)
        where Interface : class, IServerDelete<IEnumerable<Guid>>
    {
        if (prompt)
        {
            var confirm = await swalService.ShowModal("确定要删除吗？");
            if (!confirm)
                return false;
        }
        var response = await httpClient.StrongType<Interface>().
            Request(x => x.Delete(ids));
        if (await swalService.ShowIfFailure(response))
        {
            await swalService.Show("删除成功", category: SwalCategory.Success);
            await refresh();
            return true;
        }
        return false;
    }
    #endregion
}
