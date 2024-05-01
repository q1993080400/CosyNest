namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录表示钉钉的App信息
/// </summary>
public sealed record DingDingAppInfo
{
    #region 应用ID
    /// <summary>
    /// 获取应用ID
    /// </summary>
    public required string ClientID { get; init; }
    #endregion
    #region 组织ID
    /// <summary>
    /// 获取这个应用所属于的组织的ID
    /// </summary>
    public required string OrganizationID { get; init; }
    #endregion
}
