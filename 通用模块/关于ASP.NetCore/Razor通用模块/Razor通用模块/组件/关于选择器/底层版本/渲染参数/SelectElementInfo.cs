namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录可以用来描述<see cref="Selector{Candidate}"/>组件被选中的元素的情况
/// </summary>
/// <inheritdoc cref="Selector{Candidate}"/>
public sealed record SelectElementInfo<Candidate>
{
    #region 已被选择的候选项
    /// <summary>
    /// 获取已被选择的候选项
    /// </summary>
    public IReadOnlySet<Candidate> Select { get; init; }
    #endregion
    #region 最大可选数量
    /// <summary>
    /// 获取最大可选的元素数量，
    /// 选择的元素数量如果超过，会被视为无效
    /// </summary>
    public required int MaxSelectCount { get; init; }
    #endregion
    #region 最小可选数量
    /// <summary>
    /// 获取最小可选的元素数量，
    /// 选择的元素数量如果低于它，会被视为无效
    /// </summary>
    public required int MinSelectCount { get; init; }
    #endregion
    #region 是否无效
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示选择的元素数量不符合要求，
    /// 它大于最大值，或小于最小值
    /// </summary>
    public bool IsInvalid
    {
        get
        {
            var selectCount = Select.Count;
            return selectCount < MinSelectCount || selectCount > MaxSelectCount;
        }
    }
    #endregion
}
