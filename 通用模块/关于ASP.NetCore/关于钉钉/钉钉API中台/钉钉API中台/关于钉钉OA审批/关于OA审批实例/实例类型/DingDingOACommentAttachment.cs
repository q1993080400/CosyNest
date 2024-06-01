namespace System.DingDing;

/// <summary>
/// 这个记录是钉钉OA审批评论的附件
/// </summary>
public sealed record DingDingOACommentAttachment
{
    #region 附件名称
    /// <summary>
    /// 附件名称
    /// </summary>
    public required string FileName { get; init; }
    #endregion
    #region 附件大小
    /// <summary>
    /// 附件大小
    /// </summary>
    public required string FileSize { get; init; }
    #endregion
    #region 附件ID
    /// <summary>
    /// 附件ID
    /// </summary>
    public required string FileID { get; init; }
    #endregion
    #region 附件类型
    /// <summary>
    /// 附件类型
    /// </summary>
    public required string FileType { get; init; }
    #endregion
}
