namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来渲染表格表头中的一列的参数
/// </summary>
public abstract record RenderTableHeaderColumnsInfoBase
{
    #region 列的名称
    /// <summary>
    /// 获取列的名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
}
