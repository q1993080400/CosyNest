using System.NetFrancis.Http;
using System.Reflection;
using System.Text.Json;
using System.Web;

namespace System;

/// <summary>
/// 有关Razor的扩展方法全部放在这里
/// </summary>
public static partial class ExtendRazor
{
    #region 有关JS互操作
    #region 调用eval方法
    #region 说明文档
    /*问：JS函数很多，为什么需要为执行eval专门声明一个扩展方法？
      答：因为这个函数赋予了以字符串的方式执行任意JS代码的能力，
      这对于JS互操作来说非常重要且非常灵活，有必要予以特殊待遇*/
    #endregion
    #region 无返回值
    /// <summary>
    /// 通过JS互操作调用eval方法，
    /// 并执行JS代码
    /// </summary>
    /// <param name="js">执行JS代码的运行时</param>
    /// <param name="jsCode">要执行的JS代码</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    public static async ValueTask InvokeCodeVoidAsync(this IJSRuntime js, string jsCode, CancellationToken cancellation = default)
    {
        if (js is IJSInProcessRuntime jsInProcess)
            jsInProcess.InvokeVoid("InvokeCode", jsCode);
        else
            await js.InvokeVoidAsync("InvokeCode", cancellation, jsCode);
    }
    #endregion
    #region 有返回值
    /// <summary>
    /// 通过JS互操作调用eval方法，
    /// 并执行JS代码，然后返回返回值
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <returns></returns>
    /// <inheritdoc cref="InvokeCodeVoidAsync(IJSRuntime, string, CancellationToken)"/>
    public static async ValueTask<Ret> InvokeCodeAsync<Ret>(this IJSRuntime js, string jsCode, CancellationToken cancellation = default)
    {
        var finalCode = $"return {jsCode}";
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
        => CreateTask.AsyncProperty(
            cancellation => js.GetProperty<Obj>(property, cancellation).AsTask(),
            (value, cancellation) => js.SetProperty(property, value, cancellation).AsTask());
    #endregion
    #endregion
    #region 将Promise对象封装为Task
    /// <summary>
    /// 将一个JS中的Promise对象封装为Net熟悉的<see cref="Task"/>对象
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="jsRuntime">JS运行时对象</param>
    /// <param name="success">当Promise对象执行成功时，
    /// 通过这个函数将JS对象反序列化为Net对象</param>
    /// <param name="generateScript">这个委托的第一个参数是Promise执行成功时，执行的JS回调函数名称，
    /// 第二个参数是Promise执行失败时，执行的JS回调函数名称，返回值是要执行的JS脚本</param>
    /// <param name="fail">当Promise对象执行失败时，执行这个回调函数，
    /// 如果为<see langword="null"/>，直接返回默认值</param>
    /// <param name="cancellationToken">用于取消异步操作的令牌</param>
    /// <returns>当Promise对象执行成功时，返回<paramref name="success"/>的返回值，
    /// 执行失败时，返回<typeparamref name="Ret"/>的默认值</returns>
    internal static async Task<Ret?> AwaitPromise<Ret>(this IJSRuntime jsRuntime, Func<JsonElement, Ret?> success, Func<string, string, string> generateScript,
        Func<JsonElement, Ret?>? fail = null, CancellationToken cancellationToken = default)
    {
        var completionSource = new TaskCompletionSource<Ret?>();
        completionSource.SetCanceledAfter(cancellationToken);
        var document = new JSDocument(jsRuntime);
        var (successMethod, successDisposable) = await document.PackNetMethod<JsonElement>(x => completionSource.SetResult(success(x)), cancellation: cancellationToken);
        fail ??= _ => default;
        var (failMethod, failDisposable) = await document.PackNetMethod<JsonElement>(x => completionSource.SetResult(fail(x)), cancellation: cancellationToken);
        try
        {
            var script = generateScript(successMethod, failMethod);
            await jsRuntime.InvokeCodeVoidAsync(script, cancellation: cancellationToken);
            return await completionSource.Task;
        }
        finally
        {
            successDisposable.Dispose();
            failDisposable.Dispose();
        }
    }
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
    #endregion
    #region 有关组件
    #region 返回组件参数
    /// <summary>
    /// 读取一个组件的参数，然后返回
    /// </summary>
    /// <param name="component">要读取参数的组件</param>
    /// <returns></returns>
    public static IDictionary<string, object> GetParameters(this IComponent component)
    {
        var parameters = component.GetType().GetProperties().
            Where(x => x.IsDefined<ParameterAttribute>()).
            Select(x => (x.Name, x.GetValue(component))).ToArray();
        return parameters.Where(x => x.Item2 is { }).ToDictionary()!;
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
        var dictionary = parameters.ToDictionary().ToDictionary();
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
        var stateHasChanged = component.GetType().GetMethod("StateHasChanged", BindingFlags.NonPublic | BindingFlags.Instance)!;
        stateHasChanged.Invoke(component, null);
    }
    #endregion
    #region 合并渲染RenderFragment
    /// <summary>
    /// 返回一个新的<see cref="RenderFragment"/>，
    /// 它依次渲染集合中的所有<see cref="RenderFragment"/>
    /// </summary>
    /// <param name="sonRender">要渲染的<see cref="RenderFragment"/>集合</param>
    /// <returns></returns>
    public static RenderFragment MergeRender(this IEnumerable<RenderFragment> sonRender)
    {
        var newRender = sonRender.ToArray();
        return builder =>
        {
            foreach (var item in newRender)
            {
                item(builder);
            }
        };
    }
    #endregion
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
    #endregion 
}
