namespace System.DingDing;

/// <summary>
/// 这个枚举是钉钉OA审批任务的状态
/// </summary>
public enum DingDingOATaskState
{
    /// <summary>
    /// 未启动
    /// </summary>
    [EnumDescribe(Describe = "未启动")]
    NotStarted,
    /// <summary>
    /// 处理中
    /// </summary>
    [EnumDescribe(Describe = "处理中")]
    Processing,
    /// <summary>
    /// 暂停
    /// </summary>
    [EnumDescribe(Describe = "暂停")]
    Suspend,
    /// <summary>
    /// 取消
    /// </summary>
    [EnumDescribe(Describe = "取消")]
    Cancel,
    /// <summary>
    /// 完成
    /// </summary>
    [EnumDescribe(Describe = "完成")]
    Complete,
    /// <summary>
    /// 终止
    /// </summary>
    [EnumDescribe(Describe = "终止")]
    Termination
}
