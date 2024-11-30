using System.Reflection;

using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明有关配置的扩展方法

    #region 配置SignalR
    #region 注册所有SignalR中心
    /// <summary>
    /// 注册一个程序集中的所有SignalR中心
    /// </summary>
    /// <param name="builder">执行注册的路由生成器协定</param>
    /// <param name="assembly">SignalR中心所在的程序集，
    /// 该程序集中所有继承自<see cref="Hub"/>的公开类型都会被注册</param>
    /// <param name="pattern">用来获取路由模板的委托，
    /// 它的参数是继承自<see cref="Hub"/>的类型名称，返回值是路由模板，
    /// 如果为<see langword="null"/>，则默认路由模式为/Hub/{TypeName}，
    /// 如果存在，它还会删除类型名称末尾的Hub后缀</param>
    public static void MapHubAll(this IEndpointRouteBuilder builder, Assembly assembly, Func<string, string>? pattern = null)
    {
        pattern ??= static x => $"/Hub/{x.Trim(false, "Hub")}";
        var method = typeof(HubEndpointRouteBuilderExtensions).GetMethod(
            nameof(HubEndpointRouteBuilderExtensions.MapHub),
            [typeof(IEndpointRouteBuilder), typeof(string), typeof(Action<HttpConnectionDispatcherOptions>)])!;
        foreach (var item in assembly.GetExportedTypes().Where(static x => typeof(Hub).IsAssignableFrom(x) && !x.IsAbstract))
        {
            method.MakeGenericMethod(item).Invoke<object>(null, builder, pattern(item.Name), static (HttpConnectionDispatcherOptions options) =>
            {
                options.AllowStatefulReconnects = true;
            });
        }
    }
    #endregion
    #endregion
    #region 配置约定
    #region 返回模型绑定成功的Task
    /// <summary>
    /// 将模型配置为绑定成功，
    /// 并返回绑定成功的<see cref="Task"/>
    /// </summary>
    /// <param name="bindingContext">待配置模型绑定失败的约定</param>
    /// <param name="model">要绑定的模型</param>
    /// <returns></returns>
    public static Task BindingSuccess(this ModelBindingContext bindingContext, object? model)
    {
        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
    #endregion 
    #region 返回模型绑定失败的Task
    /// <summary>
    /// 将模型配置为绑定失败，
    /// 并返回绑定失败的<see cref="Task"/>
    /// </summary>
    /// <param name="bindingContext">待配置模型绑定失败的约定</param>
    /// <returns></returns>
    public static Task BindingFailed(this ModelBindingContext bindingContext)
    {
        bindingContext.Result = ModelBindingResult.Failed();
        return Task.CompletedTask;
    }
    #endregion
    #endregion
}
