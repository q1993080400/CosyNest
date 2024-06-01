namespace System.DingDing;

/// <summary>
/// 这个类型代表钉钉OA审批金额组件的信息
/// </summary>
public sealed record DingDingOAAmountComponentInfo : DingDingOAComponentInfo
{
    #region 金额
    /// <summary>
    /// 获取金额，正数表示入账，负数表示出账
    /// </summary>
    public required decimal Amount { get; init; }
    #endregion
}
