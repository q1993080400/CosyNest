using System.Collections.Immutable;
using System.NetFrancis.Http.Realize;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个记录代表Http响应的标头
/// </summary>
sealed record HttpHeaderResponse : HttpHeader, IHttpHeaderResponse
{
    #region 写入Cookie
    public IEnumerable<string>? SetCookie
    {
        get => GetHeader("Set-Cookie", v => v);
        init => SetHeader("Set-Cookie", value, v => v);
    }
    #endregion
    #region 抽象成员实现
    #region 改变标头
    public override HttpHeaderResponse With
        (Func<ImmutableDictionary<string, IEnumerable<string>>, ImmutableDictionary<string, IEnumerable<string>>> change)
        => new(change(HeadersImmutable));
    #endregion
    #endregion 
    #region 构造函数
    /// <inheritdoc cref="HttpHeader(IEnumerable{KeyValuePair{string, IEnumerable{string}}})"/>
    public HttpHeaderResponse(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        : base(headers)
    {

    }
    #endregion
}
