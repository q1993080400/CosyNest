using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个选择器
/// </summary>
/// <typeparam name="Candidate">候选元素的类型</typeparam>
public sealed partial class Selector<Candidate> : ComponentBase
{
    #region 组件参数
    #region 获取候选元素
    /// <summary>
    /// 获取所有候选元素
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlySet<Candidate> Candidates { get; set; }
    #endregion
    #region 提交选择元素的方法
    /// <summary>
    /// 提交待选择元素的方法
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<SelectElementInfo<Candidate>, Task> Submit { get; set; }
    #endregion
    #region 用来渲染组件的委托
    /// <summary>
    /// 获取用来渲染组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderSelectInfo<Candidate>> RenderComponent { get; set; }
    #endregion
    #region 初始选择委托
    /// <summary>
    /// 这个委托返回元素的初始选择状态，
    /// 它传入元素，返回元素是否被选择
    /// </summary>
    [Parameter]
    public Func<Candidate, bool> InitializationSelect { get; set; } = static x => false;
    #endregion
    #region 最大可选数量
    /// <summary>
    /// 获取最大可选的元素数量，
    /// 选择的元素数量如果超过，会被视为无效，
    /// 默认为1
    /// </summary>
    [Parameter]
    public int? MaxSelectCount { get; set; }
    #endregion
    #region 最小可选数量
    /// <summary>
    /// 获取最小可选的元素数量，
    /// 选择的元素数量如果低于它，会被视为无效，
    /// 默认为1
    /// </summary>
    [Parameter]
    public int? MinSelectCount { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 被选择的元素
    /// <summary>
    /// 获取所有被选择的元素
    /// </summary>
    private ImmutableHashSet<Candidate> SelectElement { get; set; } = [];
    #endregion
    #region 重写OnInitialized方法
    protected override void OnInitialized()
    {
        InitializedSelectElement();
    }
    #endregion
    #region 初始化候选项
    /// <summary>
    /// 重置所有候选项的被选择状态，
    /// 并将其恢复到初始状态
    /// </summary>
    private void InitializedSelectElement()
    {
        SelectElement = Candidates.Where(InitializationSelect).ToImmutableHashSet();
    }
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderSelectInfo<Candidate> GetRenderInfo()
    {
        var selectElementInfo = new SelectElementInfo<Candidate>()
        {
            MaxSelectCount = MaxSelectCount ?? 1,
            MinSelectCount = MinSelectCount ?? 1,
            Select = SelectElement
        };
        var renderSelectElementInfo = Candidates.Select(element =>
        {
            #region 用于反选候选状态的本地函数
            void ChangeSelect()
            {
                if (SelectElement.Contains(element))
                {
                    SelectElement = SelectElement.Remove(element);
                    return;
                }
                SelectElement = (MaxSelectCount is 1 ? SelectElement : []).Add(element);
                this.StateHasChanged();
            }
            #endregion
            return new RenderSelectElementInfo<Candidate>()
            {
                ChangeSelect = ChangeSelect,
                Element = element,
                SelectElementInfo = selectElementInfo
            };
        }).ToArray();
        return new()
        {
            CandidatesInfo = renderSelectElementInfo,
            SelectElementInfo = selectElementInfo,
            Submit = () => Submit(selectElementInfo),
            Reset = () =>
            {
                InitializedSelectElement();
                this.StateHasChanged();
                return Task.CompletedTask;
            }
        };
    }
    #endregion
    #endregion
}
