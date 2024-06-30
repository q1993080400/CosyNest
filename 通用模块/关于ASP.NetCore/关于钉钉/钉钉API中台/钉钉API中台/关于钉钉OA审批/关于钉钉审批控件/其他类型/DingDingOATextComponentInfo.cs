namespace System.DingDing;

/// <summary>
/// 这个记录代表钉钉审批中的文本控件
/// </summary>
public sealed record DingDingOATextComponentInfo : DingDingOAComponentInfo
{
    #region 控件文本
    /// <summary>
    /// 获取这个控件的文本
    /// </summary>
    public required string? Text { get; init; }
    #endregion
}
