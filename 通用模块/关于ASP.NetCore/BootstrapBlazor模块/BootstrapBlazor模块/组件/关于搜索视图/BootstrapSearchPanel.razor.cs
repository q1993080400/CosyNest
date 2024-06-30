using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是<see cref="SearchPanel"/>的开箱即用版，
/// 它底层由Bootstrap实现
/// </summary>
/// <typeparam name="BusinessInterface">业务接口的类型，它决定了如何获取渲染条件</typeparam>
public sealed partial class BootstrapSearchPanel<BusinessInterface> : ComponentBase
    where BusinessInterface : class, IGetRenderAllFilterCondition
{
    #region 组件参数
    #region 有关渲染
    #region 渲染整个组件的委托
    /// <inheritdoc cref="SearchPanel.RenderComponent"/>
    [Parameter]
    public RenderFragment<RenderSearchPanelInfo>? RenderComponent { get; set; }
    #endregion
    #region 获取如何渲染筛选条件
    /// <inheritdoc cref="SearchPanel.GetRenderCondition"/>
    [Parameter]
    public Func<Task<RenderFilterGroup[]>>? GetRenderCondition { get; set; }
    #endregion
    #region 搜索视图状态
    /// <inheritdoc cref="SearchPanel.SearchViewerState"/>
    [Parameter]
    public SearchViewerState SearchViewerState { get; set; } = new();
    #endregion
    #region 渲染单个属性的委托
    /// <inheritdoc cref="SearchPanel.RenderProperty"/>
    [Parameter]
    public RenderFragment<RenderSearchPanelPropertyInfo>? RenderProperty { get; set; }
    #endregion
    #region 渲染提交区域的委托
    /// <inheritdoc cref="SearchPanel.RenderSubmit"/>
    [Parameter]
    public RenderFragment<RenderSearchPanelSubmitInfo> RenderSubmit { get; set; }
    #endregion
    #endregion
    #region 有关业务
    #region 元素编号对象
    /// <inheritdoc cref="SearchPanel.ElementNumber"/>
    [Parameter]
    [EditorRequired]
    public IElementNumber ElementNumber { get; set; }
    #endregion
    #region 提交搜索
    /// <inheritdoc cref="SearchPanel.Submit"/>
    [Parameter]
    [EditorRequired]
    public Func<SearchPanelSubmitInfo, Task> Submit { get; set; }
    #endregion
    #endregion
    #endregion
}
