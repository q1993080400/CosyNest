using System.Net.Http.Headers;

namespace System.NetFrancis.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Http请求头
/// </summary>
public interface IHttpHeaderRequest : IHttpHeader
{
    #region 获取身份验证标头
    /// <summary>
    /// 获取Authorization标头，
    /// 它被用来执行身份验证
    /// </summary>
    AuthenticationHeaderValue? Authorization { get; }
    #endregion
    #region Cookie标头
    /// <summary>
    /// 获取Cookie标头，它控制浏览器Cookie
    /// </summary>
    string? Cookie { get; }
    #endregion
    #region User-Agent标头
    /// <summary>
    /// 获取User-Agent标头，
    /// 它控制用户代理
    /// </summary>
    string? UserAgent { get; }
    #endregion
    #region Host标头
    /// <summary>
    /// Host标头
    /// </summary>
    string? Host { get; }
    #endregion
}
