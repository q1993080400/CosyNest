using System.IOFrancis.Bit;
using System.SafetyFrancis.Algorithm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace System.SafetyFrancis;

/// <summary>
/// 这个静态类可以用来创建有关安全的对象
/// </summary>
public static class CreateSafety
{
    #region 辅助方法：将字符串编码为UTF8
    /// <summary>
    /// 将字符串编码为UTF8，
    /// 并返回读取编码结果的管道
    /// </summary>
    /// <param name="text">待编码的字符串</param>
    /// <returns></returns>
    private static IBitRead Coding(string text)
        => Encoding.UTF8.GetBytes(text).ToBitRead();
    #endregion
    #region 创建IIdentity
    /// <summary>
    /// 使用指定的验证类型，用户名和声明创建<see cref="IIdentity"/>对象
    /// </summary>
    /// <param name="authenticationType">身份验证的类型，如果为<see langword="null"/>，代表未通过验证</param>
    /// <param name="name">用户的名称，如果为<see langword="null"/>，代表未通过验证</param>
    /// <param name="claims">枚举该用户所有声明的键和值</param>
    /// <returns></returns>
    public static ClaimsIdentity Identity(string? authenticationType, string? name, params (string Type, string Value)[] claims)
    {
        var c = name is null ? claims : claims.Append((ClaimsIdentity.DefaultNameClaimType, name));
        return new ClaimsIdentity(c.Select(x => new Claim(x.Item1, x.Item2)), authenticationType);
    }
    #endregion
    #region 创建ClaimsPrincipal 
    #region 返回未通过验证的ClaimsPrincipal 
    /// <summary>
    /// 返回一个未通过验证的<see cref="ClaimsPrincipal"/>
    /// </summary>
    public static ClaimsPrincipal PrincipalDefault()
        => new(new ClaimsIdentity());
    #endregion
    #region 使用主标识创建ClaimsPrincipal
    /// <summary>
    /// 使用指定的验证类型，用户名和声明创建一个<see cref="ClaimsIdentity"/>，
    /// 然后用它创建一个<see cref="ClaimsPrincipal"/>对象
    /// </summary>
    /// <param name="authenticationType">身份验证的类型，如果为<see langword="null"/>，代表未通过验证</param>
    /// <param name="name">用户的名称，如果为<see langword="null"/>，代表未通过验证</param>
    /// <param name="claims">枚举该用户所有声明的键和值</param>
    /// <returns></returns>
    public static ClaimsPrincipal Principal(string? authenticationType, string? name, params (string Type, string Value)[] claims)
        => new(Identity(authenticationType, name, claims));
    #endregion
    #endregion
    #region 关于哈希值
    #region 创建计算哈希值的管道
    #region 计算哈希值的管道
    /// <summary>
    /// 创建一个可以计算哈希值的管道，
    /// 它在内部使用<see cref="HashAlgorithm"/>来计算哈希值
    /// </summary>
    /// <param name="optimization">如果这个值为<see langword="true"/>，
    /// 则启用优化模式，能够提高计算小型明文的性能，并且不会调用非托管代码，可以在Webassembly中使用</param>
    /// <param name="create">该委托用来创建哈希算法对象，请务必保证每调用一次，都会创建一个新的对象</param>
    /// <returns>该委托的输入是读取原始二进制数据的管道，返回值是读取计算后哈希值的管道</returns>
    public static BitMapping Hash(bool optimization = false, Func<HashAlgorithm>? create = null)
        => new HashPack()
        {
            Optimization = optimization,
            Create = create ?? SHA512.Create
        }.CalculateHash;
    #endregion
    #region 计算字符串的哈希值
    /// <summary>
    /// 返回一个用来计算字符串的哈希值的管道
    /// </summary>
    /// <param name="hash">该委托用于计算哈希值，
    /// 如果为<see langword="null"/>，则使用默认方法</param>
    /// <param name="coding">该委托传入待计算哈希值的字符串，
    /// 返回读取该字符串二进制形式的管道，如果为<see langword="null"/>，默认使用UTF16编码</param>
    /// <returns></returns>
    public static Func<string, Task<byte[]>> HashText(BitMapping? hash = null, Func<string, IBitRead>? coding = null)
    {
        coding ??= Coding;
        hash ??= Hash();
        return x => hash(coding(x)).ReadComplete();
    }
    #endregion
    #endregion
    #region 创建验证哈希值的管道
    #region 验证字节数组
    /// <summary>
    /// 创建一个可以用来验证哈希值的管道
    /// </summary>
    /// <param name="compared">用来作为对比的哈希值，
    /// 如果它的校验和与计算出来的哈希值一致，则验证通过</param>
    /// <returns></returns>
    /// <inheritdoc cref="HashText(BitMapping?, Func{string, IBitRead}?)"/>
    public static BitVerify HashVerify(byte[] compared, BitMapping? hash = null)
    {
        var sum = compared.Checksum();
        hash ??= Hash();
        return async read =>
        {
            var h = await hash(read).ReadComplete();
            return sum == h.Checksum();
        };
    }
    #endregion
    #region 验证字符串
    /// <summary>
    /// 创建一个用于验证字符串哈希值的管道，
    /// 它的参数就是待验证的字符串，返回值就是验证结果
    /// </summary>
    /// <param name="verify">用来验证管道的委托</param>
    /// <returns></returns>
    /// <inheritdoc cref="HashText(BitMapping, Func{string, IBitRead})"/>
    public static Func<string, Task<bool>> HashVerifyText(BitVerify verify, Func<string, IBitRead>? coding = null)
    {
        coding ??= Coding;
        return x => verify(coding(x));
    }
    #endregion
    #endregion
    #endregion
    #region 创建非对称加密管道
    #region 使用RSA
    /// <summary>
    /// 使用RSA非对称算法创建一个<see cref="BitMapping"/>
    /// </summary>
    /// <returns>该管道可以使用RSA加密或解密数据</returns>
    /// <inheritdoc cref="RSAPack(string)"/>
    public static BitMapping RSA(string key)
        => new RSAPack(key).Convert;
    #endregion
    #endregion
    #region 关于对称加密管道
    #region 创建对称加密算法工厂
    #region 使用字节数组
    /// <summary>
    /// 创建对称加密算法工厂，
    /// 它可以用来返回具备相同IV和Key参数的对称算法对象
    /// </summary>
    /// <param name="iv">初始化向量</param>
    /// <param name="key">密钥</param>
    /// <returns></returns>
    public static Func<SymmetricAlgorithm> AlgorithmFactory(byte[] iv, byte[] key)
        => () =>
        {
            var algorithm = Aes.Create();
            algorithm.IV = iv;
            algorithm.Key = key;
            return algorithm;
        };
    #endregion
    #region 使用字符串
    /// <param name="encoding">一个用来将字符串转换为字节数组的函数，
    /// 如果为<see langword="null"/>，默认使用Base64转换</param>
    /// <inheritdoc cref="AlgorithmFactory(byte[], byte[])"/>
    public static Func<SymmetricAlgorithm> AlgorithmFactory(string iv, string key, Func<string, byte[]>? encoding = null)
    {
        encoding ??= Convert.FromBase64String;
        return AlgorithmFactory(encoding(iv), encoding(key));
    }
    #endregion
    #endregion
    #region 创建转换字节数组的管道
    /// <summary>
    /// 创建一个通过对称算法，
    /// 加密或解密字节数组的双向管道
    /// </summary>
    /// <param name="algorithmFactory">对称加密算法的工厂，
    /// 本函数的功能依赖于它的返回值，为正确使用本函数，
    /// 它返回的<see cref="Security.Cryptography.SymmetricAlgorithm"/>的IV和Key必须相同</param>
    /// <returns></returns>
    public static Cryptography SymmetricAlgorithm(Func<SymmetricAlgorithm> algorithmFactory)
    {
        #region 加密本地函数
        IBitRead Encryption(IBitRead x) => new SymmetricAlgorithmPipe()
        {
            AlgorithmFactory = algorithmFactory,
            IsEncryptor = true,
            Source = x
        };
        #endregion
        #region 解密本地函数
        IBitRead Decrypt(IBitRead x) => new SymmetricAlgorithmPipe()
        {
            AlgorithmFactory = algorithmFactory,
            IsEncryptor = false,
            Source = x
        };
        #endregion
        return new Cryptography(Encryption, Decrypt);
    }
    #endregion
    #region 创建转换字符串的管道
    /// <summary>
    /// 创建一个通过对称算法，
    /// 加密或解密字符串的双向管道，
    /// 字符串使用Base64编码
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="SymmetricAlgorithm(Func{SymmetricAlgorithm})"/>
    public static (Func<string, Task<string>> Encryption, Func<string, Task<string>> Decrypt) SymmetricAlgorithmText
        (Func<SymmetricAlgorithm> algorithmFactory)
    {
        var (encryption, decrypt) = SymmetricAlgorithm(algorithmFactory);
        return (encryption.ConvertText(),
            decrypt.ConvertText(Convert.FromBase64String, Encoding.UTF8.GetString));
    }
    #endregion
    #endregion
}
