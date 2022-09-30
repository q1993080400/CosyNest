using System.Design;
using System.IOFrancis;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
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
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    /// <returns></returns>
    public static async Task<byte[]> ReadAll(this Stream stream, CancellationToken cancellation = default)
    {
        var arry = new byte[stream.Length];
        await stream.ReadAsync(arry, cancellation);
        return arry;
    }
    #endregion
    #region 将流复制到一个内存流
    /// <summary>
    /// 将流复制到一个内存流，并返回该内存流
    /// </summary>
    /// <param name="stream">待复制的流</param>
    /// <returns></returns>
    public static MemoryStream CopyToMemory(this Stream stream)
    {
        stream.Reset();
        var memory = new MemoryStream();
        stream.CopyTo(memory);
        memory.Reset();
        return memory;
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
    #region 将对象转换为IBitRead
    #region 转换流
    /// <summary>
    /// 将一个<see cref="Stream"/>转换为等效的<see cref="IFullDuplex"/>
    /// </summary>
    /// <param name="stream">待转换的<see cref="Stream"/>对象</param>
    /// <returns></returns>
    /// <inheritdoc cref="ToBitRead(IAsyncEnumerable{byte[]}, string, string?, IInstruct?)"/>
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
    #region 计算校验和
    /// <summary>
    /// 计算数据的校验和
    /// </summary>
    /// <param name="bytes">待计算校验和的数据</param>
    /// <returns></returns>
    public static int Checksum(this IEnumerable<byte> bytes)
        => bytes.Select(x => (int)x).Sum();
    #endregion
    #region 将转换管道转变为转换字符串
    /// <summary>
    /// 将一个转换管道的委托变成转换字符串的委托
    /// </summary>
    /// <param name="mapping">用来转换管道的委托</param>
    /// <param name="coding">用来编码字符串的委托，
    /// 如果为<see langword="null"/>，则默认编码为UTF8</param>
    /// <returns></returns>
    public static Func<string, IBitRead> ConvertText(this BitMapping mapping, Func<string, byte[]>? coding = null)
    {
        coding ??= Encoding.UTF8.GetBytes;
        return x => mapping(coding(x).ToBitRead());
    }
    #endregion
}
