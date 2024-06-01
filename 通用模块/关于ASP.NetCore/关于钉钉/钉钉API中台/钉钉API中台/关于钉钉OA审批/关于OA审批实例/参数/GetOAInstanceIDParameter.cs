namespace System.DingDing;

/// <summary>
/// 这个记录是<see cref="DingDingWebApiOA.GetOAInstanceID(GetOAInstanceIDParameter)"/>的参数
/// </summary>
public sealed record GetOAInstanceIDParameter
{
    #region 表单模板ID
    /// <summary>
    /// 获取表单模板的ID
    /// </summary>
    public required string FormTemplateID { get; init; }
    #endregion
    #region 开始日期
    /// <summary>
    /// 获取要筛选的开始日期，
    /// 如果不填，默认为当前日期的120天以前
    /// </summary>
    public DateTimeOffset StartDate { get; init; } = StartDateLowerLimit;
    #endregion
    #region 静态成员：开始日期下限
    /// <summary>
    /// 获取开始日期的下限
    /// </summary>
    public static DateTimeOffset StartDateLowerLimit
        => DateTimeOffset.Now - TimeSpan.FromDays(120) + TimeSpan.FromSeconds(1);
    #endregion
    #region 结束日期
    /// <summary>
    /// 获取要筛选的结束日期
    /// </summary>
    public DateTimeOffset EndDate { get; init; } = DateTimeOffset.Now;
    #endregion
    #region 发起人ID
    /// <summary>
    /// 获取要筛选的发起人ID
    /// </summary>
    public IEnumerable<string> UserIDs { get; init; } = [];
    #endregion
    #region 审批状态
    /// <summary>
    /// 获取要筛选的审批状态
    /// </summary>
    public IEnumerable<DingDingOAState> States { get; init; } = [];
    #endregion
}
