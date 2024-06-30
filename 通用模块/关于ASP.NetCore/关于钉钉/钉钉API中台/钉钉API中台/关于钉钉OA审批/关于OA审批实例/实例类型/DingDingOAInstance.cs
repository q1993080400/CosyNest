namespace System.DingDing;

/// <summary>
/// 这个记录表示一个钉钉OA审批的实例
/// </summary>
public sealed record DingDingOAInstance
{
    #region 实例ID
    /// <summary>
    /// 获取审批实例的ID
    /// </summary>
    public required string ID { get; init; }
    #endregion
    #region 标题
    /// <summary>
    /// 获取审批实例的标题
    /// </summary>
    public required string Title { get; init; }
    #endregion
    #region 创建时间
    /// <summary>
    /// 创建时间
    /// </summary>
    public required DateTimeOffset CreateTime { get; init; }
    #endregion
    #region 结束时间
    /// <summary>
    /// 结束时间
    /// </summary>
    public required DateTimeOffset? FinishTime { get; init; }
    #endregion
    #region 发起人的ID
    /// <summary>
    /// 发起人的ID
    /// </summary>
    public required string OriginatorUserID { get; init; }
    #endregion
    #region 发起部门的ID
    /// <summary>
    /// 发起部门的ID
    /// </summary>
    public required string OriginatorDeptID { get; init; }
    #endregion
    #region 发起部门的名称
    /// <summary>
    /// 发起部门的名称
    /// </summary>
    public required string OriginatorDeptName { get; init; }
    #endregion
    #region 状态
    /// <summary>
    /// 审批状态
    /// </summary>
    public required DingDingOAState State { get; init; }
    #endregion
    #region 审批人ID
    /// <summary>
    /// 所有审批人的ID
    /// </summary>
    public required IReadOnlyList<string> ApproverUserID { get; init; }
    #endregion
    #region 抄送人ID
    /// <summary>
    /// 所有抄送人的ID
    /// </summary>
    public required IReadOnlyList<string> CCUserID { get; init; }
    #endregion
    #region 是否同意
    /// <summary>
    /// 获取是否同意这个审批流程
    /// </summary>
    public required bool IsAgree { get; init; }
    #endregion
    #region 业务编号
    /// <summary>
    /// 获取审批实例业务编号
    /// </summary>
    public required string BusinessID { get; init; }
    #endregion
    #region 操作记录
    /// <summary>
    /// 获取操作记录列表
    /// </summary>
    public required IReadOnlyList<DingDingOAOperationRecord> OperationRecords { get; init; }
    #endregion
    #region 任务列表
    /// <summary>
    /// 获取任务列表
    /// </summary>
    public required IReadOnlyList<DingDingOATask> Tasks { get; init; }
    #endregion
    #region 业务动作
    /// <summary>
    /// 业务动作
    /// </summary>
    public required DingDingOABizAction BizAction { get; init; }
    #endregion
    #region 业务参数透出
    /// <summary>
    /// 业务参数透出
    /// </summary>
    public required string? BizData { get; init; }
    #endregion
    #region 审批附属实例ID
    /// <summary>
    /// 审批附属实例ID列表
    /// </summary>
    public required IReadOnlyList<string> AttachedInstanceID { get; init; }
    #endregion
    #region 主流程实例ID
    /// <summary>
    /// 主流程实例ID
    /// </summary>
    public required string? MainProcessInstanceID { get; init; }
    #endregion
    #region 表单组件详情列表
    /// <summary>
    /// 表单组件详情列表
    /// </summary>
    public required IReadOnlyList<DingDingOAFormComponentValueExtend> FormComponentValues { get; init; }
    #endregion
}
