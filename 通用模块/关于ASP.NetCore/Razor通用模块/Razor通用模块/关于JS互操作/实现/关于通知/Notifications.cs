using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="INotifications"/>的浏览器实现，
/// 它可以用来发送通知，警告：
/// 本类型在低版本IOS浏览器上无法正常工作
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="jSRuntime">封装的JS运行时对象，
/// 本对象的功能就是通过它实现的</param>
sealed class Notifications(IJSRuntime jSRuntime) : INotifications
{
    #region 公开成员
    #region 发送通知
    public async Task Send(INotificationsOptions options)
    {
        var script = $$"""
            var tag={{options.Tag.ToJSSecurity()}};
            var options=
            {
                body:{{options.Body.ToJSSecurity()}},
                tag:tag,
                icon:{{options.Icon.ToJSSecurity()}},
                requireInteraction:{{options.RequireInteraction.ToJSSecurity()}},
                data:{{(options is NotificationsOptionsWeb web ? web.Uri : null).ToJSSecurity()}}
            };
            var title={{options.Title.ToJSSecurity()}};
            var swUri='/js/sw.js';
            navigator.serviceWorker.register(swUri,{scope: swUri}).then(x=>x.update()).then(function(registration)
            {
               registration.showNotification(title,options);
            });
            """;
        await JSRuntime.InvokeCodeVoidAsync(script);
    }
    #endregion
    #region 关闭通知
    public async Task Close(IEnumerable<string>? tags)
    {
        var script = $$"""
            var tags={{(tags is null ? "null" : $"[{tags.Join(static x => $"'{x}'", ",")}]")}};
            var swUri='/js/sw.js';
            navigator.serviceWorker.register(swUri,{scope: swUri}).
            then(x=>x.update()).then(x=>x.getNotifications()).
            then(function(notifications)
            {
                for (var notification of notifications)
                {
                    if(tags==null||tags.includes(notification.tag))
                        notification.close();
                }
            });
            """;
        await JSRuntime.InvokeCodeVoidAsync(script);
    }
    #endregion
    #endregion
    #region 内部成员
    #region JS运行时对象
    /// <summary>
    /// 获取封装的JS运行时对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IJSRuntime JSRuntime { get; } = jSRuntime;

    #endregion
    #endregion
}
