using System.Underlying;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型可以使用JS来发送Web通知
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象</param>
sealed class NotificationsWeb(IJSRuntime jsRuntime) : INotifications<WebNotificationsOptions>
{
    #region 发送通知
    public async Task Send(WebNotificationsOptions parameter, CancellationToken cancellationToken = default)
        => await jsRuntime.InvokeVoidAsync("ShowNotification", cancellationToken, parameter);
    #endregion 
}
