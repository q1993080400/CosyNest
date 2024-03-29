﻿using System.NetFrancis.Http.Realize;

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
    #region 构造函数
    /// <inheritdoc cref="HttpHeader(IEnumerable{KeyValuePair{string, IEnumerable{string}}})"/>
    public HttpHeaderResponse(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        : base(headers)
    {

    }
    #endregion
}
