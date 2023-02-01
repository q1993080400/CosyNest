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
        get => HeadersVar.TryGetValue("Content-Encoding").Value;
        init
        {
            if (value is null)
                HeadersVar.Remove("Content-Encoding");
            else
                HeadersVar["Content-Encoding"] = value;
        }
    }
    #endregion
    #region 获取媒体类型标头
    public MediaTypeHeaderValue? ContentType
    {
        get => HeadersVar.TryGetValue("Content-Type").Value is { } v ?
            MediaTypeHeaderValue.Parse(v.Join(";")) : null;
        init
        {
            if (value is null)
                HeadersVar.Remove("Content-Type");
            else
                HeadersVar["Content-Type"] = value.ToString().Split(";").ToArray();
        }
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
