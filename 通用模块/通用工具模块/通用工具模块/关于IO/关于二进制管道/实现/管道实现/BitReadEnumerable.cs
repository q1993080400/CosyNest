using System.Design;
using System.Runtime.CompilerServices;

namespace System.IOFrancis.Bit;

/// <summary>
/// 这个类型是<see cref="IBitRead"/>的实现，
/// 它可以通过<see cref="IAsyncEnumerable{T}"/>来读取二进制数据
/// </summary>
sealed class BitReadEnumerable : IBitRead
{
    #region 公开成员
    #region 管道的信息
    #region 数据格式
    public string? Format { get; }
    #endregion
    #region 对数据的描述
    public string? Describe { get; }
    #endregion
    #region 二进制数据的总长度
    public long? Length
        => null;
    #endregion
    #endregion
    #region 转换为流
    public Stream ToStream()
        => CreateIO.StreamEnumerable(Bytes);
    #endregion
    #region 读取数据
    public async IAsyncEnumerable<byte[]> Read(int bufferSize = 1024, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        ExceptionIntervalOut.Check(1, null, bufferSize);
        var cache = new byte[bufferSize];
        var cachePos = 0;
        await foreach (var array in Bytes)
        {
            var len = array.Length;
            var sum = cachePos + len;
            if (sum < bufferSize)
            {
                Array.Copy(array, 0, cache, cachePos, len);
                cachePos += len;
                continue;
            }
            var arrayPos = 0;
            while (true)
            {
                var surplus = len - arrayPos;
                if (surplus <= bufferSize)
                {
                    Array.Copy(array, arrayPos, cache, cachePos, surplus);
                    if (surplus == bufferSize)
                    {
                        yield return cache;
                        cache = new byte[bufferSize];
                        cachePos = 0;
                        break;
                    }
                    cachePos = surplus;
                    break;
                }
                var copySize = bufferSize - cachePos;
                Array.Copy(array, arrayPos, cache, cachePos, copySize);
                yield return cache;
                arrayPos += copySize;
                cachePos = 0;
                cache = new byte[bufferSize];
            }
        }
        if (cachePos > 0 && cachePos < bufferSize)
            yield return cache[..cachePos];
    }
    #endregion
    #region 关于释放对象
    #region 指定对象是否可用
    public bool IsAvailable => Source?.IsAvailable ?? true;
    #endregion
    #region 释放对象
    public void Dispose()
    {
        if (Source is IDisposable disposable)
            disposable.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion
    #region 析构函数
    ~BitReadEnumerable()
    {
        Dispose();
    }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 异步迭代器
    /// <summary>
    /// 获取封装的异步迭代器对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IAsyncEnumerable<byte[]> Bytes { get; }
    #endregion
    #region 数据的来源
    /// <summary>
    /// 如果此属性不为<see langword="null"/>，
    /// 代表这个管道的数据来自于这个可释放对象，
    /// 它可以使本对象得到正确地释放
    /// </summary>
    private IInstruct? Source { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="bytes">指定的异步迭代器对象，本对象的功能就是通过它实现的</param>
    /// <param name="format">二进制数据的格式，如果格式未知，则为<see langword="null"/></param>
    /// <param name="describe">对数据的描述，如果没有描述，则为<see langword="null"/></param>
    /// <param name="source">如果此属性不为<see langword="null"/>，
    /// 代表这个管道的数据来自于这个可释放对象，它可以使本对象得到正确地释放</param>
    public BitReadEnumerable(IAsyncEnumerable<byte[]> bytes, string? format, string? describe, IInstruct? source)
    {
        Bytes = bytes;
        Format = format;
        Describe = describe;
        Source = source;
        if (source is null)
            GC.SuppressFinalize(this);
    }
    #endregion
}
