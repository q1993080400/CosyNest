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
    public HttpHeaderContent Header { get; init; }
    #endregion
    #region Http内容
    public IBitRead Content { get; init; }
    #endregion
    #region 构造函数
    #region 无参数构造函数
    public HttpContentRecording()
    {
        Content = Array.Empty<byte>().ToBitRead();
        Header = new();
    }
    #endregion
    #region 复制HttpContent的内容
    /// <summary>
    /// 使用指定的Http内容初始化对象
    /// </summary>
    /// <param name="content">指定的Http内容，在执行完毕后，它会被释放</param>
    public HttpContentRecording(HttpContent content)
    {
        var stream = new MemoryStream();
        content.CopyTo(stream, null, default);
        Content = stream.ToBitPipe().Read;
        Header = new(content.Headers);
    }
    #endregion
    #endregion
}
