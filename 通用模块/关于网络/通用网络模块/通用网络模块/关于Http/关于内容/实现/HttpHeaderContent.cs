using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.NetFrancis.Http.Realize;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个记录代表Http内容的标头
/// </summary>
public sealed record HttpHeaderContent : HttpHeader, IHttpHeaderContent
{
    #region 获取编码标头
    public IEnumerable<string>? ContentEncoding
    {
        get => GetHeader("Content-Encoding", x => x);
        init => SetHeader("Content-Encoding", value, x => x);

    }
    #endregion
    #region 获取媒体类型标头
    public MediaTypeHeaderValue? ContentType
    {
        get => GetHeader("Content-Type", x => MediaTypeHeaderValue.Parse(x.Join(";")));
        init => SetHeader("Content-Type", value, x => [x.ToString()]);
    }
    #endregion
    #region 抽象成员实现
    #region 改变标头
    public override HttpHeaderContent With
        (Func<ImmutableDictionary<string, IEnumerable<string>>, ImmutableDictionary<string, IEnumerable<string>>> change)
        => new(change(HeadersImmutable));
    #endregion
    #endregion
    #region 构造函数
    #region 无参数构造函数
    public HttpHeaderContent()
        : this([])
    {

    }
    #endregion
    #region 传入标头集合
    /// <inheritdoc cref="HttpHeader(IEnumerable{KeyValuePair{string, IEnumerable{string}}})"/>
    public HttpHeaderContent(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        : base(headers)
    {

    }
    #endregion 
    #endregion
}
