namespace System.IOFrancis.Bit;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个支持跳转的二进制读取管道
/// </summary>
public interface IBitReadFind : IBitRead
{
    #region 读取二进制流的一部分
    /// <summary>
    /// 读取二进制流的一部分
    /// </summary>
    /// <param name="range">指定读取二进制流的哪一个部分</param>
    /// <param name="bufferSize">指定缓冲区的字节数量，
    /// 如果为<see langword="null"/>，则一次读取全部数据</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    IAsyncEnumerable<byte[]> Read(Range range, long? bufferSize = null, CancellationToken cancellation = default);
    #endregion
    #region 读取二进制流的一部分（不拆分缓冲区）
    /// <summary>
    /// 读取二进制流的一部分，且不拆分缓冲区，
    /// 如果没有任何数据，则返回一个空数组
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Read(Range, long?, CancellationToken)"/>
    async Task<byte[]> ReadComplete(Range range, CancellationToken cancellation = default)
         => (await Read(range, null, cancellation).Linq(x => x.FirstOrDefault())) ?? Array.Empty<byte>();
    #endregion
}
