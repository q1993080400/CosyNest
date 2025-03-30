using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是底层使用Bootstrap实现的弹窗选择器
/// </summary>
/// <typeparam name="Candidate">候选元素的类型</typeparam>
public sealed partial class BootstrapPopUpSelect<Candidate> : ComponentBase
{
    #region 组件参数
    #region 弹窗标题
    /// <summary>
    /// 获取弹窗的标题
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Title { get; set; }
    #endregion
    #region 获取候选元素
    /// <inheritdoc cref="Selector{Candidate}.Candidates"/>
    [Parameter]
    [EditorRequired]
    public IEnumerable<Candidate> Candidates { get; set; }
    #endregion
    #region 提交选择元素的方法
    /// <inheritdoc cref="Selector{Candidate}.Submit"/>
    [Parameter]
    [EditorRequired]
    public Func<SelectElementInfo<Candidate>, Task> Submit { get; set; }
    #endregion
    #region 取消弹窗的方法
    /// <summary>
    /// 取消弹窗的方法
    /// </summary>
    [Parameter]
    [EditorRequired]
    public EventCallback Cancellation { get; set; }
    #endregion
    #region 将元素转换成string的方法
    /// <summary>
    /// 将元素转换成文本形式的方法，
    /// 它会被作为显示的文本，并且作为识别元素的键
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Candidate, string> ConvertToString { get; set; }
    #endregion
    #region 用于分组的委托
    /// <summary>
    /// 获取用于分组的委托，
    /// 它的参数是元素，返回值是分组的名称
    /// </summary>
    [Parameter]
    public Func<Candidate, string?> Group { get; set; } = static _ => null;
    #endregion
    #region 初始选择委托
    /// <inheritdoc cref="Selector{Candidate}.InitializationSelect"/>
    [Parameter]
    public Func<Candidate, bool> InitializationSelect { get; set; } = static x => false;
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
    #region 搜索条件
    /// <summary>
    /// 获取搜索条件
    /// </summary>
    private string? SearchCriteria { get; set; }
    #endregion
    #region 要渲染的集合
    /// <summary>
    /// 获取真正要渲染的集合
    /// </summary>
    /// <returns></returns>
    private HashSet<Candidate> RenderCandidates()
    {
        var list = SearchCriteria.IsVoid() ?
            Candidates :
            Candidates.Where(x => ConvertToString(x).Contains(SearchCriteria));
        return [.. list];
    }
    #endregion
    #endregion
}
