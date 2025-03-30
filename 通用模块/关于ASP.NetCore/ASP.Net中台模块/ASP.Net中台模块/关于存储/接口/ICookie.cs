namespace Microsoft.AspNetCore;

/// <summary>
/// 这个对象可以用来管理Cookie
/// </summary>
public interface ICookie : IBrowserStorage
{
    #region 读取原始Cookie
    /// <summary>
    /// 读取原始Cookie，
    /// 注意：它一次性读取全部Cookie
    /// </summary>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    async ValueTask<string> GetCookieOriginal(CancellationToken cancellation = default)
    {
        var cookies = await this.ToArrayAsync(cancellation);
        return cookies.Join(x => $"{x.Key}={x.Value}", "; ");
    }
    #endregion
    #region 写入原始Cookie
    /// <summary>
    /// 按照原始文本写入Cookie，
    /// 注意：它每次只能写入一条Cookie
    /// </summary>
    /// <param name="cookie">待写入的原始Cookie文本</param>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    ValueTask SetCookieOriginal(string cookie, CancellationToken cancellation = default);
    #endregion
    #region 从Http响应写入Cookie
    /// <summary>
    /// 如果一个<see cref="HttpResponseMessage"/>中含有Set-Cookie标头，
    /// 则写入Cookie，否则不做任何操作
    /// </summary>
    /// <param name="httpResponse">待检查的Http响应</param>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    async ValueTask SetCookieFromHttp(HttpResponseMessage httpResponse, CancellationToken cancellation = default)
    {
        if (!httpResponse.Headers.TryGetValues("Set-Cookie", out var setCookie))
            return;
        foreach (var item in setCookie)
        {
            var cookie = item.Remove("; httponly");
            await SetCookieOriginal(cookie, cancellation);
        }
    }
    #endregion
}
