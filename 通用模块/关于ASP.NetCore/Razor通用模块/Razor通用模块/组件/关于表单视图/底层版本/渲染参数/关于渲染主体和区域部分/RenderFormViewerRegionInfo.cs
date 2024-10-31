namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FormViewer{Model}"/>区域的参数，
/// 区域指的是处于同一组中的属性
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderFormViewerRegionInfo<Model>
    where Model : class
{
    #region 组的名字
    /// <summary>
    /// 获取属性组的名字
    /// </summary>
    public required string? GroupName { get; init; }
    #endregion
    #region 当前模型
    /// <summary>
    /// 获取当前模型
    /// </summary>
    public required Model FormModel { get; init; }
    #endregion
    #region 枚举组中的所有属性
    /// <summary>
    /// 这个集合的元素是一个元组，
    /// 它的第一个项是组中单个属性的渲染参数，
    /// 第二个项是这个属性的渲染结果
    /// </summary>
    public required IEnumerable<(RenderFormViewerPropertyInfoBase<Model> Info, RenderFragment Render)> RenderRegion { get; init; }
    #endregion
    #region 渲染全部
    /// <summary>
    /// 返回一个<see cref="RenderFragment"/>，
    /// 它可以用来直接渲染全部的属性
    /// </summary>
    /// <returns></returns>
    public RenderFragment RenderAllRegion()
        => x =>
        {
            foreach (var (_, render) in RenderRegion)
            {
                render(x);
            }
        };
    #endregion
}
