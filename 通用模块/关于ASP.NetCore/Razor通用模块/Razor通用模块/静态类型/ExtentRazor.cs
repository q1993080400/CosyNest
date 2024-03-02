using System.Net.Http.Headers;
using System.NetFrancis;
using System.NetFrancis.Http;
using System.Web;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace System;

/// <summary>
/// 有关Razor的扩展方法全部放在这里
/// </summary>
public static class ExtendRazor
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
    public static async ValueTask InvokeCodeVoidAsync(this IJSRuntime js, string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default)
    {
        var code = ToAsynchronous(jsCode, isAsynchronous);
        if (js is IJSInProcessRuntime jsInProcess)
            jsInProcess.InvokeVoid("InvokeCode", code);
        else
            await js.InvokeVoidAsync("InvokeCode", cancellation, code);
    }
    #endregion
    #region 有返回值
    /// <summary>
    /// 通过JS互操作调用eval方法，
    /// 并执行JS代码，然后返回返回值
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <returns></returns>
    /// <inheritdoc cref="InvokeCodeVoidAsync(IJSRuntime, string,bool, CancellationToken)"/>
    public static async ValueTask<Ret> InvokeCodeAsync<Ret>(this IJSRuntime js, string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default)
    {
        var code = ToAsynchronous(jsCode, isAsynchronous);
        #region 用来确定代码是否只有一行的本地函数
        static bool Fun(string code)
        {
            var pos = code.IndexOf(';');
            if (pos is -1)
                return true;
            return code.IndexOf(';', pos) is -1;
        }
        #endregion
        var finalCode = (Fun(code) ? "return " : "") + code;
        return js is IJSInProcessRuntime jsInProcess ?
            jsInProcess.Invoke<Ret>("InvokeCode", finalCode) :
            await js.InvokeAsync<Ret>("InvokeCode", cancellation, finalCode);
    }
    #endregion
    #region 将字符串转换为JS安全形式
    /// <summary>
    /// 将字符串转换为JS安全形式，
    /// 它通常在拼接JS代码时使用，
    /// 建议对所有可能由用户输入或控制的，
    /// 字符串形式的JS方法参数调用本方法
    /// </summary>
    /// <param name="text">待转换的字符串</param>
    /// <returns></returns>
    public static string ToJSSecurity(this string? text)
    {
        if (text is null)
            return "null";
        var encode = Convert.ToBase64String(HttpUtility.UrlEncode(text).ToBytes());
        return $"decodeURIComponent(atob(\"{encode}\"))";
    }

    /*本方法的目的在于：
      防止注入式攻击，或因为出现特殊字符，
      导致的JS语法错误*/
    #endregion
    #region 将布尔值转换为JS安全形式
    /// <summary>
    /// 将布尔值转换为JS安全形式，
    /// 它通常在拼接JS代码时使用
    /// </summary>
    /// <param name="bool">待转换的布尔值</param>
    /// <returns></returns>
    public static string ToJSSecurity(this bool @bool)
        => @bool ? "true" : "false";
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
    #region 注入IUriManager
    /// <summary>
    /// 以范围模式注入一个<see cref="IUriManager"/>，
    /// 它可以用于管理本机Uri，本服务依赖于<see cref="NavigationManager"/>，
    /// 适用于前端
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddUriManagerClient(this IServiceCollection services)
        => services.AddScoped(x =>
        {
            var navigationManager = x.GetRequiredService<NavigationManager>();
            return CreateNet.UriManager(navigationManager.BaseUri);
        });
    #endregion
    #region 注入携带Cookie的SignalRProvide对象
    /// <summary>
    /// 以瞬时模式注入一个<see cref="ISignalRProvide"/>，
    /// 它可以自动携带浏览器Cookie，
    /// 本服务依赖于<see cref="IUriManager"/>以及<see cref="IJSWindow"/>
    /// </summary>
    /// <param name="services">要添加服务的容器</param>
    /// <param name="withUri">这个委托被用来进一步执行Uri配置，
    /// 如果为<see langword="null"/>，则不执行</param>
    /// <returns></returns>
    /// <inheritdoc cref="CreateNet.ConfigureSignalRProvide(Func{IHubConnectionBuilder, Task{IHubConnectionBuilder}}?)"/>
    public static IServiceCollection AddSignalRProvideWithCookie(this IServiceCollection services,
        Action<HttpConnectionOptions>? withUri = null,
        Func<IHubConnectionBuilder, Task<IHubConnectionBuilder>>? configure = null)
        => services.AddSignalRProvide(async (uri, server) =>
        {
            var cookie = await server.GetRequiredService<IJSWindow>().Document.Cookie.ToListAsync();
            var host = server.GetRequiredService<IUriManager>().Uri.UriHost!.DomainName;
            var connection = new HubConnectionBuilder().
             WithUrl(uri, op =>
             {
                 foreach (var (key, value) in cookie.Where(x => !x.Key.IsVoid()))
                 {
                     op.Cookies.Add(new Net.Cookie(HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value), "/", host));
                 }
                 op.UseStatefulReconnect = true;
                 withUri?.Invoke(op);
             }).
             AddJsonProtocol(x => x.AddFormatterJson()).
             WithAutomaticReconnect();
            return (configure is null ? connection : await configure(connection)).Build();
        });
    #endregion
    #region 注入上传任务工厂
    /// <summary>
    /// 以单例模式注入一个<see cref="UploadTaskFactory{Progress}"/>，
    /// 它可以创建一个用于上传文件的任务
    /// </summary>
    /// <param name="services">要添加的服务容器</param>
    /// <returns></returns>
    /// <inheritdoc cref="UploadTaskFactory(IServiceProvider, UploadTaskFactoryInfo)"/>
    public static IServiceCollection AddUploadTaskFactory(this IServiceCollection services, UploadTaskFactoryInfo uploadTaskFactoryInfo)
        => services.AddSingleton(x => CreateRazor.UploadTaskFactory(x, uploadTaskFactoryInfo));
    #endregion
    #endregion
    #region 有关组件
    #region 将IBrowserFile集合转换为一个MultipartFormDataContent
    /// <summary>
    /// 将<see cref="IBrowserFile"/>集合转换为一个<see cref="MultipartFormDataContent"/>，
    /// 它可以用于后续向后端请求数据
    /// </summary>
    /// <param name="files">待转换的<see cref="IBrowserFile"/>集合</param>
    /// <param name="maxFileSize">单个文件的最大大小，超出会产生异常，默认为10M</param>
    /// <returns></returns>
    public static async Task<MultipartFormDataContent> ToFormDataContent(this IEnumerable<IBrowserFile> files, long maxFileSize = 1024 * 1024 * 10)
    {
        var content = new MultipartFormDataContent();
        foreach (var file in files)
        {
            using var fileStream = file.OpenReadStream(maxFileSize);
            var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Reset();
            var fileContent = new StreamContent(memoryStream);
            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "\"files\"", file.Name);
        }
        return content;
    }
    #endregion
    #region 返回组件参数
    /// <summary>
    /// 读取一个组件的参数，然后返回
    /// </summary>
    /// <param name="component">要读取参数的组件</param>
    /// <returns></returns>
    public static IDictionary<string, object> GetParameters(this IComponent component)
    {
        var parameters = component.GetType().GetProperties().
            Where(x => x.HasAttributes<ParameterAttribute>()).
            Select(x => (x.Name, x.GetValue(component))).ToArray();
        return parameters.Where(x => x.Item2 is { }).ToDictionary(true)!;
    }
    #endregion
    #region 关于ParameterView
    #region 重构ParameterView 
    /// <summary>
    /// 重构一个<see cref="ParameterView"/>，
    /// 将它的部分参数替换，然后返回一个新的<see cref="ParameterView"/>
    /// </summary>
    /// <param name="parameters">待重构的组件参数</param>
    /// <param name="reconfiguration">用来重构的委托，
    /// 它的参数是一个字典，复制了所有组件参数的名称和值</param>
    /// <returns></returns>
    public static ParameterView Reconfiguration(this ParameterView parameters, Action<IDictionary<string, object?>> reconfiguration)
    {
        var dictionary = parameters.ToDictionary().ToDictionary(true);
        reconfiguration(dictionary);
        return ParameterView.FromDictionary(dictionary!);
    }
    #endregion 
    #endregion
    #region 公开StateHasChanged方法
    /// <summary>
    /// 公开组件的StateHasChanged方法，并调用它
    /// </summary>
    /// <param name="component">要刷新的组件</param>
    public static void StateHasChanged(this ComponentBase component)
    {
        var stateHasChanged = component.GetTypeData().FindMethod("StateHasChanged");
        stateHasChanged.Invoke(component, null);
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
    #region 有关NavigationManager
    #region 导航到组件
    #region 泛型方法
    /// <typeparam name="Component">组件的类型</typeparam>
    /// <inheritdoc cref="NavigateToComponent(NavigationManager, Type, UriParameter?,bool)"/>
    public static void NavigateToComponent<Component>(this NavigationManager navigation, UriParameter? parameter = null, bool forceLoad = false)
        where Component : IComponent
        => navigation.NavigateToComponent(typeof(Component), parameter, forceLoad);
    #endregion
    #region 非泛型方法
    /// <summary>
    /// 导航到组件
    /// </summary>
    /// <param name="navigation">导航对象</param>
    /// <param name="componentType">组件的类型</param>
    /// <param name="parameter">组件路径的参数部分，
    /// 如果为<see langword="null"/>，表示没有参数</param>
    /// <param name="forceLoad">如果这个值为<see langword="true"/>，则绕过缓存强制刷新</param>
    public static void NavigateToComponent(this NavigationManager navigation, Type componentType, UriParameter? parameter = null, bool forceLoad = false)
    {
        var uri = new UriComplete()
        {
            UriExtend = ToolRazor.GetRouteSafe(componentType),
            UriParameter = parameter
        };
        navigation.NavigateTo(uri, forceLoad);
    }
    #endregion
    #endregion
    #region 一次性阻止导航
    /// <summary>
    /// 注册一个事件，它能够阻止用户进入其他导航，
    /// 但是与原生方法不同的是，它不会持续地锁定新的导航
    /// </summary>
    /// <param name="navigation">用来执行导航的对象</param>
    /// <param name="locationChangingHandler">当触发导航时的事件，
    /// 它返回一个布尔值，指示是否应该阻止导航</param>
    /// <returns>一个<see cref="IDisposable"/>，释放它可以停止阻止导航</returns>
    public static IDisposable RegisterLocationChangingHandlerDisposable(this NavigationManager navigation,
        Func<Task<bool>> locationChangingHandler)
        => new LocationChangingHandlerDisposable(navigation, locationChangingHandler);
    #endregion
    #endregion 
}
