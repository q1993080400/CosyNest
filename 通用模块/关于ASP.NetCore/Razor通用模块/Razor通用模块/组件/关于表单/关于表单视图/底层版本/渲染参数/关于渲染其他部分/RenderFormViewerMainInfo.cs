namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FormViewer{Model}"/>主体部分的参数，
/// 主体部分指的是表单的所有属性部分，不包括提交重置按钮等
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderFormViewerMainInfo<Model>
    where Model : class
{
    #region 当前模型
    /// <summary>
    /// 获取当前模型
    /// </summary>
    public required Model FormModel { get; init; }
    #endregion
    #region 枚举每个组的渲染成果
    /// <summary>
    /// 获取每个分组的渲染结果
    /// </summary>
    public required IEnumerable<RenderFormViewerRegionInfo<Model>> RenderGroup { get; init; }
    #endregion
}
