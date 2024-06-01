namespace System.DingDing;

/// <summary>
/// 这个记录是基本的钉钉OA审批表单的详情，
/// 它仅能用于明细组件中的子组件
/// </summary>
public record DingDingOAFormComponentValue
{
    #region 组件名称
    /// <summary>
    /// 组件名称
    /// </summary>
    public required string? Name { get; init; }
    #endregion
    #region 标签值
    /// <summary>
    /// 标签的值
    /// </summary>
    public required string? Value { get; init; }
    #endregion
    #region 标签扩展值
    /// <summary>
    /// 标签扩展值
    /// </summary>
    public required string? ExtValue { get; init; }
    #endregion
    #region 组件类型
    /// <summary>
    /// 组件的类型
    /// </summary>
    public required DingDingOAComponentType ComponentType { get; init; }
    #endregion
    #region 组件的信息
    /// <summary>
    /// 获取组件的信息，
    /// 如果无法识别，则为<see langword="null"/>
    /// </summary>
    public DingDingOAComponentInfo? Info
        => Value is { } ?
        DingDingOAComponentInfo.Create(ComponentType, Value) : null;
    #endregion
}
