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
        get => HeadersVar.TryGetValue("Authorization").Value is { } v ?
            AuthenticationHeaderValue.Parse(v.Join(" ")) : null;
        init
        {
            if (value is null)
                HeadersVar.Remove("Authorization");
            else
                HeadersVar["Authorization"] = value.ToString().Split(' ').ToArray();
        }
    }
    #endregion
    #region Cookie标头
    public string? Cookie
    {
        get => HeadersVar.TryGetValue("Cookie").Value is { } v ?
            v.Join(";") : null;
        init
        {
            if (value is null)
                HeadersVar.Remove("Cookie");
            else
                HeadersVar["Cookie"] = value.Split(";").ToArray();
        }
    }
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
