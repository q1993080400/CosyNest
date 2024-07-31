using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是用来渲染<see cref="BootstrapFormViewer{Model}"/>属性的默认方法
/// </summary>
/// <inheritdoc cref="BootstrapFormViewer{Model}"/>
public sealed partial class BootstrapFormViewerProperty<Model> : ComponentBase
    where Model : class
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取渲染这个属性的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFormViewerPropertyInfo<Model> RenderInfo { get; set; }
    #endregion
    #region 当数据改变时触发的委托
    /// <summary>
    /// 当数据改变时触发的委托，
    /// 它的参数就是数据的新值
    /// </summary>
    [Parameter]
    public Func<object?, Task>? OnPropertyChange { get; set; }
    #endregion
    #region 高亮文本
    /// <summary>
    /// 获取高亮文本的集合
    /// </summary>
    [CascadingParameter(Name = SearchPanel.HighlightParameter)]
    private IReadOnlyCollection<string>? Highlight { get; set; }
    #endregion
    #endregion
}
