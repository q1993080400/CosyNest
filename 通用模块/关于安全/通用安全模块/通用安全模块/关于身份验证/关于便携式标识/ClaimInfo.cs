using System.Security.Claims;

namespace System.SafetyFrancis;

/// <summary>
/// 这个记录允许使用Json传递<see cref="Claim"/>，
/// 它解决了循环引用问题
/// </summary>
public sealed record ClaimInfo
{
    #region 静态方法：创建对象
    /// <summary>
    /// 通过声明创建对象
    /// </summary>
    /// <param name="claim">用来创建对象的声明</param>
    /// <returns></returns>
    public static ClaimInfo Create(Claim claim)
        => new()
        {
            Type = claim.Type,
            Issuer = claim.Issuer,
            OriginalIssuer = claim.OriginalIssuer,
            Value = claim.Value,
            ValueType = claim.ValueType
        };
    #endregion
    #region 声明类型
    /// <summary>
    /// 获取声明类型
    /// </summary>
    public required string Type { get; init; }
    #endregion
    #region 声明的值
    /// <summary>
    /// 获取声明的值
    /// </summary>
    public required string Value { get; init; }
    #endregion
    #region 声明的值类型
    /// <summary>
    /// 获取声明的值的类型
    /// </summary>
    public required string? ValueType { get; init; }
    #endregion
    #region 声明的颁发者
    /// <summary>
    /// 获取声明的颁发者
    /// </summary>
    public required string? Issuer { get; init; }
    #endregion
    #region 原始颁发者
    /// <summary>
    /// 获取声明的原始颁发者
    /// </summary>
    public required string? OriginalIssuer { get; init; }
    #endregion
    #region 转换为声明
    /// <summary>
    /// 将本对象转换为声明
    /// </summary>
    /// <returns></returns>
    public Claim ToClaim()
        => new(Type, Value, ValueType, Issuer, OriginalIssuer);
    #endregion
}
