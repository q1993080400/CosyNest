using System.DataFrancis;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件为搜索提供帮助，
/// 它可以自动生成一个<see cref="DataFilterDescription{Obj}"/>
/// </summary>
/// <typeparam name="Obj">实体类的类型</typeparam>
public sealed partial class SearchViewer<Obj> : ComponentBase, IContentComponent<RenderFragment<RenderSearchViewerInfo<Obj>>>
{
    #region 组件参数
    #region 子内容
    /// <summary>
    /// 获取组件的子内容
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<RenderSearchViewerInfo<Obj>>? ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 渲染参数
    /// <summary>
    /// 获取这个组件的渲染参数
    /// </summary>
    private RenderSearchViewerInfo<Obj> Info { get; } = new();
    #endregion
    #endregion
}
