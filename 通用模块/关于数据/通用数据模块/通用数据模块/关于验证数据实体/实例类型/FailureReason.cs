using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录用来封装验证失败的原因
/// </summary>
public sealed record FailureReason
{
    #region 验证失败的成员
    /// <summary>
    /// 验证失败的成员，
    /// 它一般是属性或类型（如果整个类型验证不通过）
    /// </summary>
    public required MemberInfo MemberInfo { get; init; }
    #endregion
    #region 验证失败的原因
    /// <summary>
    /// 返回验证失败的原因
    /// </summary>
    public required string Prompt { get; init; }
    #endregion
}
