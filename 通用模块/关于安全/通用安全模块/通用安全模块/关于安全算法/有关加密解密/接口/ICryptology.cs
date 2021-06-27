using System.Collections.Generic;
using System.IOFrancis.Bit;
using System.Text;

namespace System.SafetyFrancis.Algorithm
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个密码算法，
    /// 它既可以加密数据，也可以解密数据
    /// </summary>
    public interface ICryptology : IEncryption
    {
        #region 执行解密
        /// <summary>
        /// 对密文进行解密，并返回读取明文的管道
        /// </summary>
        /// <param name="ciphertext">待解密的密文</param>
        /// <returns></returns>
        IBitRead Decrypt(IBitRead ciphertext);
        #endregion
        #region 执行解密，返回String
        /// <summary>
        /// 对密文进行解密，并以字符串的格式返回明文
        /// </summary>
        /// <param name="ciphertext">待解密的密文</param>
        /// <param name="decoding">用来将字节数组解码为字符串的委托，
        /// 如果为<see langword="null"/>，则默认使用UTF16进行解码</param>
        /// <returns></returns>
        string Decrypt(IEnumerable<byte> ciphertext, Func<IEnumerable<byte>, string>? decoding = null)
        {
            var plaintext = Decrypt(ciphertext.ToBitRead());
            var bytes = plaintext.ReadComplete().Result;
            return decoding is null ? Encoding.Unicode.GetString(bytes) : decoding(bytes);
        }
        #endregion
    }
}
