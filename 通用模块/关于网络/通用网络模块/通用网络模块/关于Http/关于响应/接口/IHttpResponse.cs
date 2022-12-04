using System.Net;

namespace System.NetFrancis.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Http请求的响应
/// </summary>
public interface IHttpResponse : IMessagePacket<HttpStatusCode, IHttpContent>
{
    #region 响应的标头
    /// <summary>
    /// 获取Http响应的标头
    /// </summary>
    IHttpHeaderResponse Header { get; }
    #endregion
    #region 如果响应不成功，则抛出异常
    /// <summary>
    /// 如果响应不成功，则抛出异常
    /// </summary>
    void ThrowIfNotSuccess();
    #endregion
}
