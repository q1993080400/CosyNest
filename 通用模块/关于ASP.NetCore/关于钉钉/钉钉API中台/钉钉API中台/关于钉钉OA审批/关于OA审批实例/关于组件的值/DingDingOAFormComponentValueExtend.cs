namespace System.DingDing;

/// <summary>
/// 这个记录是完全的钉钉OA审批表单的详情
/// </summary>
public sealed record DingDingOAFormComponentValueExtend : DingDingOAFormComponentValue
{
    #region 组件ID
    /// <summary>
    /// 组件ID
    /// </summary>
    public required string ID { get; init; }
    #endregion
    #region 组件别名
    /// <summary>
    /// 组件别名
    /// </summary>
    public required string? BizAlias { get; init; }
    #endregion
}
