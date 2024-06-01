namespace System.DingDing;

/// <summary>
/// 这个类型代表钉钉OA审批附件组件的信息
/// </summary>
public sealed record DingDingOAAttachmentComponentInfo : DingDingOAComponentInfo
{
    #region 附件信息
    /// <summary>
    /// 钉钉附件的信息
    /// </summary>
    public required IReadOnlyList<DingDingOAAttachmentInfo> AttachmentInfo { get; init; }
    #endregion
}
