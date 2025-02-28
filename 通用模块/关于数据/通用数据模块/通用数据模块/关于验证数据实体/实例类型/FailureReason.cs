using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录用来封装验证失败的原因
/// </summary>
public sealed record FailureReason
{
    #region 验证失败的属性
    /// <summary>
    /// 验证失败的属性
    /// </summary>
    public required PropertyInfo Property { get; init; }
    #endregion
    #region 验证失败的原因
    /// <summary>
    /// 返回验证失败的原因
    /// </summary>
    public required string Prompt { get; init; }
    #endregion
}
