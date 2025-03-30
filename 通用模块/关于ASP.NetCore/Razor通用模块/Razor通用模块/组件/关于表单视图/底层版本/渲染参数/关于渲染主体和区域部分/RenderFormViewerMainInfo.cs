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
    #region 索引每个分组的渲染成果
    /// <summary>
    /// 按分组名称，索引每个分组的渲染成果
    /// </summary>
    public required IReadOnlyDictionary<string, RenderFragment> RenderGroup { get; init; }
    #endregion
    #region 获取无分组的属性的渲染结果
    /// <summary>
    /// 获取无分组的属性的渲染结果
    /// </summary>
    public required RenderFragment RenderNotGroup { get; init; }
    #endregion
    #region 获取所有渲染组
    /// <summary>
    /// 获取所有的渲染组
    /// </summary>
    /// <param name="firstRenderNotGroup">如果这个值为<see langword="true"/>，
    /// 则将无分组排在前面，否则排在后面</param>
    /// <returns></returns>
    public IEnumerable<RenderFragment> AllGroup(bool firstRenderNotGroup)
    {
        var group = RenderGroup.OrderBy(x => x.Key).Select(x => x.Value);
        return firstRenderNotGroup ?
        [RenderNotGroup, .. group] :
        [.. group, RenderNotGroup];
    }
    #endregion
    #region 渲染全部
    /// <summary>
    /// 返回一个<see cref="RenderFragment"/>，
    /// 它可以用来直接渲染全部分组的属性
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="AllGroup(bool)"/>
    public RenderFragment RenderAllGroup(bool firstRenderNotGroup)
        => AllGroup(firstRenderNotGroup).MergeRender();
    #endregion
}
