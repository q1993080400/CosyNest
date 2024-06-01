namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来渲染表身中的一列的参数的基类
/// </summary>
/// <typeparam name="Model">表格的模型类型</typeparam>
public abstract record RenderBodyColumnsInfoBase<Model>
    where Model : class
{
    #region 行号
    /// <summary>
    /// 返回这一行的行号，从0开始
    /// </summary>
    public required int RowIndex { get; init; }
    #endregion
    #region 要渲染的模型
    /// <summary>
    /// 获取要渲染的模型
    /// </summary>
    public required Model TableModel { get; init; }
    #endregion
    #region 列的名字
    /// <summary>
    /// 获取列的名字
    /// </summary>
    public required string ColumnsName { get; init; }
    #endregion
    #region 渲染出的结果是否很长
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示最终渲染出的表身数据可能会很长，
    /// 它会影响到一些布局
    /// </summary>
    public required bool IsLong { get; init; }
    #endregion
}
