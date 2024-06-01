namespace System.DingDing;

/// <summary>
/// 这个类型是钉钉OA审批附件的信息
/// </summary>
public sealed record DingDingOAAttachmentInfo
{
    #region 钉盘ID
    /// <summary>
    /// 钉盘ID
    /// </summary>
    public required string SpaceID { get; init; }
    #endregion
    #region 文件名
    /// <summary>
    /// 文件名
    /// </summary>
    public required string FileName { get; init; }
    #endregion
    #region 文件大小
    /// <summary>
    /// 文件大小，单位不明
    /// </summary>
    public required long FileSize { get; init; }
    #endregion
    #region 文件类型
    /// <summary>
    /// 文件类型
    /// </summary>
    public required string FileType { get; init; }
    #endregion
    #region 文件ID
    /// <summary>
    /// 文件ID
    /// </summary>
    public required string FileID { get; init; }
    #endregion
}
