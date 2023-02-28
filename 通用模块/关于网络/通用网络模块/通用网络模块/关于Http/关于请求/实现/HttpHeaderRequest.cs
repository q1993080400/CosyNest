using System.Net.Http.Headers;
using System.NetFrancis.Http.Realize;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个记录封装了Http请求头
/// </summary>
public sealed record HttpHeaderRequest : HttpHeader, IHttpHeaderRequest
{
    #region 标头属性
    #region 获取身份验证标头
    public AuthenticationHeaderValue? Authorization
    {
        get => GetHeader("Authorization", v => AuthenticationHeaderValue.Parse(v.Join(" ")));
        init => SetHeader("Authorization", value, v => v.ToString().Split(' ').ToArray());
    }
    #endregion
    #region Cookie标头
    public string? Cookie
    {
        get => GetHeader("Cookie", v => v.Join(";"));
        init => SetHeader("Cookie", value, v => v.Split(";").ToArray());
    }
    #endregion
    #region User-Agent标头
    public string? UserAgent
    {
        get => GetHeader("User-Agent", v => v.Join(" "));
        init => SetHeader("User-Agent", value, v => new[] { v });
    }

    /*警告：
      国内微信，QQ等浏览器实现这个标头不规范，
      在这种情况下，直接设置这个标头会引发异常*/
    #endregion
    #endregion
    #region 构造函数
    #region 无参数构造函数
    public HttpHeaderRequest()
    {

    }
    #endregion
    #region 传入字典
    /// <inheritdoc cref="HttpHeader(IEnumerable{KeyValuePair{string, IEnumerable{string}}})"/>
    public HttpHeaderRequest(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        : base(headers)
    {

    }
    #endregion
    #endregion
}
