using System.Design;
using System.IOFrancis;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.Runtime.CompilerServices;
using System.Text;

namespace System;

/// <summary>
/// 关于IO的扩展方法
/// </summary>
public static class ExtenIO
{
    #region 根据文件类型筛选文件集合
    /// <summary>
    /// 在一个文件集合中，筛选出所有与指定文件类型兼容的文件
    /// </summary>
    /// <param name="list">要筛选的集合</param>
    /// <param name="fileType">作为条件的路径对象</param>
    /// <param name="isForward">如果这个值为<see langword="true"/>，选择是这个类型的文件，
    /// 否则选择不是这个类型的文件，即取反</param>
    /// <returns></returns>
    public static IEnumerable<IFile> WhereFileType(this IEnumerable<IFile> list, IFileType fileType, bool isForward = true)
        => list.Where(x => x.IsCompatible(fileType) == isForward);
    #endregion
    #region 关于流
    #region 读取流的全部内容
    /// <summary>
    /// 读取流中的全部内容
    /// </summary>
    /// <param name="stream">待读取内容的流</param>
    /// <returns></returns>
    public static async Task<byte[]> ReadAll(this Stream stream)
    {
        var memory = await stream.CopyToMemory();
        return memory.ToArray();
    }
    #endregion
    #region 将流复制到一个内存流
    /// <summary>
    /// 将流复制到一个内存流，并返回该内存流
    /// </summary>
    /// <param name="stream">待复制的流</param>
    /// <returns></returns>
    public static async Task<MemoryStream> CopyToMemory(this Stream stream)
    {
        stream.Reset();
        var memory = new MemoryStream();
        await stream.CopyToAsync(memory);
        memory.Reset();
        return memory;
    }
    #endregion
    #region 将流保存到文件中
    /// <summary>
    /// 将流保存到文件中
    /// </summary>
    /// <param name="stream">要读取的流</param>
    /// <param name="path">要保存到的文件路径</param>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    public static async Task SaveFile(this Stream stream, PathText path, CancellationToken cancellationToken = default)
    {
        if (!stream.CanRead)
            throw new NotSupportedException("这个流不可读取");
        stream.Reset();
        if (File.Exists(path))
            File.Delete(path);
        using var fileStream = CreateIO.FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fileStream, cancellationToken);
    }
    #endregion
    #region 重置流
    /// <summary>
    /// 将流重置到开始位置
    /// </summary>
    /// <param name="stream">待重置的流</param>
    public static void Reset(this Stream stream)
    {
        if (stream.CanSeek && stream.Position is not 0)
            stream.Seek(0, SeekOrigin.Begin);
    }
    #endregion
    #endregion
    #region 关于管道
    #region 将对象转换为IBitRead
    #region 转换流
    /// <summary>
    /// 将一个<see cref="Stream"/>转换为等效的<see cref="IFullDuplex"/>
    /// </summary>
    /// <param name="stream">待转换的<see cref="Stream"/>对象</param>
    /// <returns></returns>
    /// <inheritdoc cref="ToBitRead(IAsyncEnumerable{byte}, string?, string?, IInstruct?)"/>
    public static IFullDuplex ToBitPipe(this Stream stream, string? format = null, string? describe = null)
    {
        var pipe = new BitPipeStream(stream, format, describe);
        return CreateIO.FullDuplex(pipe, pipe);
    }
    #endregion
    #region 转换异步迭代器
    #region 转换枚举字节集合的迭代器
    /// <summary>
    /// 创建一个<see cref="IBitRead"/>，
    /// 它通过迭代器读取数据
    /// </summary>
    /// <inheritdoc cref="BitReadEnumerable{Byte}.BitReadEnumerable(IAsyncEnumerable{Byte}, string?, string?, IInstruct?)"/>
    public static IBitRead ToBitRead(this IAsyncEnumerable<IEnumerable<byte>> bytes, string? format = null, string? describe = null, IInstruct? source = null)
        => new BitReadEnumerable<IEnumerable<byte>>(bytes, format, describe, source);
    #endregion
    #region 转换枚举字节的迭代器
    ///<inheritdoc cref="ToBitRead(IAsyncEnumerable{IEnumerable{byte}}, string, string?, IInstruct?)"/>
    public static IBitRead ToBitRead(this IAsyncEnumerable<byte> bytes, string? format = null, string? describe = null, IInstruct? source = null)
        => new BitReadEnumerable<byte>(bytes, format, describe, source);
    #endregion
    #endregion
    #region 转换同步迭代器
    #region 转换枚举字节集合的迭代器
    ///<inheritdoc cref="ToBitRead(IAsyncEnumerable{IEnumerable{byte}}, string, string?, IInstruct?)"/>
    public static IBitRead ToBitRead(this IEnumerable<IEnumerable<byte>> bytes, string? format = null, string? describe = null)
        => new BitReadEnumerable<IEnumerable<byte>>(bytes.Fit(), format, describe, null);
    #endregion
    #region 转换枚举字节的迭代器
    /// <inheritdoc cref="ToBitRead(IAsyncEnumerable{IEnumerable{byte}}, string, string?, IInstruct?)"/>
    public static IBitRead ToBitRead(this IEnumerable<byte> bytes, string? format = null, string? describe = null)
        => new BitReadEnumerable<byte>(bytes.Fit(), format, describe, null);
    #endregion
    #endregion
    #endregion
    #region 拆分缓冲区
    /// <summary>
    /// 按照指定的缓冲区大小，重新拆分异步流的缓冲区
    /// </summary>
    /// <param name="source">待拆分的异步流</param>
    /// <param name="bufferSize">新缓冲区的大小</param>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    public static async IAsyncEnumerable<byte[]> SplitBuffer(this IAsyncEnumerable<byte[]> source, int bufferSize = 1024, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        ExceptionIntervalOut.Check(1, null, bufferSize);
        await using var enumerator = source.GetAsyncEnumerator(cancellation);
        var buffer = new byte[bufferSize];
        var surplus = Array.Empty<byte>();
        var pos = 0;
        while (true)
        {
            while (surplus.Any())           //处理上次迭代的剩余部分
            {
                var copyLen = Math.Min(bufferSize - pos, surplus.Length);
                Array.Copy(surplus, 0, buffer, pos, copyLen);
                pos += copyLen;
                if (pos == bufferSize)
                {
                    yield return buffer;
                    buffer = new byte[bufferSize];
                    pos = 0;
                }
                surplus = copyLen == surplus.Length ? Array.Empty<byte>() : surplus[copyLen..];
            }
            var hasElement = await enumerator.MoveNextAsync();
            if (!hasElement)
            {
                if (pos > 0)      //如果迭代完毕，将剩余部分全部返回
                    yield return buffer[..pos];
                yield break;
            }
            var current = enumerator.Current;
            var len = current.Length;
            var difference = bufferSize - pos - len;
            if (difference >= 0)        //如果返回的字节数组不足以填充缓冲区，则将它们全部复制到缓冲区
            {
                Array.Copy(current, 0, buffer, pos, len);
                pos += len;
            }
            else                    //如果返回的字节数组超过了缓冲区的范围，则将未超过的部分复制到缓冲区，超过部分复制到剩余数据中
            {
                var copyLen = len + difference;
                Array.Copy(current, 0, buffer, pos, copyLen);
                surplus = current[copyLen..];
                pos += copyLen;
            }
            if (pos == bufferSize)              //如果缓冲区被填满，则返回之
            {
                yield return buffer;
                buffer = new byte[bufferSize];
                pos = 0;
            }
        }
    }
    #endregion
    #endregion
    #region 计算校验和
    /// <summary>
    /// 计算数据的校验和
    /// </summary>
    /// <param name="bytes">待计算校验和的数据</param>
    /// <returns></returns>
    public static int Checksum(this IEnumerable<byte> bytes)
        => bytes.Select(x => (int)x).Sum();
    #endregion
    #region 关于转换管道
    #region 连接转换管道
    /// <summary>
    /// 将两个转换管道连接起来
    /// </summary>
    /// <param name="input">输入转换管道</param>
    /// <param name="output">输出转换管道</param>
    /// <returns></returns>
    public static BitMapping Join(this BitMapping input, BitMapping output)
        => x => output(input(x));
    #endregion
    #region 将转换管道转变为转换字符串
    /// <summary>
    /// 将一个转换管道的委托变成转换字符串的委托
    /// </summary>
    /// <param name="mapping">用来转换管道的委托</param>
    /// <param name="coding">用来输入字符串编码为字节数组的委托，
    /// 如果为<see langword="null"/>，则默认编码为UTF8</param>
    /// <param name="decode">用来将输出字节数组解码为字符串的委托，
    /// 如果为<see langword="null"/>，则默认解码为Base64</param>
    /// <returns></returns>
    public static Func<string, Task<string>> ConvertText(this BitMapping mapping, Func<string, byte[]>? coding = null, Func<byte[], string>? decode = null)
    {
        coding ??= Encoding.UTF8.GetBytes;
        decode ??= Convert.ToBase64String;
        return async x => decode(await mapping(coding(x).ToBitRead()).ReadComplete());
    }
    #endregion 
    #endregion
}
