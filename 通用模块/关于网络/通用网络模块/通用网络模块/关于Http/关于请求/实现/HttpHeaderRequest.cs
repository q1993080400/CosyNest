using System.Collections.Immutable;
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
        get => GetHeader("Authorization", v => AuthenticationHeaderValue.Parse(v.Single()));
        init => SetHeader("Authorization", value, v => [v.ToString()]);
    }
    #endregion
    #region Cookie标头
    public string? Cookie
    {
        get => GetHeader("Cookie", v => v.Join(";"));
        init => SetHeader("Cookie", value, v => v.Split(";"));
    }
    #endregion
    #region User-Agent标头
    public string? UserAgent
    {
        get => GetHeader("User-Agent", v => v.Single());
        init => SetHeader("User-Agent", value, v => [v]);
    }

    /*警告：
      国内微信，QQ等浏览器实现这个标头不规范，
      在这种情况下，直接设置这个标头会引发异常*/
    #endregion
    #region Host标头
    public string? Host
    {
        get => GetHeader("Host", v => v.First());
        init => SetHeader("Host", value, v => [v]);
    }
    #endregion
    #endregion
    #region 抽象成员实现
    #region 改变标头
    public override HttpHeaderRequest With
        (Func<ImmutableDictionary<string, IEnumerable<string>>, ImmutableDictionary<string, IEnumerable<string>>> change)
        => new(change(HeadersImmutable));
    #endregion
    #endregion
    #region 构造函数
    #region 无参数构造函数
    public HttpHeaderRequest()
        : this([])
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
