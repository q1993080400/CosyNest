namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个枚举指示<see cref="Arrange"/>组件每一列最多有多少元素
/// </summary>
public enum ArrangeParticle
{
    /// <summary>
    /// 只能拥有少数元素，
    /// 它会让每个元素占据更多的空间
    /// </summary>
    Minority,
    /// <summary>
    /// 可以拥有中等数量的元素
    /// </summary>
    Medium,
    /// <summary>
    /// 可以拥有较多数量的元素，
    /// 它可能会使每个元素变得更紧凑
    /// </summary>
    Most
}
