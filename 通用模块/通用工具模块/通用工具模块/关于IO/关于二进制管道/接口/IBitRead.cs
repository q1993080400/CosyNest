using System.IOFrancis.FileSystem;

namespace System.IOFrancis.Bit;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个支持读取的二进制管道
/// </summary>
public interface IBitRead : IBitPipeBase
{
    #region 说明文档
    /*问：新版Net推荐使用值类型（例如Memory）来代替字节数组，
      请问为什么本接口不这样做？
      答：因为为了方便考虑，Read方法支持一次读取所有数据，
      如果返回值类型的话，在数据较多时复制该对象的成本会非常可怕*/
    #endregion
    #region 将数据保存到文件中
    /// <summary>
    /// 将数据保存到文件中
    /// </summary>
    /// <param name="path">要保存的路径</param>
    /// <param name="autoName">如果这个值为<see langword="true"/>，
    /// 且该管道具有格式，且<paramref name="path"/>没有扩展名，
    /// 则自动为其加上扩展名</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    async Task SaveToFile(PathText path, bool autoName = true, CancellationToken cancellation = default)
    {
        if (autoName && Format is { } f && !Path.HasExtension(path))        //如果具有扩展名，而且路径忘记加上扩展名
            path = ToolPath.RefactoringPath(path, newExtension: f);         //则将扩展名重构
        using var stream = CreateIO.FileStream(path);
        await CopyTo(stream, cancellation);
    }
    #endregion
    #region 读取二进制管道
    /// <summary>
    /// 以流的形式读取二进制数据
    /// </summary>
    /// <param name="cancellation">用于取消迭代的令牌</param>
    /// <returns></returns>
    IEnumerableFit<byte> Read(CancellationToken cancellation = default);

    /*实现本API请遵循以下规范：
      #每次遍历该迭代器时，都应该从数据的开头返回数据，
      换言之，在多次读取数据时，不应该像Stream一样需要手动的复位操作

      #虽然从外表上看不出来，但是在底层应该启用缓冲，
      来避免一次性读取全部数据

      #本API应该是一个纯函数，换言之，
      修改返回的字节数组中的值，不应该影响下一次调用方法返回的数据，
      由于本API需要分割缓冲区，因此这个规范很容易满足*/
    #endregion
    #region 读取二进制管道（不拆分缓冲区）
    /// <summary>
    /// 一次性读取全部二进制数据，且不拆分缓冲区，
    /// 如果没有任何数据，则返回一个空数组
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Read(CancellationToken)"/>
    Task<byte[]> ReadComplete(CancellationToken cancellation = default)
        => Task.Run(() => Read(cancellation).ToArray());
    #endregion
    #region 以Base64的形式读取二进制管道
    /// <summary>
    /// 读取二进制管道，然后以Base64的形式返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Read(CancellationToken)"/>
    async Task<string> ReadBase64(CancellationToken cancellation = default)
    {
        var bytes = await ReadComplete(cancellation);
        return Convert.ToBase64String(bytes);
    }
    #endregion
    #region 复制二进制数据
    #region 复制到另一个管道
    /// <summary>
    /// 读取这个管道的二进制数据，
    /// 并将其复制到另一个管道中
    /// </summary>
    /// <param name="write">复制的目标管道</param>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    Task CopyTo(IBitWrite write, CancellationToken cancellation = default)
       => write.Write(Read(cancellation), cancellation).AsTask();
    #endregion
    #region 复制到另一个流
    /// <summary>
    /// 读取这个管道的二进制数据，
    /// 并将其复制到另一个流中
    /// </summary>
    /// <param name="write">复制的目标流</param>
    /// <inheritdoc cref="CopyTo(IBitWrite, CancellationToken)"/>
    async Task CopyTo(Stream write, CancellationToken cancellation = default)
    {
        await foreach (var item in Read(cancellation))
        {
            write.WriteByte(item);
        }
    }
    #endregion
    #endregion
    #region 转换管道
    /// <summary>
    /// 转换本管道的输出，并返回一个新的管道
    /// </summary>
    /// <param name="mapping">用来转换管道的函数</param>
    /// <returns></returns>
    IBitRead Pipe(BitMapping mapping)
        => mapping(this);
    #endregion
}
