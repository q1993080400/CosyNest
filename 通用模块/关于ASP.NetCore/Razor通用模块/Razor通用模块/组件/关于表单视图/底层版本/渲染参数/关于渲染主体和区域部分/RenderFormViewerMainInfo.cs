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
    #region 渲染全部
    /// <summary>
    /// 返回一个<see cref="RenderFragment"/>，
    /// 它可以用来直接渲染全部分组的属性
    /// </summary>
    /// <param name="firstRenderNotGroup">如果这个值为<see langword="true"/>，
    /// 则先渲染无分组的属性，否则先渲染各个分组</param>
    /// <returns></returns>
    public RenderFragment RenderAllGroup(bool firstRenderNotGroup)
    {
        RenderFragment[] fragments =
            [
               RenderNotGroup,
                x=>
                {
                    foreach (var (_,renderGroup) in RenderGroup.OrderBy(x=>x.Key))
                    {
                        renderGroup(x);
                    }
                }
            ];
        var renderFragments = firstRenderNotGroup ? fragments : fragments.Reverse().ToArray();
        return x =>
        {
            foreach (var fragment in renderFragments)
            {
                fragment(x);
            }
        };
    }
    #endregion
}
