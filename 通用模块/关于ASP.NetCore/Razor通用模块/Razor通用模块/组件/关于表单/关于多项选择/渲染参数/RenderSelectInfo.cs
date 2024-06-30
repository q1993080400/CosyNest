namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="Selector{Candidate}"/>组件的参数
/// </summary>
/// <inheritdoc cref="Selector{Candidate}"/>
public sealed record RenderSelectInfo<Candidate>
{
    #region 获取待渲染的候选元素
    /// <summary>
    /// 获取待渲染的候选元素
    /// </summary>
    public required IEnumerable<Candidate> Candidates { get; init; }
    #endregion
    #region 选择或取消选择元素的方法
    /// <summary>
    /// 这个委托的参数是一个元素，
    /// 如果该元素已被选择，则取消它，
    /// 如果未被选择，则选择它，
    /// 返回值如果为<see langword="true"/>，
    /// 表示选择它，否则表示取消它
    /// </summary>
    public required Func<Candidate, bool> SelectOrCancel { get; init; }
    #endregion
    #region 获取是否至少选择了一个元素
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示至少选择了一个元素
    /// </summary>
    public required bool AnySelect { get; init; }
    #endregion
    #region 判断元素是否被选中的方法
    /// <summary>
    /// 这个委托传入元素，
    /// 并判断该元素是否被选中
    /// </summary>
    public required Func<Candidate, bool> IsSelect { get; init; }
    #endregion
    #region 提交选择元素的方法
    /// <summary>
    /// 提交待选择元素的方法
    /// </summary>
    public required Func<Task> Submit { get; set; }
    #endregion
}
