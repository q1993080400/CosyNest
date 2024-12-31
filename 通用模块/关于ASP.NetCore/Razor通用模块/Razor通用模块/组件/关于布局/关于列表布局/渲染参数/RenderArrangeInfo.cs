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
    #region 颗粒度
    /// <summary>
    /// 获取颗粒度，
    /// 它指示每一行应该有多少元素
    /// </summary>
    public required ArrangeParticle Particle { get; init; }
    #endregion
}
