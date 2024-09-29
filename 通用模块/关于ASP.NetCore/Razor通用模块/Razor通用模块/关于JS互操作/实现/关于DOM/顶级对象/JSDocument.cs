using System.Text.Json;

using Microsoft.AspNetCore;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="IJSDocument"/>的实现，
/// 可以视为一个JS中的Document对象
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
sealed class JSDocument(IJSRuntime jsRuntime) : IJSDocument
{
    #region 返回索引Cookie的字典
    public ICookie Cookie { get; }
        = new JSCookie(jsRuntime);
    #endregion
    #region 获取或设置标题
    public IAsyncProperty<string> Title { get; }
        = jsRuntime.PackProperty<string>("document.title");
    #endregion
    #region 返回页面的可见状态
    public Task<VisibilityState> VisibilityState
    {
        get
        {
            #region 本地函数
            async Task<VisibilityState> Fun()
            {
                var visibilityState = await jsRuntime.InvokeCodeAsync<string>("document.visibilityState");
                return visibilityState switch
                {
                    "visible" => JSInterop.VisibilityState.Visible,
                    "hidden" => JSInterop.VisibilityState.Hidden,
                    "prerender" => JSInterop.VisibilityState.Prerender,
                    var state => throw new NotSupportedException($"{state}是无法识别的页面可见性状态")
                };
            }
            #endregion
            return Fun();
        }
    }
    #endregion
    #region 将Net方法注册为JS方法
    #region 正式方法
    public ValueTask<(string MethodName, IDisposable Freed)> PackNetMethod<Obj>(Action<Obj> action, string? jsMethodName = null, CancellationToken cancellation = default)
        => action switch
        {
            Action<JsonElement> a => PackNetMethodJson(a, jsMethodName, cancellation),
            Action<IJSStreamReference> a => PackNetMethodStream(a, jsMethodName, cancellation),
            null => throw new ArgumentNullException(nameof(action)),
            _ => throw new NotSupportedException($"本方法的的泛型参数只支持{nameof(JsonElement)}或{nameof(IJSStreamReference)}")
        };
    #endregion
    #region 辅助方法
    #region 参数为JsonElement
    /// <inheritdoc cref="IJSDocument.PackNetMethod{Obj}(Action{Obj}, string?, CancellationToken)"/>
    private async ValueTask<(string MethodName, IDisposable Freed)> PackNetMethodJson(Action<JsonElement> action, string? jsMethodName = null, CancellationToken cancellation = default)
    {
        var springboard = CreateASP.JSObjectName();
        var newJSMethodName = jsMethodName ?? CreateASP.JSObjectName();
        var netMethodPack = DotNetObjectReference.Create(new NetMethodPack<JsonElement>(action));
        await jsRuntime.InvokeVoidAsync("RegisterNetMethod",
            cancellation, netMethodPack, newJSMethodName, nameof(NetMethodPack<JsonElement>.Invoke));
        return (newJSMethodName, netMethodPack);
    }
    #endregion
    #region 方法参数为Stream
    /// <inheritdoc cref="IJSDocument.PackNetMethod{Obj}(Action{Obj}, string?, CancellationToken)"/>
    private async ValueTask<(string MethodName, IDisposable Freed)> PackNetMethodStream(Action<IJSStreamReference> action, string? jsMethodName = null, CancellationToken cancellation = default)
    {
        var streamName = CreateASP.JSObjectName();
        var springboard = CreateASP.JSObjectName();
        jsMethodName ??= CreateASP.JSObjectName();
        var script = $$"""
            window.{{springboard}}=
            function(net)
            {
                window.{{jsMethodName}}=function(parameter)
                {
                    window.{{streamName}}=parameter;
                    net.invokeMethodAsync('{{nameof(NetMethodPack<JsonElement>.Invoke)}}',null);
                }
            }
""";
        var netMethodPack = DotNetObjectReference.Create(new NetMethodPack<JsonElement>(async _ =>
        {
            var stream = await jsRuntime.InvokeCodeAsync<IJSStreamReference>(streamName);
            action(stream);
        }));
        await jsRuntime.InvokeCodeVoidAsync(script, cancellation: cancellation);
        await jsRuntime.InvokeVoidAsync(springboard, cancellation, netMethodPack);
        return (jsMethodName, netMethodPack);
    }
    #endregion
    #endregion
    #region 私有辅助类
    /// <summary>
    /// 这个类型封装了注册到JS运行时中的Net方法
    /// </summary>
    /// <typeparam name="Obj">方法的参数类型</typeparam>
    /// <remarks>
    /// 使用指定的参数初始化对象
    /// </remarks>
    /// <param name="action">事件中执行的Net方法</param>
    private sealed class NetMethodPack<Obj>(Action<Obj> action)
    {
        #region 封装的Net方法
        /// <summary>
        /// 获取事件中执行的Net方法
        /// </summary>
        private Action<Obj> Action { get; } = action;
        #endregion
        #region 执行Net方法
        /// <summary>
        /// 执行Net方法
        /// </summary>
        /// <param name="obj">方法的参数，
        /// 这些参数会被反序列化并传给底层的委托，
        /// 请从JS调用本方法，不要从Net调用本方法</param>
        [JSInvokable]
        public void Invoke(Obj obj)
        {
            Action(obj);
        }
        #endregion
    }
    #endregion
    #endregion
}
