namespace System.DingDing;

/// <summary>
/// 这个记录是钉钉OA审批的操作记录
/// </summary>
public sealed record DingDingOAOperationRecord
{
    #region 操作人的ID
    /// <summary>
    /// 操作人的ID
    /// </summary>
    public required string OriginatorUserID { get; init; }
    #endregion
    #region 操作时间
    /// <summary>
    /// 操作时间
    /// </summary>
    public required DateTimeOffset Date { get; init; }
    #endregion
    #region 操作类型
    /// <summary>
    /// 操作类型
    /// </summary>
    public required DingDingOAOperationType OperationType { get; init; }
    #endregion
    #region 操作结果
    /// <summary>
    /// 操作结果
    /// </summary>
    public required DingDingOAOperationResult Result { get; init; }
    #endregion
    #region 评论内容
    /// <summary>
    /// 评论内容
    /// </summary>
    public required string Remark { get; init; }
    #endregion
    #region 评论附件
    /// <summary>
    /// 评论的附件
    /// </summary>
    public required IReadOnlyList<DingDingOACommentAttachment> Attachments { get; init; }
    #endregion
    #region 抄送人ID
    /// <summary>
    /// 获取抄送人的ID列表
    /// </summary>
    public required IReadOnlyList<string> CCUserID { get; init; }
    #endregion
}
