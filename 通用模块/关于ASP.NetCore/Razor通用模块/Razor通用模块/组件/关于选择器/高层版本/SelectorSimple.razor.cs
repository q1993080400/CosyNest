using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是<see cref="Selector{Candidate}"/>的开箱即用版，
/// 它可以作为一个选择器
/// </summary>
/// <typeparam name="BusinessInterface">业务接口的类型，它用来获取候选项</typeparam>
/// <inheritdoc cref="Selector{Candidate}"/>
public sealed partial class SelectorSimple<Candidate, BusinessInterface> : ComponentBase
    where BusinessInterface : class, IGetCandidates<Candidate>
    where Candidate : IWithID
{
    #region 组件参数
    #region 已选择元素的ID
    /// <summary>
    /// 获取已选择元素的ID，
    /// 它会被用来初始化元素的选择状态
    /// </summary>
    [Parameter]
    public IReadOnlySet<Guid> SelectedElementID { get; set; } = new HashSet<Guid>();
    #endregion
    #region 提交选择元素的方法
    /// <inheritdoc cref="Selector{Candidate}.Submit"/>
    [Parameter]
    [EditorRequired]
    public Func<SelectElementInfo<Candidate>, Task> Submit { get; set; }
    #endregion
    #region 用来渲染组件的委托
    /// <inheritdoc cref="Selector{Candidate}.RenderComponent"/>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderSelectInfo<Candidate>> RenderComponent { get; set; }
    #endregion
    #region 用来筛选候选元素的委托
    /// <summary>
    /// 用来筛选候选元素的委托，
    /// 它传入待筛选的元素，返回是否保留这个元素，
    /// 通过它可以实现搜索等功能
    /// </summary>
    [Parameter]
    public Func<Candidate, bool>? FilterCandidate { get; set; }
    #endregion
    #region 最大可选数量
    /// <inheritdoc cref="Selector{Candidate}.MaxSelectCount"/>
    [Parameter]
    public int? MaxSelectCount { get; set; }
    #endregion
    #region 最小可选数量
    /// <inheritdoc cref="Selector{Candidate}.MinSelectCount"/>
    [Parameter]
    public int? MinSelectCount { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取候选元素
    /// <summary>
    /// 获取所有候选元素
    /// </summary>
    private IReadOnlySet<Candidate>? Candidates { get; set; }
    #endregion
    #region 初始化选择状态
    /// <summary>
    /// 初始化元素的选择状态，
    /// 并返回它是否被选择
    /// </summary>
    /// <param name="candidate">要判断的元素</param>
    /// <returns></returns>
    private bool InitializationSelect(Candidate candidate)
        => SelectedElementID.Contains(candidate.ID);
    #endregion
    #region 重写OnInitializedAsync方法
    protected override async Task OnInitializedAsync()
    {
        var candidates = await StrongTypeInvokeFactory.StrongType<BusinessInterface>().
            Invoke(x => x.AllCandidates());
        Candidates = candidates.ToImmutableHashSet();
    }
    #endregion
    #endregion
}
