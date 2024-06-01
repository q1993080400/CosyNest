namespace System.DingDing;

/// <summary>
/// 这个记录可以用来下载钉钉OA审批附件
/// </summary>
public sealed record DingDingOAAttachmentDownloadInfo
{
    #region 钉盘空间ID
    /// <summary>
    /// 钉盘空间的ID
    /// </summary>
    public required string SpaceID { get; init; }
    #endregion
    #region 文件ID
    /// <summary>
    /// 文件ID
    /// </summary>
    public required string FileID { get; init; }
    #endregion
    #region 下载地址
    /// <summary>
    /// 文件下载地址
    /// </summary>
    public required string DownloadUri { get; init; }
    #endregion
    #region 过期时间
    /// <summary>
    /// 下载地址过期时间
    /// </summary>
    public required DateTimeOffset ExpirationDate { get; init; }
    #endregion
}
