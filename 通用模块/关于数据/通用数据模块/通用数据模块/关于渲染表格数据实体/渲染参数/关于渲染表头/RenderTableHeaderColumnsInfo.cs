using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来渲染表头中的一列的参数，
/// 它表示这一列映射到一个具体的属性
/// </summary>
public sealed record RenderTableHeaderColumnsInfo : RenderTableHeaderColumnsInfoBase
{
    #region 列对应的属性
    /// <summary>
    /// 获取列对应的属性
    /// </summary>
    public required PropertyInfo PropertyInfo { get; init; }
    #endregion
}
