using System;
using System.Collections.Generic;
using System.IOFrancis.Bit;
using System.Linq;
using System.Threading.Tasks;

namespace System.SafetyFrancis.Algorithm
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个哈希算法
    /// </summary>
    public interface IHash : IEncryption
    {
        #region 验证哈希值
        /// <summary>
        /// 验证哈希值是否相符
        /// </summary>
        /// <param name="data">待验证的数据</param>
        /// <param name="hash">用来作为对比的哈希值，
        /// 函数会对比这个参数和<paramref name="data"/>的哈希值是否完全一致</param>
        /// <returns>如果哈希值一致，则为<see langword="true"/>，否则为<see langword="false"/></returns>
        async Task<bool> Verify(IBitRead data, IEnumerable<byte> hash)
        {
            var ciphertext = Encryption(data);
            return hash.SequenceEqual(await ciphertext.ReadComplete());
        }
        #endregion
    }
}
