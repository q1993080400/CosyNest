using System.DataFrancis;

namespace System.DingDing;

/// <summary>
/// 钉钉OA审批任务的结果
/// </summary>
public enum DingDingOATaskResult
{
    /// <summary>
    /// 未确定
    /// </summary>
    [RenderData(Name = "未确定")]
    None,
    /// <summary>
    /// 同意
    /// </summary>
    [RenderData(Name = "同意")]
    Agree,
    /// <summary>
    /// 拒绝
    /// </summary>
    [RenderData(Name = "拒绝")]
    Refuse,
    /// <summary>
    /// 转交
    /// </summary>
    [RenderData(Name = "转交")]
    Transfer
}
