using System.DataFrancis;

namespace System.DingDing;

/// <summary>
/// 这个枚举表示钉钉OA审批的状态
/// </summary>
public enum DingDingOAState
{
    /// <summary>
    /// 正在审批
    /// </summary>
    [RenderData(Name = "正在审批")]
    UnderApproval,
    /// <summary>
    /// 已撤销
    /// </summary>
    [RenderData(Name = "已撤销")]
    Rescinded,
    /// <summary>
    /// 审批完成
    /// </summary>
    [RenderData(Name = "审批完成")]
    ApprovalCompleted
}
