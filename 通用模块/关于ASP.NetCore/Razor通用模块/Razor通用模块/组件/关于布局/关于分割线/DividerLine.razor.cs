namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件表示一个分割线
/// </summary>
public sealed partial class DividerLine : ComponentBase
{
    #region 组件参数
    #region 获取是否垂直
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示是一条垂直分割线，否则是一条水平分割线
    /// </summary>
    [Parameter]
    public bool IsVertical { get; set; }
    #endregion
    #region 用于渲染说明的委托
    /// <summary>
    /// 用于渲染说明的委托，
    /// 说明会被放在分割线的中间
    /// </summary>
    [Parameter]
    public RenderFragment? RenderDescription { get; set; }
    #endregion
    #endregion
}
