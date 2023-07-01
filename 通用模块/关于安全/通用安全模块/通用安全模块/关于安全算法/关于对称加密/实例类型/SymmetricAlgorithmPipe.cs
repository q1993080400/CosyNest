using System.Design;
using System.IOFrancis;
using System.IOFrancis.Bit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace System.SafetyFrancis.Algorithm;

/// <summary>
/// 这个类型是表示一个对称加解密管道
/// </summary>
sealed class SymmetricAlgorithmPipe : WithoutRelease, IBitRead
{
    #region 公开成员
    #region 转换为Stream
    public Stream ToStream()
        => CreateIO.StreamEnumerable(Read());
    #endregion
    #region 对管道的描述
    public string? Describe => Source?.Describe;
    #endregion
    #region 管道的格式
    public string? Format => null;
    #endregion
    #region 管道的长度
    public long? Length
        => null;
    #endregion
    #region 读取管道
    #region 正式方法
    public async IAsyncEnumerable<byte[]> Read(int bufferSize = 1024, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        using var stream = IsEncryptor ? await Encryptor(cancellation) : await Decryptor(cancellation);
        await foreach (var item in stream.ToBitPipe().Read.Read(bufferSize, cancellation))
        {
            yield return item;
        }
    }
    #endregion
    #region 读取加密管道
    /// <summary>
    /// 获取一个<see cref="Stream"/>，
    /// 它填充了加密后的数据
    /// </summary>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    private async Task<Stream> Encryptor(CancellationToken cancellation)
    {
        using var algorithm = AlgorithmFactory();
        using var encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
        using var stream = new MemoryStream();
        using var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(await Source.ReadComplete(cancellation));
        cryptoStream.Close();
        return new MemoryStream(stream.ToArray());
    }
    #endregion
    #region 读取解密管道
    /// <summary>
    /// 获取一个<see cref="Stream"/>，
    /// 它填充了解密后的数据
    /// </summary>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    private async Task<Stream> Decryptor(CancellationToken cancellation)
    {
        using var algorithm = AlgorithmFactory();
        using var decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
        using var stream = new MemoryStream();
        stream.Write(await Source.ReadComplete(cancellation));
        stream.Reset();
        using var cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
        return await cryptoStream.CopyToMemory();
    }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 待转换的原始管道
    /// <summary>
    /// 获取待转换的原始管道
    /// </summary>
    internal required IBitRead Source { get; init; }
    #endregion
    #region 对称加密算法工厂
    /// <summary>
    /// 获取对称加密算法的工厂，
    /// 本对象的功能依赖于它的返回值，为正确使用本类型，
    /// 它返回的<see cref="SymmetricAlgorithm"/>的IV和Key必须相同
    /// </summary>
    internal required Func<SymmetricAlgorithm> AlgorithmFactory { get; init; }
    #endregion
    #region 是否加密
    /// <summary>
    /// 如果这个值为<see langword="true"/>，表示是加密，否则为解密
    /// </summary>
    internal required bool IsEncryptor { get; init; }
    #endregion
    #endregion
}
