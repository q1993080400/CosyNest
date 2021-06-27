using System.Collections.Generic;
using System.Design;
using System.IOFrancis.Bit;
using System.Text;

namespace System.SafetyFrancis.Algorithm
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以将数据加密
    /// </summary>
    public interface IEncryption : IDisposablePro
    {
        #region 执行加密
        /// <summary>
        /// 对明文进行加密，并返回读取密文的管道
        /// </summary>
        /// <param name="plaintext">用来读取明文的管道</param>
        /// <returns></returns>
        IBitRead Encryption(IBitRead plaintext);
        #endregion
        #region 执行加密，传入String
        /// <summary>
        /// 对字符串格式的明文进行加密，
        /// 并返回密文字节数组
        /// </summary>
        /// <param name="plaintext">待加密的明文</param>
        /// <param name="coding">将明文编码为字节数组的委托，
        /// 如果为<see langword="null"/>，则默认使用UTF16编码</param>
        /// <returns></returns>
        byte[] Encryption(string plaintext, Func<string, IEnumerable<byte>>? coding = null)
        {
            var bytes = coding is null ? Encoding.Unicode.GetBytes(plaintext) : coding(plaintext);
            return Encryption(bytes.ToBitRead()).ReadComplete().Result;
        }
        #endregion
    }
}
