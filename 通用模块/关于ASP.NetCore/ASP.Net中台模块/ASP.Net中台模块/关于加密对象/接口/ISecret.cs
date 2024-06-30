using Microsoft.AspNetCore.DataProtection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将自身加密或解密
/// </summary>
public interface ISecret<out Obj>
{
    #region 是否加密
    /// <summary>
    /// 获取自身是否加密
    /// </summary>
    bool IsEncryption { get; }
    #endregion
    #region 加密对象
    /// <summary>
    /// 加密对象
    /// </summary>
    /// <param name="dataProtector">用来加密的对象</param>
    /// <returns></returns>
    Obj Encryption(IDataProtector dataProtector);
    #endregion
    #region 解密对象
    /// <summary>
    /// 解密这个对象
    /// </summary>
    /// <param name="dataProtector">用来解密的对象</param>
    /// <returns></returns>
    Obj Decryption(IDataProtector dataProtector);
    #endregion
}
