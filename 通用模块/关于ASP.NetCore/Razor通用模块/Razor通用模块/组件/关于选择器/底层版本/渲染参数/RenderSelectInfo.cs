namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="Selector{Candidate}"/>组件的参数
/// </summary>
/// <inheritdoc cref="Selector{Candidate}"/>
public sealed record RenderSelectInfo<Candidate>
{
    #region 用来渲染所有候选项的参数
    /// <summary>
    /// 获取用来渲染所有候选项的参数
    /// </summary>
    public required IReadOnlyCollection<RenderSelectElementInfo<Candidate>> CandidatesInfo { get; init; }
    #endregion
    #region 提交选择元素的方法
    /// <summary>
    /// 提交待选择元素的方法
    /// </summary>
    public required Func<Task> Submit { get; init; }
    #endregion
    #region 重置被选择的元素的方法
    /// <summary>
    /// 这个委托可以重置所有元素的被选择状态，
    /// 并将其恢复到初始状态
    /// </summary>
    public required Func<Task> Reset { get; init; }
    #endregion
    #region 被选中的元素的情况
    /// <summary>
    /// 这个对象描述组件被选中的元素的情况
    /// </summary>
    public required SelectElementInfo<Candidate> SelectElementInfo { get; init; }
    #endregion
}
