namespace System.DingDing;

/// <summary>
/// 这个记录表示钉钉部门中的一个员工角色
/// </summary>
public sealed record DingDingDepartmentRole
{
    #region 钉钉用户
    /// <summary>
    /// 对应的钉钉用户
    /// </summary>
    public required DingDingUserInfo DingDingUserInfo { get; init; }
    #endregion
    #region 是否为部门领导
    /// <summary>
    /// 获取是否为部门领导
    /// </summary>
    public required bool IsLeader { get; init; }
    #endregion
}
