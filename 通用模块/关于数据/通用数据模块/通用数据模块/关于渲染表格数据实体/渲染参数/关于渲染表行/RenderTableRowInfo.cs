namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来渲染表格中一行的参数
/// </summary>
/// <typeparam name="Model">表格模型的类型</typeparam>
public sealed record RenderTableRowInfo<Model>
    where Model : class
{
    #region 要渲染的模型
    /// <summary>
    /// 获取要渲染的模型
    /// </summary>
    public required Model TableModel { get; init; }
    #endregion
    #region 所有列的渲染参数
    /// <summary>
    /// 获取所有列的渲染参数
    /// </summary>
    public required IReadOnlyCollection<RenderBodyColumnsInfoBase<Model>> RenderBodyColumnsInfo { get; init; }
    #endregion
    #region 行号
    /// <summary>
    /// 返回这一行的行号，从0开始
    /// </summary>
    public required int RowIndex { get; init; }
    #endregion
    #region ID
    /// <summary>
    /// 获取这一行的ID
    /// </summary>
    public required string ID { get; init; }
    #endregion
}
