namespace System.DataFrancis;

/// <summary>
/// 这个记录描述排序实体的条件，
/// 如果这个对象在一个集合中，
/// 那么排序优先级用位置来表示，排在前面的条件优先级高
/// </summary>
public sealed record SortCondition : DataCondition
{
    #region 排序状态
    /// <summary>
    /// 获取排序的状态
    /// </summary>
    public required SortStatus SortStatus { get; init; }
    #endregion
}
