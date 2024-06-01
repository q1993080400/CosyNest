using System.DataFrancis;

namespace System.DingDing;

/// <summary>
/// 这个枚举是钉钉OA审批任务的状态
/// </summary>
public enum DingDingOATaskState
{
    /// <summary>
    /// 未启动
    /// </summary>
    [RenderData(Name = "未启动")]
    NotStarted,
    /// <summary>
    /// 处理中
    /// </summary>
    [RenderData(Name = "处理中")]
    Processing,
    /// <summary>
    /// 暂停
    /// </summary>
    [RenderData(Name = "暂停")]
    Suspend,
    /// <summary>
    /// 取消
    /// </summary>
    [RenderData(Name = "取消")]
    Cancel,
    /// <summary>
    /// 完成
    /// </summary>
    [RenderData(Name = "完成")]
    Complete,
    /// <summary>
    /// 终止
    /// </summary>
    [RenderData(Name = "终止")]
    Termination
}
