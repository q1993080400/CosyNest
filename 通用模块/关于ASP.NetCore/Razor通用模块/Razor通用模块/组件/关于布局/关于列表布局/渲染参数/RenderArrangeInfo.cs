namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="Arrange"/>组件的参数
/// </summary>
public sealed record RenderArrangeInfo
{
    #region 容器样式
    /// <summary>
    /// 获取容器的样式，
    /// 它必须被赋值给容器元素的style
    /// </summary>
    public required string Style { get; init; }
    #endregion
}
