using System.Design;
using System.IOFrancis.Bit;
using System.Security.Cryptography;

namespace System.SafetyFrancis.Algorithm;

/// <summary>
/// 这个类型可以通过BCL内置的<see cref="HashAlgorithm"/>来计算哈希值
/// </summary>
sealed class HashBCL : WithoutRelease
{
#pragma warning disable CS8618

    #region 是否开启优化
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则启用优化模式，能够提高计算小型明文的性能，
    /// 并且不会调用非托管代码，可以在Webassembly中使用，
    /// 但是代价是，在计算通过网络传输或长明文的哈希时容易卡死
    /// </summary>
    public bool Optimization { get; init; }
    #endregion
    #region 用来创建哈希算法的委托
    /// <summary>
    /// 该委托用来创建哈希算法对象
    /// </summary>
    public Func<HashAlgorithm> Create { get; init; }
    #endregion
    #region 计算哈希值
    /// <summary>
    /// 计算二进制数据的哈希值
    /// </summary>
    /// <param name="plaintext">用来读取二进制数据的管道</param>
    /// <returns></returns>
    public IBitRead CalculateHash(IBitRead plaintext)
    {
        #region 以异步流返回哈希值的本地函数
        async IAsyncEnumerable<byte[]> Fun()
        {
            using var hash = Create();
            if (Optimization)
            {
                yield return hash.ComputeHash(await plaintext.ReadComplete());
                yield break;
            }
            using var stream = plaintext.ToStream();
            yield return await hash.ComputeHashAsync(stream);
        }
        #endregion
        return Fun().ToBitRead();
    }
    #endregion
}
