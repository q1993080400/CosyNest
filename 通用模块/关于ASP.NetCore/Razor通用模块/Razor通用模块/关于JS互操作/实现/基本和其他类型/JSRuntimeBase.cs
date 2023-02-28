using System.Text.Json;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个抽象类是所有JS对象封装的基类
/// </summary>
abstract class JSRuntimeBase
{
    #region 封装的JS运行时
    /// <summary>
    /// 获取封装的JS运行时对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected IJSRuntime JSRuntime { get; }
    #endregion
    #region 将Promise对象封装为Task
    /// <summary>
    /// 将一个JS中的Promise对象封装为Net熟悉的<see cref="Task"/>对象
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="success">当Promise对象执行成功时，
    /// 通过这个函数将JS对象反序列化为Net对象</param>
    /// <param name="generateScript">这个委托的第一个参数是Promise执行成功时，执行的JS回调函数名称，
    /// 第二个参数是Promise执行失败时，执行的JS回调函数名称，返回值是要执行的JS脚本</param>
    /// <returns>当Promise对象执行成功时，返回<paramref name="success"/>的返回值，
    /// 执行失败时，返回<typeparamref name="Ret"/>的默认值</returns>
    protected async Task<Ret?> AwaitPromise<Ret>(Func<JsonElement, Ret?> success, Func<string, string, string> generateScript, CancellationToken cancellationToken = default)
    {
        var task = new ExplicitTask<Ret?>();
        var document = new JSDocument(JSRuntime);
        var (successMethod, successDisposable) = await document.PackNetMethod<JsonElement>(x =>
        {
            var result = success(x);
            task.Completed(result);
        }, cancellation: cancellationToken);
        var (failMethod, failDisposable) = await document.PackNetMethod<JsonElement>(_ => task.Completed(default), cancellation: cancellationToken);
        try
        {
            var script = generateScript(successMethod, failMethod);
            await JSRuntime.InvokeCodeVoidAsync(script, cancellation: cancellationToken);
            return await task;
        }
        finally
        {
            successDisposable.Dispose();
            failDisposable.Dispose();
        }
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的JS运行时初始化对象
    /// </summary>
    /// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
    public JSRuntimeBase(IJSRuntime jsRuntime)
    {
        this.JSRuntime = jsRuntime;
    }
    #endregion
}
