namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="Arrange"/>组件的参数
/// </summary>
public sealed record RenderArrangeInfo
{
    #region 容器CSS
    /// <summary>
    /// 获取容器的CSS，
    /// 它必须被赋值给容器元素的class
    /// </summary>
    public required string CSS { get; init; }
    #endregion
}
