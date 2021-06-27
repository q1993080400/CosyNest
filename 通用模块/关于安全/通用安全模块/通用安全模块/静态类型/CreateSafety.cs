using System.Linq;
using System.SafetyFrancis.Algorithm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace System.SafetyFrancis
{
    /// <summary>
    /// 这个静态类可以用来创建有关安全的对象
    /// </summary>
    public static class CreateSafety
    {
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
        public static ClaimsPrincipal PrincipalDefault { get; } = new(new ClaimsIdentity());
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
        #region 创建IHash
        #region 可指定哈希算法
        /// <summary>
        /// 创建一个<see cref="IHash"/>，
        /// 它在内部使用<see cref="HashAlgorithm"/>来计算哈希值
        /// </summary>
        /// <typeparam name="Hash">用于计算哈希值的算法</typeparam>
        /// <param name="optimization">如果这个值为<see langword="true"/>，
        /// 则启用优化模式，能够提高计算小型明文的性能，并且不会调用非托管代码，可以在Webassembly中使用</param>
        /// <returns></returns>
        public static IHash Hash<Hash>(bool optimization = false)
            where Hash : HashAlgorithm, new()
            => new HashBCL<Hash>() { Optimization = optimization };
        #endregion
        #region 使用SHA512Managed
        /// <summary>
        /// 创建一个<see cref="IHash"/>，
        /// 它使用<see cref="SHA512Managed"/>作为算法
        /// </summary>
        /// <param name="optimization">如果这个值为<see langword="true"/>，
        /// 则启用优化模式，能够提高计算小型明文的性能，并且不会调用非托管代码，可以在Webassembly中使用</param>
        /// <returns></returns>
        public static IHash Hash(bool optimization = false)
            => Hash<SHA512Managed>(optimization);
        #endregion
        #endregion
        #region 创建ICryptology
        #region 使用RSA
        /// <summary>
        /// 使用RSA非对称算法创建<see cref="ICryptology"/>
        /// </summary>
        /// <param name="algorithm">用于执行算法的对象，必须已经导入密钥</param>
        /// <returns></returns>
        public static ICryptology CryptologyRSA(RSA algorithm)
            => new RSABCL(algorithm);
        #endregion
        #endregion
        #region 创建IEncryption
        #region 先计算哈希，然后加密
        /// <summary>
        /// 创建一个<see cref="ICryptology"/>，
        /// 它先对明文计算哈希值，然后对哈希值加密，
        /// 解密的结果也是哈希值，这种操作常用于传递密码
        /// </summary>
        /// <param name="hash">用于计算哈希值的对象</param>
        /// <param name="cryptology">用于对哈希值进行加解密的对象</param>
        /// <returns></returns>
        public static ICryptology EncryptionHash(IHash hash, ICryptology cryptology)
            => new CryptologyHash(hash, cryptology);
        #endregion
        #endregion
    }
}
