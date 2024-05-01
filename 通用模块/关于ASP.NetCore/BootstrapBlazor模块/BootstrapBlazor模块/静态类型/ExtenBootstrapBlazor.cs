using System.NetFrancis.Api;

using BootstrapBlazor.Components;

namespace System;

/// <summary>
/// 这个类型是有关BootstrapBlazor的扩展方法
/// </summary>
public static class ExtenBootstrapBlazor
{
    #region 有关MessageService
    #region 直接显示消息
    /// <summary>
    /// 直接显示一条消息，在几秒钟后会消失
    /// </summary>
    /// <param name="messageService">消息服务</param>
    /// <param name="message">要显示的消息</param>
    /// <returns></returns>
    public static Task Show(this MessageService messageService, string message)
        => messageService.Show(new()
        {
            Content = message
        });
    #endregion
    #endregion
    #region 有关SwalService 
    #region 直接显示消息
    /// <summary>
    /// 直接显示一条消息确认框
    /// </summary>
    /// <param name="swalService">消息确认框服务</param>
    /// <param name="message">要显示的消息</param>
    /// <param name="title">消息的标题</param>
    /// <param name="category">消息框的类型</param>
    /// <returns></returns>
    public static Task Show(this SwalService swalService, string message, string title = "提示", SwalCategory category = SwalCategory.Error)
        => swalService.Show(new()
        {
            Content = message,
            Title = title,
            Category = category
        });
    #endregion
    #region 如果一个API响应失败，则弹出信息
    /// <summary>
    /// 如果一个API响应失败，
    /// 则弹出一个信息说明失败原因
    /// </summary>
    /// <param name="swalService">消息确认框服务</param>
    /// <param name="apiPack">API响应对象</param>
    /// <returns>API是否响应成功</returns>
    public static async Task<bool> ShowIfFailure(this SwalService swalService, APIPack apiPack)
    {
        if (!apiPack.IsSuccess)
            await swalService.Show(apiPack.FailureReason!);
        return apiPack.IsSuccess;
    }
    #endregion
    #endregion
}
