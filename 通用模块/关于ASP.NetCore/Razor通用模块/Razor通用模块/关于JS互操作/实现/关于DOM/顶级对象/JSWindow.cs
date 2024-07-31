﻿using System.Design;
using System.Diagnostics.CodeAnalysis;
using System.MathFrancis;
using System.NetFrancis;
using System.NetFrancis.Http;
using System.Text.Json;
using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="IJSWindow"/>的实现，
/// 可以视为一个Window对象
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
sealed class JSWindow(IJSRuntime jsRuntime) : IJSWindow
{
    #region 返回Document对象
    public IJSDocument Document { get; } = new JSDocument(jsRuntime);
    #endregion
    #region 关于硬件
    #region 返回屏幕对象
    private JSScreen? ScreenField;

    public Task<IJSScreen> Screen => Task.Run(async () =>
    {
        if (ScreenField is { })
            return ScreenField;
        var width = await jsRuntime.InvokeCodeAsync<int>("innerWidth");
        var height = await jsRuntime.InvokeCodeAsync<int>("innerHeight");
        ScreenField = new()
        {
            LogicalResolution = CreateMath.SizePixel(width, height),
            DevicePixelRatio = await jsRuntime.InvokeCodeAsync<double>("devicePixelRatio")
        };
        return (IJSScreen)ScreenField;
    });
    #endregion
    #region 返回Navigator对象
    public IJSNavigator Navigator { get; }
        = new JSNavigator(jsRuntime);
    #endregion
    #endregion
    #region 关于存储
    #region 本地存储
    public IAsyncDictionary<string, string> LocalStorage { get; }
        = new JSStorage(jsRuntime, true);
    #endregion
    #region 会话存储
    public IAsyncDictionary<string, string> SessionStorage { get; }
        = new JSStorage(jsRuntime, false);
    #endregion
    #endregion
    #region 返回Location对象
    public IJSLocation Location { get; }
        = new JSLocation(jsRuntime);
    #endregion
    #region 关于弹窗
    #region 弹出确认窗
    public ValueTask<bool> Confirm(string message, CancellationToken cancellation = default)
          => jsRuntime.InvokeAsync<bool>("confirm", cancellation, message);
    #endregion
    #region 弹出消息窗
    public ValueTask Alert(string message, CancellationToken cancellation = default)
           => jsRuntime.InvokeVoidAsync("alert", cancellation, message);
    #endregion
    #region 弹出输入框
    public ValueTask<string?> Prompt(string? text = null, string? value = null, CancellationToken cancellation = default)
        => jsRuntime.InvokeAsync<string?>("prompt", cancellation, text, value);
    #endregion
    #region 打印窗口
    public ValueTask Print(CancellationToken cancellation = default)
          => jsRuntime.InvokeVoidAsync("print", cancellation);
    #endregion
    #endregion
    #region 关于打开与关闭窗口
    #region 打开新窗口
    public ValueTask Open(string strUrl, string strWindowName, string strWindowFeatures = "noopener", CancellationToken cancellation = default)
        => jsRuntime.InvokeVoidAsync("open", cancellation, strUrl, strWindowName, strWindowFeatures);
    #endregion
    #region 关闭窗口
    public ValueTask Close(CancellationToken cancellation = default)
         => jsRuntime.InvokeVoidAsync("close", cancellation);
    #endregion
    #endregion
    #region 直接执行JS代码
    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args)
        => jsRuntime.InvokeAsync<TValue>(identifier, args);

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        => jsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);
    #endregion
    #region 通过JS发起Post请求
    public async ValueTask<Ret?> FetchPost<Ret>(UriComplete uri, object? parameter, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= CreateDesign.JsonCommonOptions();
        return await jsRuntime.AwaitPromise(x => JsonSerializer.Deserialize<Ret>(x, options),
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
                                'Content-Type': '{{MediaTypeName.TextJson}}'
                            }
                        };
                        fetch('{{uri}}',myInit).
                            then(x=>x.json()).
                            then(x=>{{successMethod}}(x)).
                            catch({{failMethod}});
                """;
                     return script;
                 }, null, cancellationToken);
    }
    #endregion
    #region 关于通知
    #region 请求通知权限
    public Task<bool> RequestNotifications()
        => jsRuntime.AwaitPromise(x => true,
            (success, fail) =>
            $$"""
            if(!("Notification" in window))
            {
                {{fail}}('浏览器不支持通知API');
                return;
            }
            Notification.requestPermission().then(state=>
            {
                if(state=='granted')
                    {{success}}(state);
                else
                    {{fail}}(state);
            }).catch(error=>
            {
                {{fail}}(error);
            });
            """);
    #endregion
    #region 返回通知对象
    #region 正式属性
    public async Task<INotifications?> Notifications(bool requestNotifications)
    {
        if (NotificationsFiled is { })
            return NotificationsFiled;
        return await jsRuntime.AwaitPromise(_ => NotificationsFiled = new Notifications(jsRuntime), (success, fail) =>
         $$"""
                if(!("Notification" in window))
                {
                    {{fail}}('浏览器不支持通知API');
                    return;
                }
                if(Notification.permission=='granted')
                {
                    {{success}}(1);
                }
                else
                {
                    if({{(!requestNotifications).ToJSSecurity()}}||Notification.permission=='denied')
                    {
                        {{fail}}(1);
                    }
                    else
                    {
                        Notification.requestPermission().then(state=>
                        {
                            if(state=='granted')
                                {{success}}(state);
                            else
                                {{fail}}(state);
                        }).catch(error=>
                        {
                            {{fail}}(error);
                        });
                    }
                }
                """);
    }
    #endregion
    #region 缓存通知对象
    /// <summary>
    /// 获取缓存的通知对象
    /// </summary>
    private INotifications? NotificationsFiled { get; set; }
    #endregion
    #endregion
    #endregion
}
