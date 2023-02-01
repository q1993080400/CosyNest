using System.IOFrancis.Bit;

namespace System.SafetyFrancis.Algorithm;

/// <summary>
/// 该记录封装了一个双向密码学算法，
/// 它可以同时进行加密与解密，
/// 注意：此处的加密和解密是广义上的，
/// 加密包括计算哈希，签名等，解密包括解密，解开签名等
/// </summary>
/// <param name="Encryption">用来加密的管道</param>
/// <param name="Decrypt">用来解密的管道</param>
public sealed record Cryptography(BitMapping Encryption, BitMapping Decrypt)
{
    #region 加密管道
    /// <summary>
    /// 用来加密的管道
    /// </summary>
    public BitMapping Encryption { get; init; } = Encryption;
    #endregion
    #region 解密管道
    /// <summary>
    /// 用来解密的管道
    /// </summary>
    public BitMapping Decrypt { get; init; } = Decrypt;
    #endregion
}
