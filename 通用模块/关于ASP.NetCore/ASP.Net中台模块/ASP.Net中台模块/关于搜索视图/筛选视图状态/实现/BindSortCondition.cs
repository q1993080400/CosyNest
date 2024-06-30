using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以将值绑定到排序条件
/// </summary>
/// <inheritdoc cref="BindFilterCondition{Target, Action, Property}.BindFilterCondition(RenderFilter{Target, Action})"/>
sealed class BindSortCondition(RenderFilter<FilterTargetSingle, FilterActionSort> renderFilter) :
   BindFilterCondition<FilterTargetSingle, FilterActionSort, SortStatus>(renderFilter), IBindProperty<SortStatus>
{
    #region 排序状态
    /// <summary>
    /// 获取排序状态
    /// </summary>
    public SortStatus Value { get; set; }
    #endregion
    #region 生成排序条件
    public override DataCondition[] GenerateFilter()
        => (Value, RenderFilter.FilterTarget) switch
        {
            (SortStatus.None, _) => [],
            (_, { } filterTargetSingle) => [new SortCondition()
            {
                Identification = filterTargetSingle.PropertyAccess.Name,
                SortStatus = Value,
                IsVirtually=filterTargetSingle.IsVirtually
            }],
            _ => throw new NotSupportedException("无法生成排序条件")
        };
    #endregion
}
