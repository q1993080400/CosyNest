using System.NetFrancis;
using System.Reflection;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace System;

/// <summary>
/// 有关Razor的扩展方法全部放在这里
/// </summary>
public static class ExtenRazor
{
    #region 有关JS互操作
    #region 调用eval方法
    #region 说明文档
    /*问：JS函数很多，为什么需要为执行eval专门声明一个扩展方法？
      答：因为这个函数赋予了以字符串的方式执行任意JS代码的能力，
      这对于JS互操作来说非常重要且非常灵活，有必要予以特殊待遇*/
    #endregion
    #region 视情况将JS代码转换为异步代码
    /// <summary>
    /// 视情况将JS代码转换为异步JS代码
    /// </summary>
    /// <param name="code">要转换的JS代码</param>
    /// <param name="isAsynchronous">如果这个值为<see langword="true"/>，
    /// 会将<paramref name="code"/>转换为等待异步代码的JS代码，否则将其原路返回</param>
    /// <returns></returns>
    private static string ToAsynchronous(string code, bool isAsynchronous)
        => isAsynchronous ? $"Promise.any([{code}])" : code;
    #endregion
    #region 无返回值
    /// <summary>
    /// 通过JS互操作调用eval方法，
    /// 并执行JS代码
    /// </summary>
    /// <param name="js">执行JS代码的运行时</param>
    /// <param name="jsCode">要执行的JS代码</param>
    /// <param name="isAsynchronous">如果这个值为<see langword="true"/>，
    /// 表示该JS代码是异步代码，且需要等待，会对脚本进行一些特殊处理和转换</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    public static ValueTask InvokeCodeVoidAsync(this IJSRuntime js, string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default)
        => js.InvokeVoidAsync("eval", cancellation, ToAsynchronous(jsCode, isAsynchronous));
    #endregion
    #region 有返回值
    /// <summary>
    /// 通过JS互操作调用eval方法，
    /// 并执行JS代码，然后返回返回值
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <returns></returns>
    /// <inheritdoc cref="InvokeCodeVoidAsync(IJSRuntime, string,bool, CancellationToken)"/>
    public static ValueTask<Ret> InvokeCodeAsync<Ret>(this IJSRuntime js, string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default)
        => js.InvokeAsync<Ret>("eval", cancellation, ToAsynchronous(jsCode, isAsynchronous), jsCode);
    #endregion
    #endregion
    #region 有关JS属性
    #region 读取属性
    /// <summary>
    /// 读取一个JS属性
    /// </summary>
    /// <typeparam name="Property">属性的类型</typeparam>
    /// <param name="js">JS运行时对象</param>
    /// <param name="property">用来访问属性的表达式，它相对于Window对象</param>
    /// <returns></returns>
    public static ValueTask<Property> GetProperty<Property>(this IJSRuntime js, string property, CancellationToken cancellation = default)
        => js.InvokeCodeAsync<Property>(property, cancellation: cancellation);
    #endregion
    #region 写入属性
    /// <summary>
    /// 写入一个JS属性
    /// </summary>
    /// <param name="value">待写入的属性的值</param>
    /// <returns></returns>
    /// <inheritdoc cref="GetProperty{Property}(IJSRuntime, string, CancellationToken)"/>
    public static ValueTask SetProperty(this IJSRuntime js, string property, object? value, CancellationToken cancellation = default)
    {
        value = value switch
        {
            string s => $"\"{s}\"",
            null => "null",
            var v => v
        };
        return js.InvokeCodeVoidAsync($"{property}={value}", cancellation: cancellation);
    }
    #endregion
    #region 封装JS异步属性
    /// <summary>
    /// 将一个JS属性封装为异步属性
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetProperty{Property}(IJSRuntime, string, CancellationToken)"/>
    public static IAsyncProperty<Obj> PackProperty<Obj>(this IJSRuntime js, string property)
        => CreateTasks.AsyncProperty(
            cancellation => js.GetProperty<Obj>(property, cancellation).AsTask(),
            (value, cancellation) => js.SetProperty(property, value, cancellation).AsTask());
    #endregion
    #endregion
    #endregion
    #region 关于依赖注入
    #region 注入前端对象
    /// <summary>
    /// 向服务容器注入常用前端对象
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddFront(this IServiceCollection services)
    {
        services.AddJSWindow();
        return services;
    }
    #endregion
    #region 注入IJSWindow
    /// <summary>
    /// 向服务容器注入一个<see cref="IJSWindow"/>
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddJSWindow(this IServiceCollection services)
        => services.AddScoped<IJSWindow, JSWindow>();
    #endregion
    #region 注入ISignalRProvide
    /// <summary>
    /// 以暂时模式注册一个<see cref="ISignalRProvide"/>服务
    /// </summary>
    /// <param name="services">要注册的服务容器</param>
    /// <returns></returns>
    /// <inheritdoc cref="CreateNet.SignalRProvide(Func{string, HubConnection}?, Func{string, string}?)"/>
    public static IServiceCollection AddSignalRProvide(this IServiceCollection services, Func<string, HubConnection>? create = null)
        => services.AddTransient
        (x => CreateNet.SignalRProvide(create,
            uri => x.GetRequiredService<NavigationManager>().ToAbsoluteUri(uri).AbsoluteUri));
    #endregion
    #region 注入IHttpClient
    /// <summary>
    /// 注入一个<see cref="IHttpClient"/>，
    /// 它可以用于请求WebApi
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <param name="baseAddress">请求的基地址，它通常是服务器的域名</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClient(this IServiceCollection services, string baseAddress)
    {
        services.AddHttpClient("webapi", client => client.BaseAddress = new(baseAddress));
        var a = services.AddScoped(server => server.GetRequiredService<IHttpClientFactory>().CreateClient("webapi").ToHttpClient());
        return services;
    }
    #endregion
    #endregion
    #region 有关组件
    #region 公开StateHasChanged方法
    private static MethodInfo StateHasChangedMethod { get; }
    = typeof(ComponentBase).GetTypeData().FindMethod("StateHasChanged");

    /// <summary>
    /// 调用一个<see cref="ComponentBase"/>的StateHasChanged方法
    /// </summary>
    /// <param name="component">要调用方法的<see cref="ComponentBase"/></param>
    public static void StateHasChanged(this ComponentBase component)
        => StateHasChangedMethod.Invoke<object>(component);
    #endregion
    #region 返回支持同步读取上传文件的流
    /// <summary>
    /// 读取一个待上传的文件，并返回一个支持同步读取的流，
    /// 只支持10M以内的文件
    /// </summary>
    /// <param name="file">用于读取上传文件的对象</param>
    /// <returns></returns>
    public static async Task<Stream> OpenSynchronizeReadStream(this IBrowserFile file)
    {
        using var stream = file.OpenReadStream(1024 * 1024 * 10);
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Reset();
        return memoryStream;
    }
    #endregion
    #endregion
    #region 返回是否被验证
    /// <summary>
    /// 返回是否验证成功
    /// </summary>
    /// <param name="authenticationStateProvider">用于提供验证的服务</param>
    /// <returns></returns>
    public static async Task<bool> IsAuthenticated(this AuthenticationStateProvider authenticationStateProvider)
        => (await authenticationStateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated();
    #endregion
}
