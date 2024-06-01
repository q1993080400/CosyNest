namespace System.DingDing;

/// <summary>
/// 这个类型代表钉钉OA审批图片组件的信息
/// </summary>
public sealed record DingDingOAImageComponentInfo : DingDingOAComponentInfo
{
    #region 图片的Uri
    /// <summary>
    /// 获取所有图片的Uri
    /// </summary>
    public required IReadOnlyList<string> Uris { get; init; }
    #endregion
}
