using System.Design;
using System.Runtime.CompilerServices;

namespace System.IOFrancis.Bit;

/// <summary>
/// 这个类型允许将流适配为管道
/// </summary>
/// <inheritdoc cref="ExtenIO.ToBitPipe(Stream, string, string?)"/>
sealed class BitPipeStream(Stream stream, string? format, string? describe) : AutoRelease, IBitRead, IBitWrite
{
    #region 封装的对象
    #region Stream对象
    /// <summary>
    /// 获取封装的<see cref="IO.Stream"/>对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Stream Stream { get; } = stream;
    #endregion
    #endregion
    #region 数据的基本信息
    #region 数据的描述
    public string? Describe { get; } = describe;
    #endregion
    #region 数据的格式
    public string? Format { get; } = format;
    #endregion
    #region 数据的长度
    public long? Length => Stream.Length;
    #endregion
    #endregion
    #region 操作数据
    #region 有关锁
    #region 说明文档
    /*问：一般情况下，只有写入才会被锁定，
      为什么本接口也要锁定读取，不允许在读取的时候重复读取？
      答：因为Stream的读取也是有状态的，它会修改Position

      问：为什么Read方法中需要通过using语句释放锁，
      而不是方法返回前将Lock设为false？这样有什么好处？
      答：好处在于，如果迭代器没有被遍历完毕，但是调用了IAsyncEnumerator.DisposeAsync，
      这种情况下锁仍然会被释放*/
    #endregion
    #region 指示是否被锁定
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则该管道正在被读取，为了避免脏读，此时不允许写入或重复读取
    /// </summary>
    private bool Lock { get; set; }
    #endregion
    #region 检查锁
    /// <summary>
    /// 如果这个管道正在被锁定，则触发一个异常
    /// </summary>
    private void CheckLock()
    {
        if (Lock)
            throw new Exception($"该管道正在被读取，在读取完毕之前不允许重复读取或写入数据");
    }
    #endregion
    #endregion
    #region 读取数据
    public async IAsyncEnumerable<byte[]> Read(int bufferSize = 1024, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        ExceptionIntervalOut.Check(1, null, bufferSize);
        CheckLock();
        using var @lock = FastRealize.Disposable(() => Lock = true, () => Lock = false);
        Stream.Reset();
        while (true)
        {
            var memory = new byte[bufferSize];
            switch (await Stream.ReadAtLeastAsync(memory, bufferSize, false, cancellation))
            {
                case 0:
                    yield break;
                case var c when c < bufferSize:
                    yield return memory[0..c];
                    yield break;
                default:
                    yield return memory;
                    break;
            }
        }
    }
    #endregion
    #region 写入数据
    public async ValueTask Write(IEnumerable<byte> data, CancellationToken cancellation = default)
    {
        CheckLock();
        using var @lock = FastRealize.Disposable(() => Lock = true, () => Lock = false);
        Stream.Reset();
        foreach (var item in data.Chunk(1024))
        {
            await Stream.WriteAsync(item, cancellation);
            await Stream.FlushAsync(cancellation);
        }
    }
    #endregion
    #region 转换为流
    public Stream ToStream()
    {
        CheckLock();
        Stream.Reset();
        return Stream;
    }
    #endregion
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
        => Stream.Dispose();

    #endregion
}
