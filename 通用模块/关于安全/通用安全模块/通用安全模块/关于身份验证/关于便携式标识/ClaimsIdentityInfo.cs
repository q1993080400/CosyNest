using System.Security.Claims;

namespace System.SafetyFrancis;

/// <summary>
/// 这个记录允许使用Json传递<see cref="ClaimsIdentity"/>，
/// 它解决了循环引用问题
/// </summary>
public sealed record ClaimsIdentityInfo
{
    #region 静态方法：创建对象
    /// <summary>
    /// 通过声明创建对象
    /// </summary>
    /// <param name="claimsIdentity">用来创建对象的声明</param>
    /// <returns></returns>
    public static ClaimsIdentityInfo Create(ClaimsIdentity claimsIdentity)
        => new()
        {
            AuthenticationType = claimsIdentity.AuthenticationType,
            ClaimInfos = [.. claimsIdentity.Claims.Select(ClaimInfo.Create)]
        };
    #endregion
    #region 身份验证的类型
    /// <summary>
    /// 获取身份验证的类型
    /// </summary>
    public required string? AuthenticationType { get; init; }
    #endregion
    #region 所有声明
    /// <summary>
    /// 获取对象拥有的所有声明
    /// </summary>
    public required IReadOnlyCollection<ClaimInfo> ClaimInfos { get; init; }
    #endregion
    #region 转换为标识
    /// <summary>
    /// 将本对象转换为标识
    /// </summary>
    /// <returns></returns>
    public ClaimsIdentity ToIdentity()
        => new([.. ClaimInfos.Select(x => x.ToClaim())], AuthenticationType);
    #endregion
}
