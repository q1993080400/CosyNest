namespace System.DingDing;

/// <summary>
/// 这个枚举表示钉钉OA审批的状态
/// </summary>
public enum DingDingOAState
{
    /// <summary>
    /// 正在审批
    /// </summary>
    [EnumDescribe(Describe = "正在审批")]
    UnderApproval,
    /// <summary>
    /// 已撤销
    /// </summary>
    [EnumDescribe(Describe = "已撤销")]
    Rescinded,
    /// <summary>
    /// 审批完成
    /// </summary>
    [EnumDescribe(Describe = "审批完成")]
    ApprovalCompleted
}
