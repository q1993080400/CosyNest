using System.NetFrancis.Api;

using BootstrapBlazor.Components;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 这个类型是有关BootstrapBlazor的扩展方法
/// </summary>
public static partial class ExtendBootstrapBlazor
{
    #region 关于MessageService
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
    #region 关于SwalService 
    #region 直接显示消息
    /// <summary>
    /// 直接显示一条消息框
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
    #region 直接显示一个确认框
    /// <summary>
    /// 直接显示一个确认框，并返回确认结果
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Show(SwalService, string, string, SwalCategory)"/>
    public static Task<bool> ShowConfirm(this SwalService swalService, string message, string title = "提示", SwalCategory category = SwalCategory.Question)
        => swalService.ShowModal(new SwalOption()
        {
            Content = message,
            Title = title,
            Category = category
        });
    #endregion
    #region 如果一个API响应失败，则弹出信息
    /// <summary>
    /// 如果一个API响应失败，
    /// 则弹出一个信息说明失败原因，
    /// 如果成功，则弹出响应成功的提示
    /// </summary>
    /// <param name="swalService">消息确认框服务</param>
    /// <param name="apiPack">API响应对象</param>
    /// <param name="successMessage">在操作成功时显示的弹窗消息，
    /// 如果为<see langword="null"/>，则使用一个默认消息</param>
    /// <returns>API是否响应成功</returns>
    public static async Task<bool> ShowIfFailure(this SwalService swalService, APIPack apiPack, string? successMessage = null)
    {
        var failureReason = apiPack.FailureReason;
        var (message, category) = failureReason is null ?
            (successMessage ?? "操作成功", SwalCategory.Success) :
            (failureReason, SwalCategory.Error);
        await swalService.Show(message, category: category);
        return apiPack.Success;
    }
    #endregion
    #endregion
    #region 关于依赖注入
    #region 注入IFileUploadNavigationContext
    /// <summary>
    /// 以范围模式注入一个<see cref="IFileUploadNavigationContext"/>，
    /// 并将其作为级联参数传递给所有组件，
    /// 当正在上传文件，且用户试图离开的时候，
    /// 会给予一个Bootstrap弹窗提示，询问用户是否离开
    /// </summary>
    /// <param name="services">待注入服务的容器</param>
    /// <returns></returns>
    public static IServiceCollection AddFileUploadNavigationContextPromptBootstrap(this IServiceCollection services)
        => services.AddCascadingValue(serviceProvider =>
        {
            var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
            return CreateRazor.FileUploadNavigationContext(navigationManager, async uploadFiles =>
            {
                var prompt = $"有{uploadFiles.Count}个文件还没有上传完毕，确定离开页面吗？";
                var swalService = serviceProvider.GetRequiredService<SwalService>();
                return !await swalService.ShowConfirm(prompt);
            });
        });
    #endregion
    #endregion
}
