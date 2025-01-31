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
    #region 是否填满剩余空间
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则最后一个子元素会填满这一行的所有剩余空间，
    /// 在这种情况下，不能使用gap属性，只能使用padding属性，否则会出现问题
    /// </summary>
    [Parameter]
    public bool FillRemainingSpace { get; set; }
    #endregion
    #endregion
    #region 用来控制每一行元素数量的CSS变量的名称
    /// <summary>
    /// 返回用来控制每一行元素数量的CSS变量的名称，
    /// 如果你需要显式控制这个参数，
    /// 可以将它拼接后赋值给父容器的style属性
    /// </summary>
    public const string ColumnsCountVariable = "--arrangeColumnsCount";
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
            ArrangeParticle.EspeciallyMany => "arrangeEspeciallyMany",
            var p => throw new NotSupportedException($"未能识别{p}类型的枚举")
        };
        var arrange = FillRemainingSpace ? "arrangeFill" : "arrange";
        var css = $"{arrange} {particle} arrangeDefinition";
        return new()
        {
            CSS = css,
            Particle = Particle
        };
    }
    #endregion
    #endregion
}
