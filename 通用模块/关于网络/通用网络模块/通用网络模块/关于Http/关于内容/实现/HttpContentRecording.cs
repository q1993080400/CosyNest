using System.IOFrancis.Bit;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IHttpContent"/>的实现，
/// 可以视为一个Http消息的内容
/// </summary>
public sealed record HttpContentRecording : IHttpContent
{
    #region Http标头
    IHttpHeaderContent IHttpContent.Header
        => Header;

    /// <summary>
    /// 返回Http内容的标头
    /// </summary>
    public required HttpHeaderContent Header { get; init; }
    #endregion
    #region Http内容
    public required IBitRead Content { get; init; }
    #endregion
}
