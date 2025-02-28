namespace System.DingDing;

/// <summary>
/// 钉钉OA审批操作的结果
/// </summary>
public enum DingDingOAOperationResult
{
    /// <summary>
    /// 未处理
    /// </summary>
    [EnumDescribe(Describe = "未处理")]
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
    Refuse
}
