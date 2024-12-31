namespace System;

public static partial class ExtendRazor
{
    //这个部分类专门用来声明有关JS互操作的扩展方法

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
    #endregion
    #region 关于JS属性
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
    #region 关于依赖注入
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
}
