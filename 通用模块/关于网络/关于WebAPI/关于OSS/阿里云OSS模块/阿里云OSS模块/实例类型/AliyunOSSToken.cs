namespace System.NetFrancis;

/// <summary>
/// 这个记录是用来访问阿里云OSS的凭证
/// </summary>
public sealed record AliyunOSSToken
{
    #region ID
    /// <summary>
    /// APP的ID
    /// </summary>
    public required string AaccessKeyID { get; init; }
    #endregion
    #region Secret
    /// <summary>
    /// APP的Secret
    /// </summary>
    public required string AccessKeySecret { get; init; }
    #endregion
    #region 终结点
    /// <summary>
    /// 获取终结点名称，
    /// 它是阿里云在世界各地OSS服务器的域名
    /// </summary>
    public required string EndPoint { get; init; }
    #endregion
    #region 存储池名称
    /// <summary>
    /// 获取存储池的名称
    /// </summary>
    public required string Bucket { get; init; }
    #endregion
}
