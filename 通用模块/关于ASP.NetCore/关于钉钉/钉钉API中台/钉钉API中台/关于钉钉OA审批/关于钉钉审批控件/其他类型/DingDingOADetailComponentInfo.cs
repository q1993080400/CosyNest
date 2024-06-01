namespace System.DingDing;

/// <summary>
/// 这个记录是钉钉OA审批明细控件的信息
/// </summary>
public sealed record DingDingOADetailComponentInfo : DingDingOAComponentInfo
{
    #region 所有子控件
    /// <summary>
    /// 获取所有子控件的信息，
    /// 它是一个嵌套数组，每一个元素代表了每一行
    /// </summary>
    public required IReadOnlyList<IReadOnlyList<DingDingOAFormComponentValue>> Son { get; init; }
    #endregion
}
