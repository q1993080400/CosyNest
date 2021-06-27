using System.Design;
using System.IOFrancis.Bit;

namespace System.SafetyFrancis.Algorithm
{
    /// <summary>
    /// 这个类型是<see cref="ICryptology"/>的实现，
    /// 它会先取明文的哈希值，再对哈希值进行加密，
    /// 解密的结果也是哈希值
    /// </summary>
    class CryptologyHash : AutoRelease, ICryptology
    {
        #region 封装的对象
        #region 用来计算哈希值的对象
        /// <summary>
        /// 获取用来计算哈希值的对象
        /// </summary>
        private IHash PackHash { get; }
        #endregion
        #region 用来加解密的对象
        /// <summary>
        /// 获取用来加解密哈希值的对象
        /// </summary>
        private ICryptology PackCryptology { get; }
        #endregion
        #endregion
        #region 释放对象
        protected override void DisposeRealize()
        {
            PackHash.Dispose();
            PackCryptology.Dispose();
        }
        #endregion
        #region 执行加密
        public IBitRead Encryption(IBitRead plaintext)
        {
            var hash = PackHash.Encryption(plaintext);
            return PackCryptology.Encryption(hash);
        }
        #endregion
        #region 执行解密
        public IBitRead Decrypt(IBitRead ciphertext)
            => PackCryptology.Decrypt(ciphertext);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="hash">用来计算哈希值的对象</param>
        /// <param name="cryptology">用来加解密哈希值的对象</param>
        public CryptologyHash(IHash hash, ICryptology cryptology)
        {
            PackHash = hash;
            PackCryptology = cryptology;
        }
        #endregion
    }
}
