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
    /// 提交待选择元素的方法，
    /// 方法的参数就是所有被选择的元素
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<IReadOnlyCollection<Candidate>, Task> Submit { get; set; }
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
    #region 最大选择数量
    /// <summary>
    /// 获取所允许的最大选择数量，
    /// 默认为1
    /// </summary>
    [Parameter]
    public int MaxSelectCount { get; set; } = 1;
    #endregion
    #endregion
    #region 内部成员
    #region 被选择的元素
    /// <summary>
    /// 获取所有被选择的元素
    /// </summary>
    private HashSet<Candidate> SelectElement { get; set; } = [];
    #endregion
    #region 重写SetParametersAsync方法
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.TryGetValue<Func<Candidate, bool>>(nameof(InitializationSelect), out var initializationSelect))
        {
            var candidates = parameters.GetValueOrDefault<IReadOnlySet<Candidate>>(nameof(Candidates))!;
            SelectElement = candidates.Where(initializationSelect).ToHashSet();
        }
        await base.SetParametersAsync(parameters);
    }
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderSelectInfo<Candidate> GetRenderInfo()
        => new()
        {
            Candidates = Candidates,
            Submit = async () =>
            {
                await this.Submit(SelectElement);
            },
            SelectOrCancel = x =>
            {
                if (!Candidates.Contains(x))
                    return false;
                if (MaxSelectCount is 1)
                    SelectElement.Clear();
                var isSelect = !SelectElement.Remove(x);
                if (isSelect)
                    SelectElement.Add(x);
                this.StateHasChanged();
                return isSelect;
            },
            IsSelect = SelectElement.Contains,
            AnySelect = SelectElement.Count > 0
        };
    #endregion
    #endregion
}
