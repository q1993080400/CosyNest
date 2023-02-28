using System.Design;
using System.Diagnostics.CodeAnalysis;
using System.Maths;
using System.NetFrancis;
using System.NetFrancis.Http;
using System.Text.Json;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="IJSWindow"/>的实现，
/// 可以视为一个Window对象
/// </summary>
sealed class JSWindow : JSRuntimeBase, IJSWindow
{
    #region 返回Document对象
    private IJSDocument? DocumentField;

    public IJSDocument Document
        => DocumentField ??= new JSDocument(JSRuntime);
    #endregion
    #region 关于硬件
    #region 返回屏幕对象
    private JSScreen? ScreenField;

    public Task<IJSScreen> Screen => Task.Run(async () =>
    {
        if (ScreenField is { })
            return ScreenField;
        var width = await JSRuntime.InvokeCodeAsync<int>("innerWidth");
        var height = await JSRuntime.InvokeCodeAsync<int>("innerHeight");
        ScreenField = new()
        {
            Resolution = CreateMath.SizePixel(width, height),
            DevicePixelRatio = await JSRuntime.InvokeCodeAsync<double>("devicePixelRatio")
        };
        return (IJSScreen)ScreenField;
    });
    #endregion
    #region 返回Navigator对象
    private IJSNavigator? NavigatorField;

    public IJSNavigator Navigator
        => NavigatorField ??= new JSNavigator(JSRuntime);
    #endregion
    #endregion
    #region 关于存储
    #region 返回本地存储对象
    private JSLocalStorage? LocalStorageField;

    public IAsyncDictionary<string, string> LocalStorage
         => LocalStorageField ??= new(JSRuntime);
    #endregion
    #endregion
    #region 返回Location对象
    private JSLocation? LocationField;

    public IJSLocation Location
        => LocationField ??= new(JSRuntime);
    #endregion
    #region 关于弹窗
    #region 弹出确认窗
    public ValueTask<bool> Confirm(string message, CancellationToken cancellation = default)
          => JSRuntime.InvokeAsync<bool>("confirm", cancellation, message);
    #endregion
    #region 弹出消息窗
    public ValueTask Alert(string message, CancellationToken cancellation = default)
           => JSRuntime.InvokeVoidAsync("alert", cancellation, message);
    #endregion
    #region 弹出输入框
    public ValueTask<string?> Prompt(string? text = null, string? value = null, CancellationToken cancellation = default)
        => JSRuntime.InvokeAsync<string?>("prompt", cancellation, text, value);
    #endregion
    #region 打印窗口
    public ValueTask Print(CancellationToken cancellation = default)
          => JSRuntime.InvokeVoidAsync("print", cancellation);
    #endregion
    #endregion
    #region 关于打开与关闭窗口
    #region 打开新窗口
    public ValueTask Open(string strUrl, string strWindowName, string? strWindowFeatures = null, CancellationToken cancellation = default)
        => JSRuntime.InvokeVoidAsync("open", cancellation, strUrl, strWindowName, strWindowFeatures);
    #endregion
    #region 关闭窗口
    public ValueTask Close(CancellationToken cancellation = default)
         => JSRuntime.InvokeVoidAsync("close", cancellation);
    #endregion
    #endregion
    #region 直接执行JS代码
    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args)
        => JSRuntime.InvokeAsync<TValue>(identifier, args);

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        => JSRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);
    #endregion
    #region 通过JS发起Post请求
    public async ValueTask<Ret?> FetchPost<Ret>(UriComplete uri, object? parameter, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= CreateDesign.JsonCommonOptions;
        return await AwaitPromise(x => JsonSerializer.Deserialize<Ret>(x, options),
                 (successMethod, failMethod) =>
                 {
                     var json = JsonSerializer.Serialize(parameter, parameter?.GetType() ?? typeof(object), options);
                     var script = $$"""
                        var myInit = 
                        { 
                            method: 'post',
                            body: {{json.ToJSSecurity()}},
                            credentials:'same-origin',
                            headers: 
                            {
                                'Content-Type': '{{MediaTypeName.Json}}'
                            }
                        };
                        fetch('{{uri}}',myInit).
                            then(x=>x.json()).
                            then(x=>{{successMethod}}(x)).
                            catch({{failMethod}});
                """;
                     return script;
                 }, cancellationToken);
    }
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSWindow(IJSRuntime jsRuntime)
        : base(jsRuntime)
    {

    }
    #endregion
}
