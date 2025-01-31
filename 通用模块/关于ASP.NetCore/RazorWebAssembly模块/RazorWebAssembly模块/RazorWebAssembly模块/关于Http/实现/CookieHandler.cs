using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="DelegatingHandler"/>的实现，
/// 它指示浏览器，应该在请求中携带Cookie
/// </summary>
sealed class CookieHandler : DelegatingHandler
{
    #region 发送Http请求
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        request.Headers.Add("X-Requested-With", ["XMLHttpRequest"]);
        return base.SendAsync(request, cancellationToken);
    }
    #endregion 
}