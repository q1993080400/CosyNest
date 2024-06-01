namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来渲染表格中表头的参数
/// </summary>
public sealed record RenderTableHeaderInfo
{
    #region 用来渲染表头每一列的委托
    /// <summary>
    /// 这个集合的元素可以用来渲染表头的每一列
    /// </summary>
    public required IReadOnlyCollection<RenderTableHeaderColumnsInfoBase> RenderHeaderColumns { get; init; }
    #endregion
}
