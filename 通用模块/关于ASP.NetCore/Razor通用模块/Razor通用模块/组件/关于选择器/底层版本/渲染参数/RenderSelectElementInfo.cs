namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="Selector{Candidate}"/>每个候选项的委托
/// </summary>
/// <inheritdoc cref="Selector{Candidate}"/>
public sealed record RenderSelectElementInfo<Candidate>
{
    #region 要渲染的候选项
    /// <summary>
    /// 获取要渲染的候选项
    /// </summary>
    public required Candidate Element { get; init; }
    #endregion
    #region 是否已被选择
    /// <summary>
    /// 获取这个元素是否已被选择
    /// </summary>
    public bool IsSelect
        => SelectElementInfo.Select.Contains(Element);
    #endregion
    #region 变更选择的委托
    /// <summary>
    /// 调用这个委托可以反转这个候选项的被选择状态，
    /// 注意：它不检查选择的元素数量是否超过限制，如果你需要这个功能，
    /// 可以与<see cref="SelectElementInfo"/>属性配合完成这个操作
    /// </summary>
    public required Action ChangeSelect { get; init; }
    #endregion
    #region 被选中的元素的情况
    /// <summary>
    /// 这个对象描述组件被选中的元素的情况
    /// </summary>
    public required SelectElementInfo<Candidate> SelectElementInfo { get; init; }
    #endregion
}
