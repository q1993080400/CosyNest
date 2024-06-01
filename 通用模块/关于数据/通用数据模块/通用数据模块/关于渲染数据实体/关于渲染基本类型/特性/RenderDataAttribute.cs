namespace System.DataFrancis;

/// <summary>
/// 这个特性可以用来描述如何渲染数据
/// </summary>
public sealed class RenderDataAttribute : RenderDataBaseAttribute
{
    #region 显示名称
    /// <summary>
    /// 要在UI上显示的名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
}
