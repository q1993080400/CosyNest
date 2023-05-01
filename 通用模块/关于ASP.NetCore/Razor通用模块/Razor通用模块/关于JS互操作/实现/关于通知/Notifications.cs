using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="INotifications"/>的浏览器实现，
/// 它可以用来发送通知，警告：
/// 本类型在安卓版谷歌浏览器，以及低版本IOS浏览器上无法正常工作
/// </summary>
sealed class Notifications : INotifications
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
            var swUri='/_content/ToolRazor/js/sw.js';
            navigator.serviceWorker.register(swUri,{scope: swUri}).then(x=>x.update()).then(function(registration)
            {
               registration.showNotification(title,options);
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
    private IJSRuntime JSRuntime { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="jSRuntime">封装的JS运行时对象，
    /// 本对象的功能就是通过它实现的</param>
    public Notifications(IJSRuntime jSRuntime)
    {
        JSRuntime = jSRuntime;
    }
    #endregion
}
