using System.Reflection;

using Microsoft.AspNetCore.Authorization;
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
        pattern ??= x => $"/Hub/{x.Trim(false, "Hub")}";
        var method = typeof(HubEndpointRouteBuilderExtensions).GetMethod(
            nameof(HubEndpointRouteBuilderExtensions.MapHub),
            [typeof(IEndpointRouteBuilder), typeof(string), typeof(Action<HttpConnectionDispatcherOptions>)])!;
        foreach (var item in assembly.GetExportedTypes().Where(x => typeof(Hub).IsAssignableFrom(x) && !x.IsAbstract))
        {
            method.MakeGenericMethod(item).Invoke<object>(null, builder, pattern(item.Name), (HttpConnectionDispatcherOptions options) =>
            {
                options.AllowStatefulReconnects = true;
            });
        }
    }
    #endregion
    #endregion
    #region 配置AuthorizationOptions
    #region 通过静态方法验证
    /// <summary>
    /// 通过约定先于配置的方式，
    /// 为授权策略配置验证方式
    /// </summary>
    /// <param name="options">待配置的授权策略</param>
    /// <param name="policyValidation">这个类型中所有返回<see cref="bool"/>，
    /// 有且仅有一个类型为<see cref="AuthorizationHandlerContext"/>的公开静态方法的名称会作为策略的名称，
    /// 方法作为策略的验证方法，并将其添加到授权策略中</param>
    public static void AddPolicyValidation(this AuthorizationOptions options, Type policyValidation)
    {
        var methods = policyValidation.GetMethods(BindingFlags.Public | BindingFlags.Static).
            Where(x => x.IsSame(typeof(bool), [typeof(AuthorizationHandlerContext)])).
            ToDictionary(x => x.Name, x => x);
        foreach (var (name, method) in methods)
        {
            var fun = method.CreateDelegate<Func<AuthorizationHandlerContext, bool>>();
            options.AddPolicy(name, x => x.RequireAssertion(fun));
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
