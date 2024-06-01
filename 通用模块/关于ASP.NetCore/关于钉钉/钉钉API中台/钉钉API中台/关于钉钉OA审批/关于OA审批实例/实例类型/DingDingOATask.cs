namespace System.DingDing;

/// <summary>
/// 这个记录是一个钉钉OA审批的任务
/// </summary>
public sealed record DingDingOATask
{
    #region 任务ID
    /// <summary>
    /// 任务的ID
    /// </summary>
    public required string TaskID { get; init; }
    #endregion
    #region 任务处理人
    /// <summary>
    /// 任务处理人
    /// </summary>
    public required string UserID { get; init; }
    #endregion
    #region 状态
    /// <summary>
    /// 任务的状态
    /// </summary>
    public required DingDingOATaskState State { get; init; }
    #endregion
    #region 任务的结果
    /// <summary>
    /// 任务的结果
    /// </summary>
    public required DingDingOATaskResult Result { get; init; }
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
    #region 移动端任务Uri
    /// <summary>
    /// 移动端任务Uri
    /// </summary>
    public required string MobileUri { get; init; }
    #endregion
    #region PC端任务Uri
    /// <summary>
    /// PC端任务Uri
    /// </summary>
    public required string PCUri { get; init; }
    #endregion
    #region 实例ID
    /// <summary>
    /// 实例ID
    /// </summary>
    public required string InstanceID { get; init; }
    #endregion
    #region 任务节点ID
    /// <summary>
    /// 任务节点ID
    /// </summary>
    public required string ActivityID { get; init; }
    #endregion
}
