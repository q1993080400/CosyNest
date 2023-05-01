using System.Design;
using System.IOFrancis.Bit;
using System.Security.Cryptography;

namespace System.SafetyFrancis.Algorithm;

/// <summary>
/// 这个类型是一个非对称加密算法，
/// 它在底层使用BCL自带的<see cref="RSA"/>
/// </summary>
sealed class RSAPack : AutoRelease
{
    #region 重要说明
    /*本类型只提供根据密钥加密和解密的功能，
      至于如何生成密钥，需要使用者自己解决*/
    #endregion
    #region 封装的对象
    #region 密码算法
    /// <summary>
    /// 获取封装的非对称算法，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private RSA Algorithm { get; }
    #endregion
    #region 是否拥有私钥
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则使用私钥进行加密转换，否则使用公钥
    /// </summary>
    private bool HasPrivate { get; }
    #endregion
    #endregion
    #region 执行转换
    /// <summary>
    /// 执行密码学转换，并返回读取转换结果的管道
    /// </summary>
    /// <param name="original">读取原文的管道</param>
    /// <returns></returns>
    public IBitRead Convert(IBitRead original)
    {
        #region 本地函数
        async IAsyncEnumerable<byte[]> Fun()
        {
            var bytes = await original.ReadComplete();
            var pkcs1 = RSAEncryptionPadding.Pkcs1;
            yield return HasPrivate ?
                Algorithm.Decrypt(bytes, pkcs1) :
                Algorithm.Encrypt(bytes, pkcs1);
        }
        #endregion
        return Fun().ToBitRead();
    }
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
        => Algorithm.Dispose();
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的密钥初始化对象
    /// </summary>
    /// <param name="key">以XML形式表现的密钥，
    /// 如果出现P，Q，D参数，则默认为私钥，否则为公钥</param>
    public RSAPack(string key)
    {
        Algorithm = RSA.Create();
        Algorithm.FromXmlString(key);
        HasPrivate = key.Contains("<P>") && key.Contains("<Q>") && key.Contains("<D>");
    }
    #endregion
}
