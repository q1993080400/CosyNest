namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个记录表示一个可以用来登录亚马逊WebAPI的凭证
/// </summary>
public sealed record AmazonCredentialsInfo
{
    #region 访问ID
    /// <summary>
    /// 获取访问ID
    /// </summary>
    public required string AccessKeyID { get; init; }
    #endregion
    #region 访问密钥
    /// <summary>
    /// 获取访问密钥
    /// </summary>
    public required string SecretAccessKey { get; init; }
    #endregion
}
