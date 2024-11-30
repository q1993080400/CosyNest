namespace Microsoft.AspNetCore;

/// <summary>
/// 这个对象可以用来管理Cookie
/// </summary>
public interface ICookie : IAsyncDictionary<string, string>
{
    #region 写入原始Cookie
    /// <summary>
    /// 按照原始文本写入Cookie
    /// </summary>
    /// <param name="cookie">待写入的原始Cookie文本</param>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    ValueTask SetCookieOriginal(string cookie, CancellationToken cancellation = default);
    #endregion
    #region 从Http响应写入Cookie
    /// <summary>
    /// 如果一个<see cref="IHttpResponse"/>中含有Set-Cookie标头，
    /// 则写入Cookie，否则不做任何操作
    /// </summary>
    /// <param name="httpResponse">待检查的Http响应</param>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    async ValueTask SetCookieFromHttp(HttpResponseMessage httpResponse, CancellationToken cancellation = default)
    {
        httpResponse.Headers.TryGetValues("Set-Cookie", out var setCookie);
        if (setCookie is null)
            return;
        foreach (var item in setCookie)
        {
            var cookie = item.Remove("; httponly");
            await SetCookieOriginal(cookie, cancellation);
        }
    }
    #endregion
}
