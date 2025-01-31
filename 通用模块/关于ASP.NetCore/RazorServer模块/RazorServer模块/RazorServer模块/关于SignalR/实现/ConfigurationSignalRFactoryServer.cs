using System.Net;
using System.NetFrancis;

using Microsoft.JSInterop;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是一个专门用于Server模式的<see cref="IConfigurationSignalRFactory"/>实现，
/// 它可以让SignalR正确获取Cookie
/// </summary>
/// <inheritdoc cref="ConfigurationSignalRFactory(IServiceProvider)"/>
sealed class ConfigurationSignalRFactoryServer(IServiceProvider serviceProvider) : ConfigurationSignalRFactory(serviceProvider)
{
    #region 获取Cookie
    protected override async Task<CookieContainer?> GetCookie()
    {
        var jsWindows = ServiceProvider.GetRequiredService<IJSWindow>();
        var keyValues = await jsWindows.Document.Cookie.ToArrayAsync();
        var cookieContainer = new CookieContainer();
        var domainName = ServiceProvider.GetRequiredService<IHostProvide>().Host.DomainName;
        foreach (var (key, value) in keyValues)
        {
            var cookie = new Cookie(key, value, "/", domainName);
            cookieContainer.Add(cookie);
        }
        return cookieContainer;
    }
    #endregion
}
