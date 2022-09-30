namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本组件是一个导航栏控件
/// </summary>
public sealed partial class NavigationBar<Obj> : ComponentBase
{
    #region 组件参数
    #region 导航栏的每一项
#pragma warning disable BL0007
    private IEnumerable<Obj> ItemsField;

    /// <summary>
    /// 获取或设置导航栏的每一项
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<Obj> Items
    {
        get => ItemsField;
        set => ItemsField = value.ToArray();
    }
#pragma warning restore
    #endregion
    #region 项模板
    /// <summary>
    /// 获取用来呈现导航栏中每一项的模板
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<Obj> ItemTemplate { get; set; }
    #endregion
    #region 连接符
    /// <summary>
    /// 获取用来呈现连接符的模板
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment DelimiterTemplate { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 这个字典用来接收参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }
    #endregion
    #endregion 
}
