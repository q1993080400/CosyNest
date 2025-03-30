using System.Diagnostics.CodeAnalysis;
using System.MathFrancis;
using System.Underlying;

using Microsoft.AspNetCore;

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
            LogicalResolution = CreateMath.Size<double>(width, height),
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
    public IBrowserStorage LocalStorage { get; }
        = new JSStorage(jsRuntime, true);
    #endregion
    #region 会话存储
    public IBrowserStorage SessionStorage { get; }
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
    public async ValueTask<(bool IsConfirm, string? Input)> Prompt(string? text = null, string? value = null, CancellationToken cancellation = default)
    {
        var input = await jsRuntime.InvokeAsync<string?>("prompt", cancellation, text, value);
        var isConfirm = input is { };
        return (isConfirm, input.IsVoid() ? null : input);
    }
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
    #region 关于通知
    #region 请求通知权限
    public async Task<bool> RequestNotifications(CancellationToken cancellationToken = default)
        => await jsRuntime.InvokeAsync<bool>("RequestNotificationPermission", cancellationToken);
    #endregion
    #region 返回通知对象
    public async Task<INotifications<WebNotificationsOptions>?> Notifications(CancellationToken cancellationToken = default)
        => await RequestNotifications(cancellationToken) ?
        new NotificationsWeb(jsRuntime) : null;
    #endregion
    #endregion
}
