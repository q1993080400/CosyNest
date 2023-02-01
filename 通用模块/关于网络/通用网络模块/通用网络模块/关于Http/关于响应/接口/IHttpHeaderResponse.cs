namespace System.NetFrancis.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Http响应的标头
/// </summary>
public interface IHttpHeaderResponse : IHttpHeader
{
    #region 写入Cookie
    /// <summary>
    /// 获取Set-Cookie标头的值，
    /// 它可用于写入Cookie
    /// </summary>
    IEnumerable<string>? SetCookie { get; }
    #endregion
}
