namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个布局依次排列元素，每个元素的宽度都相同
/// </summary>
public sealed partial class Arrange : ComponentBase, IContentComponent<RenderFragment<RenderArrangeInfo>>
{
    #region 组件参数
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderArrangeInfo> ChildContent { get; set; }
    #endregion
    #region 颗粒度
    /// <summary>
    /// 获取颗粒度，
    /// 它指示每一行应该有多少元素
    /// </summary>
    [Parameter]
    public ArrangeParticle Particle { get; set; } = ArrangeParticle.Medium;
    #endregion
    #region 替代的获取渲染参数的委托
    /// <summary>
    /// 这个委托可以用来代替默认方法生成渲染参数，
    /// 它的参数就是这个组件本身
    /// </summary>
    [Parameter]
    public Func<Arrange, RenderArrangeInfo>? GetRenderInfoReplace { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取渲染参数
    /// <summary>
    /// 获取渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderArrangeInfo GetRenderInfo()
    {
        var particle = Particle switch
        {
            ArrangeParticle.Minority => "arrangeMinority",
            ArrangeParticle.Medium => "arrangeMedium",
            ArrangeParticle.Most => "arrangeMost",
            var p => throw new NotSupportedException($"未能识别{p}类型的枚举")
        };
        var css = $"arrange {particle}";
        return new()
        {
            CSS = css,
        };
    }
    #endregion
    #endregion
}
