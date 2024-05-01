namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录封装了有关钉钉的配置
/// </summary>
public sealed record DingDingConfiguration
{
    #region 应用ID
    /// <summary>
    /// 获取应用的ID
    /// </summary>
    public required string ClientID { get; init; }
    #endregion
    #region 应用密钥
    /// <summary>
    /// 获取应用的密钥
    /// </summary>
    public required string ClientSecret { get; init; }
    #endregion
    #region 组织ID
    /// <summary>
    /// 获取钉钉的组织ID
    /// </summary>
    public required string OrganizationID { get; init; }
    #endregion
}
