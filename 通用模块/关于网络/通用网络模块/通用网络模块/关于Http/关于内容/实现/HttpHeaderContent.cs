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
        init => SetHeader("Content-Type", value, x => [.. x.ToString().Split(";")]);
    }
    #endregion
    #region 构造函数
    #region 无参数构造函数
    public HttpHeaderContent()
    {

    }
    #endregion
    #region 复制HttpContentHeaders
    /// <summary>
    /// 复制一个<see cref="HttpContentHeaders"/>的所有属性，并初始化对象
    /// </summary>
    /// <param name="headers">待复制的内容标头</param>
    internal HttpHeaderContent(HttpContentHeaders headers)
        : base(headers)
    {

    }
    #endregion
    #endregion
}
