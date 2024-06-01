namespace System.DingDing;

/// <summary>
/// 钉钉OA审批实例业务动作
/// </summary>
public enum DingDingOABizAction
{
    /// <summary>
    /// 表示正常发起
    /// </summary>
    None,
    /// <summary>
    /// 表示该审批实例是基于原来的实例修改而来
    /// </summary>
    Modify,
    /// <summary>
    /// 表示该审批实例是由原来的实例撤销后重新发起的
    /// </summary>
    Revoke
}
