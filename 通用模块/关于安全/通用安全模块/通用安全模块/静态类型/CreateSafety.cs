using System.Security.Claims;
using System.Security.Principal;

namespace System.SafetyFrancis;

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
        return new ClaimsIdentity(c.Select(static x => new Claim(x.Item1, x.Item2)), authenticationType);
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
}
