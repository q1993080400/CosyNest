namespace System.DingDing;

/// <summary>
/// 钉钉OA审批任务的结果
/// </summary>
public enum DingDingOATaskResult
{
    /// <summary>
    /// 未确定
    /// </summary>
    [EnumDescribe(Describe = "未确定")]
    None,
    /// <summary>
    /// 同意
    /// </summary>
    [EnumDescribe(Describe = "同意")]
    Agree,
    /// <summary>
    /// 拒绝
    /// </summary>
    [EnumDescribe(Describe = "拒绝")]
    Refuse,
    /// <summary>
    /// 转交
    /// </summary>
    [EnumDescribe(Describe = "转交")]
    Transfer
}
