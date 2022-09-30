﻿using System.Net.Http.Headers;
using System.NetFrancis.Http.Realize;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个记录封装了Http请求头
/// </summary>
public sealed record HttpHeaderRequest : HttpHeader, IHttpHeaderRequest
{
    #region 标头属性
    #region 获取身份验证标头
    public AuthenticationHeaderValue? Authorization { get; init; }
    #endregion
    #endregion 
    #region 枚举预定义标头属性
    protected override IEnumerable<(string Name, string? Value)> Predefined()
        => new[]
        {
                (nameof(Authorization),Authorization?.ToString()),
        };
    #endregion
    #region 构造函数
    #region 无参数构造函数
    public HttpHeaderRequest()
    {

    }
    #endregion
    #region 传入HttpRequestHeaders
    /// <summary>
    /// 复制一个<see cref="HttpRequestHeaders"/>的所有属性，并初始化对象
    /// </summary>
    /// <param name="headers">待复制的请求标头</param>
    internal HttpHeaderRequest(HttpRequestHeaders headers)
        : base(headers)
    {
        Authorization = headers.Authorization;
    }
    #endregion
    #region 传入字典
    /// <inheritdoc cref="HttpHeader(IEnumerable{KeyValuePair{string, IEnumerable{string}}})"/>
    public HttpHeaderRequest(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        : base(headers)
    {

    }
    #endregion
    #region 传入字符串字典
    /// <inheritdoc cref="HttpHeader(IEnumerable{KeyValuePair{string, IEnumerable{string}}})"/>
    public HttpHeaderRequest(IEnumerable<KeyValuePair<string, string>> headers)
        : base(headers.Select(x => (x.Key, (IEnumerable<string>)new[] { x.Value })).ToDictionary(true))
    {

    }
    #endregion
    #endregion
}
