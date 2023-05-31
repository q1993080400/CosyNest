using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个对象可以用来管理Cookie
/// </summary>
public interface ICookie : IAsyncDictionary<string, string>
{
    #region 说明文档
    /*实现本API请遵循以下规范：
      应该将Cookie进行缓存，尽量减少对JS互操作的依赖，
      这是因为这玩意非常不靠谱，尤其是使用HubConnection连接的情况下，
      很容易出现问题*/
    #endregion
    #region 有关写入Cookie
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
    async ValueTask SetCookieFromHttp(IHttpResponse httpResponse, CancellationToken cancellation = default)
    {
        if (httpResponse.Header.SetCookie is { } setCookie)
        {
            foreach (var item in setCookie)
            {
                var cookie = item.Remove("; httponly");
                await SetCookieOriginal(cookie, cancellation);
            }
        }
    }
    #endregion
    #region 刷新Cookie
    /// <summary>
    /// 如果您直接使用JS脚本，而不是本类型的API设置Cookie，
    /// 请调用本方法刷新Cookie缓存
    /// </summary>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Refresh(CancellationToken cancellationToken = default);
    #endregion
    #endregion
}
