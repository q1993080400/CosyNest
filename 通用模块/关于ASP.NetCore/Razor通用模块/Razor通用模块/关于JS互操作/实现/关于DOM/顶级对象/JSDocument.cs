using Microsoft.AspNetCore;

using System.IOFrancis.Bit;
using System.Text.Json;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="IJSDocument"/>的实现，
/// 可以视为一个JS中的Document对象
/// </summary>
sealed class JSDocument : JSRuntimeBase, IJSDocument
{
    #region 返回索引Cookie的字典
    private JSCookie? CookieField;

    public ICookie Cookie
        => CookieField ??= new(JSRuntime);
    #endregion
    #region 获取或设置标题
    private IAsyncProperty<string>? TitleField;

    public IAsyncProperty<string> Title
        => TitleField ??= JSRuntime.PackProperty<string>("document.title");
    #endregion
    #region 关于具有ID的控件
    #region 跳转到具有指定ID的元素
    public ValueTask ScrollIntoView(string id, CancellationToken cancellation = default)
         => JSRuntime.InvokeCodeVoidAsync($"document.getElementById('{id}').scrollIntoView()", cancellation: cancellation);
    #endregion
    #region 获取具有指定ID的元素
    public async ValueTask<IElementJS?> GetElementById(string id, CancellationToken cancellation = default)
    {
        var script = $"document.getElementById('{id}')==null";
        if (await JSRuntime.InvokeCodeAsync<bool>(script, false, cancellation))
            return null;
        return new Element(JSRuntime, id);
    }
    #endregion
    #region 获取具有焦点的元素
    public ValueTask<IElementJS?> ActiveElement
    {
        get
        {
            #region 本地函数
            async ValueTask<IElementJS?> Fun()
            {
                var id = await JSRuntime.InvokeCodeAsync<string>("document.activeElement.id");
                return id is null ? null : new Element(JSRuntime, id);
            }
            #endregion
            return Fun();
        }
    }
    #endregion
    #endregion
    #region video截图
    public async ValueTask<IBitRead?> VideoScreenshot(string id, string format = "png", CancellationToken cancellation = default)
    {
        var objName = ToolASP.CreateJSObjectName();
        var task = new ExplicitTask<IJSStreamReference>();
        var (methodName, freed) = await PackNetMethod<IJSStreamReference>(task.Completed, cancellation: cancellation);
        try
        {
            _ = Task.Run(async () =>
            {
                var script = $$"""
            var player = document.getElementById("{{id}}");  
            player.setAttribute("crossOrigin", "anonymous");  
            var canvas = document.createElement("canvas");
            canvas.width = player.clientWidth;
            canvas.height = player.clientHeight;
            canvas.getContext("2d").drawImage(player, 0, 0, canvas.width, canvas.height);
            canvas.toBlob(x=>
            {
                window.{{methodName}}(x);
            },"image/{{format}}");  
            """;
                await JSRuntime.InvokeCodeVoidAsync(script, cancellation: cancellation);
            }, cancellation);
            await using var jsStream = await task;
            if (jsStream is null)
                return null;
            using var stream = await jsStream.OpenReadStreamAsync(1024 * 1024 * 50, cancellation);
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellation);
            return memoryStream.ToBitPipe(format).Read;
        }
        finally
        {
            freed.Dispose();
        }
    }
    #endregion
    #region 关于JS调用Net方法
    #region 私有辅助类
    #region Net方法封装
    /// <summary>
    /// 这个类型封装了注册到JS运行时中的Net方法
    /// </summary>
    /// <typeparam name="Obj">方法的参数类型</typeparam>
    private sealed class NetMethodPack<Obj>
    {
        #region 封装的Net方法
        /// <summary>
        /// 获取事件中执行的Net方法
        /// </summary>
        private Action<Obj> Action { get; }
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
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="action">事件中执行的Net方法</param>
        public NetMethodPack(Action<Obj> action)
        {
            this.Action = action;
        }
        #endregion
    }
    #endregion
    #region 事件释放器
    /// <summary>
    /// 当这个对象被释放时，
    /// 它也会将JS事件解除注册
    /// </summary>
    private sealed class JSEventFreed : IDisposable
    {
        #region 公开成员
        public void Dispose()
        {
            if (Disposable is null)
                return;
            Disposable.Dispose();
            Disposable = null;
            _ = Task.Run(async () => await JSRuntime.InvokeCodeVoidAsync(Script));
        }
        #endregion
        #region 内部成员
        #region 释放时执行的脚本
        /// <summary>
        /// 获取释放时执行的JS脚本
        /// </summary>
        private string Script { get; }
        #endregion
        #region 待释放的对象
        /// <summary>
        /// 获取待释放的Net对象封装
        /// </summary>
        private IDisposable? Disposable { get; set; }
        #endregion
        #region JS运行时
        /// <summary>
        /// 获取封装的JS运行时，
        /// 本对象通过它执行脚本
        /// </summary>
        private IJSRuntime JSRuntime { get; }
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="script">释放时执行的JS脚本</param>
        /// <param name="disposable">待释放的Net对象封装</param>
        /// <param name="jSRuntime">封装的JS运行时，本对象通过它执行脚本</param>
        public JSEventFreed(string script, IDisposable disposable, IJSRuntime jSRuntime)
        {
            Script = script;
            Disposable = disposable;
            JSRuntime = jSRuntime;
        }
        #endregion
    }
    #endregion
    #endregion
    #region 为JS对象添加Net事件
    public async ValueTask<IDisposable> AddEvent(string? id, string eventName, Action<JsonElement> action, Func<string, IEnumerable<string>>? getParameter = null, CancellationToken cancellation = default)
    {
        var (methodName, freed) = await PackNetMethod(action, cancellation: cancellation);
        const string eventParameter = "event";
        var parameter = getParameter is null ? eventParameter : getParameter(eventParameter).Join(",");
        var eventMethodName = ToolASP.CreateJSObjectName();
        var script = $"window.{eventMethodName}=function({eventParameter}){{window.{methodName}([{parameter}])}};";
        var targetName = "document";
        if (id is { })
            targetName += $".getElementById('{id}')";
        script += $@"{targetName}.addEventListener('{eventName}',{eventMethodName});";
        await JSRuntime.InvokeCodeVoidAsync(script, cancellation: cancellation);
        var freedScript = $"{targetName}.removeEventListener('{eventName}',{eventMethodName};";
        return new JSEventFreed(freedScript, freed, JSRuntime);
    }
    #endregion
    #region 将Net方法注册为JS方法
    #region 正式类型
    public ValueTask<(string MethodName, IDisposable Freed)> PackNetMethod<Obj>(Action<Obj> action, string? methodName = null, CancellationToken cancellation = default)
        => action switch
        {
            Action<JsonElement> a => PackNetMethodJson(a, methodName, cancellation),
            Action<IJSStreamReference> a => PackNetMethodStream(a, methodName, cancellation),
            null => throw new ArgumentNullException(nameof(action)),
            _ => throw new NotSupportedException($"本方法的的泛型参数只支持{nameof(JsonElement)}或{nameof(IJSStreamReference)}")
        };
    #endregion
    #region 方法参数为JsonElement
    /// <inheritdoc cref="IJSDocument.PackNetMethod{Obj}(Action{Obj}, string?, CancellationToken)"/>
    private async ValueTask<(string MethodName, IDisposable Freed)> PackNetMethodJson(Action<JsonElement> action, string? methodName = null, CancellationToken cancellation = default)
    {
        var springboard = ToolASP.CreateJSObjectName();
        methodName ??= ToolASP.CreateJSObjectName();
        var script = $$"""
            window.{{springboard}}=
            function(net)
            {
                window.{{methodName}}=function(parameter)
                {
                    net.invokeMethodAsync('{{nameof(NetMethodPack<JsonElement>.Invoke)}}',parameter);
                }
            }
""";
        var netMethodPack = DotNetObjectReference.Create(new NetMethodPack<JsonElement>(action));
        await JSRuntime.InvokeCodeVoidAsync(script, cancellation: cancellation);
        await JSRuntime.InvokeVoidAsync(springboard, cancellation, netMethodPack);
        return (methodName, netMethodPack);
    }
    #endregion
    #region 
    /// <inheritdoc cref="IJSDocument.PackNetMethod{Obj}(Action{Obj}, string?, CancellationToken)"/>
    private async ValueTask<(string MethodName, IDisposable Freed)> PackNetMethodStream(Action<IJSStreamReference> action, string? methodName = null, CancellationToken cancellation = default)
    {
        var streamName = ToolASP.CreateJSObjectName();
        var springboard = ToolASP.CreateJSObjectName();
        methodName ??= ToolASP.CreateJSObjectName();
        var script = $$"""
            window.{{springboard}}=
            function(net)
            {
                window.{{methodName}}=function(parameter)
                {
                    window.{{streamName}}=parameter;
                    net.invokeMethodAsync('{{nameof(NetMethodPack<JsonElement>.Invoke)}}',null);
                }
            }
""";
        var netMethodPack = DotNetObjectReference.Create(new NetMethodPack<JsonElement>(async _ =>
        {
            var stream = await JSRuntime.InvokeCodeAsync<IJSStreamReference>(streamName);
            action(stream);
        }));
        await JSRuntime.InvokeCodeVoidAsync(script, cancellation: cancellation);
        await JSRuntime.InvokeVoidAsync(springboard, cancellation, netMethodPack);
        return (methodName, netMethodPack);
    }
    #endregion
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSDocument(IJSRuntime jsRuntime)
        : base(jsRuntime)
    {

    }
    #endregion
}
