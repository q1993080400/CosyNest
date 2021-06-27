using System.Collections.Generic;
using System.Design;
using System.IOFrancis.Bit;
using System.Security.Cryptography;

namespace System.SafetyFrancis.Algorithm
{
    /// <summary>
    /// 这个类型是<see cref="IHash"/>的实现，
    /// 它通过BCL内置的<see cref="HashAlgorithm"/>来计算哈希值
    /// </summary>
    /// <typeparam name="Hash">用来计算哈希值的BCL对象类型</typeparam>
    class HashBCL<Hash> : WithoutRelease, IHash
        where Hash : HashAlgorithm, new()
    {
        #region 是否开启优化
        /// <summary>
        /// 如果这个值为<see langword="true"/>，
        /// 则启用优化模式，能够提高计算小型明文的性能，
        /// 并且不会调用非托管代码，可以在Webassembly中使用
        /// </summary>
        public bool Optimization { get; init; }
        #endregion
        #region 计算哈希值
        public IBitRead Encryption(IBitRead plaintext)
        {
            #region 以异步流返回哈希值的本地函数
            async IAsyncEnumerable<byte[]> Fun()
            {
                using var hash = new Hash();
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
}
