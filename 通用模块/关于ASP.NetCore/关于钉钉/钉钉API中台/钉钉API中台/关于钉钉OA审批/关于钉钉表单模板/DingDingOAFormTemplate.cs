namespace System.DingDing;

/// <summary>
/// 这个记录表示一个钉钉OA表单模板
/// </summary>
public sealed record DingDingOAFormTemplate
{
    #region 名称
    /// <summary>
    /// 获取表单模板的名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
    #region ID
    /// <summary>
    /// 获取表单模板的ID
    /// </summary>
    public required string FormTemplateID { get; init; }
    #endregion
    #region Uri
    /// <summary>
    /// 获取表单模板的Uri
    /// </summary>
    public required string Uri { get; init; }
    #endregion
    #region 图标Uri
    /// <summary>
    /// 获取图标的Uri
    /// </summary>
    public required string IconUri { get; init; }
    #endregion
    #region 分组ID
    /// <summary>
    /// 获取表单模板的分组ID，
    /// 如果不存在，则为<see langword="null"/>
    /// </summary>
    public required string? GroupID { get; init; }
    #endregion
    #region 分组名称
    /// <summary>
    /// 获取表单模板的分组名称，
    /// 如果不存在，则为<see langword="null"/>
    /// </summary>
    public required string? GroupName { get; init; }
    #endregion
}
