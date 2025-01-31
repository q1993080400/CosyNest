namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是<see cref="Masking"/>组件的渲染参数
/// </summary>
public sealed record RenderMaskingInfo
{
    #region 外部容器CSS
    /// <summary>
    /// 获取外部容器的CSS样式
    /// </summary>
    public required string ExternalCSS { get; init; }
    #endregion
    #region 内部容器CSS样式
    /// <summary>
    /// 获取内部容器的CSS样式，
    /// 如果为<see langword="null"/>，
    /// 表示不需要内部容器
    /// </summary>
    public required string? InternalCSS { get; init; }
    #endregion
}
