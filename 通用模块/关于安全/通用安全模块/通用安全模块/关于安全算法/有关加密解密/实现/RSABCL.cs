using System.Collections.Generic;
using System.Design;
using System.IOFrancis.Bit;
using System.Security.Cryptography;

namespace System.SafetyFrancis.Algorithm
{
    /// <summary>
    /// 这个类型是<see cref="ICryptology"/>的实现，
    /// 可以视为一个非对称加密算法，
    /// 它在底层使用BCL自带的<see cref="RSA"/>
    /// </summary>
    class RSABCL : AutoRelease, ICryptology
    {
        #region 封装的密码算法
        /// <summary>
        /// 获取封装的非对称算法，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private RSA Algorithm { get; }
        #endregion
        #region 关于加密和解密
        #region 辅助方法
        /// <summary>
        /// 该方法为加密和解密提供统一抽象
        /// </summary>
        /// <param name="read">用来读取原始明文或密文的管道</param>
        /// <param name="fun">用来进行加密或解密的函数</param>
        /// <returns></returns>
        private static IBitRead Aided(IBitRead read, Func<byte[], RSAEncryptionPadding, byte[]> fun)
        {
            #region 本地函数
            async IAsyncEnumerable<byte[]> Fun()
            {
                yield return fun(await read.ReadComplete(), RSAEncryptionPadding.Pkcs1);
            }
            #endregion
            return Fun().ToBitRead();
        }
        #endregion
        #region 执行解密
        public IBitRead Decrypt(IBitRead ciphertext)
            => Aided(ciphertext, Algorithm.Decrypt);
        #endregion
        #region 执行加密
        public IBitRead Encryption(IBitRead plaintext)
            => Aided(plaintext, Algorithm.Encrypt);
        #endregion 
        #endregion
        #region 释放对象
        protected override void DisposeRealize()
            => Algorithm.Dispose();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的RSA算法初始化对象
        /// </summary>
        /// <param name="algorithm">指定的非对称算法，本对象的功能就是通过它实现的</param>
        public RSABCL(RSA algorithm)
        {
            this.Algorithm = algorithm;
        }
        #endregion
    }
}
