namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="TableViewer{Model}"/>的参数
/// </summary>
public sealed record RenderTableViewerInfo
{
    #region 渲染表头的委托
    /// <summary>
    /// 用来渲染表头的委托
    /// </summary>
    public required RenderFragment RenderHeader { get; init; }
    #endregion
    #region 渲染行的委托
    /// <summary>
    /// 枚举所有用来渲染行的委托
    /// </summary>
    public required IReadOnlyCollection<RenderFragment> RenderRow { get; init; }
    #endregion
    #region 列的数量
    /// <summary>
    /// 获取列的数量
    /// </summary>
    public required int ColumnsCount { get; init; }
    #endregion
    #region 用来给元素编号的对象
    /// <summary>
    /// 这个对象可以用来给待渲染的元素编号
    /// </summary>
    public required IElementNumber ElementNumber { get; init; }
    #endregion
}
