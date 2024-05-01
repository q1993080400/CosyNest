namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录表示一个钉钉OA表单模板
/// </summary>
public sealed record DingDingOAFormTemplate
{
    #region 表单的名称
    /// <summary>
    /// 获取表单的名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
    #region 表单的ID
    /// <summary>
    /// 获取表单的ID
    /// </summary>
    public required string ID { get; init; }
    #endregion
}
