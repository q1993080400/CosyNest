namespace System.DataFrancis;

/// <summary>
/// 这个记录表示查询和排序数据的条件，
/// 它可以被转换为Json，然后在前端和后端之间进行传递
/// </summary>
public sealed record DataFilterDescription
{
    #region 查询条件
    /// <summary>
    /// 描述查询实体的条件，
    /// 这些条件之间使用与逻辑
    /// </summary>
    public IReadOnlyCollection<QueryCondition> QueryCondition { get; init; } = [];
    #endregion
    #region 排序条件
    /// <summary>
    /// 描述排序实体的条件，
    /// 这些条件按照优先级从高到低排列
    /// </summary>
    public IReadOnlyCollection<SortCondition> SortCondition { get; init; } = [];
    #endregion
    #region 重构对象
    /// <summary>
    /// 重构这个对象的查询和排序条件，
    /// 并返回一个新的对象
    /// </summary>
    /// <param name="reconfigurationQuery">传入旧条件，返回新条件的委托</param>
    /// <returns></returns>
    public DataFilterDescription Reconfiguration(Func<DataCondition, DataCondition> reconfigurationQuery)
        => this with
        {
            QueryCondition = QueryCondition.Select(reconfigurationQuery).Cast<QueryCondition>().ToArray(),
            SortCondition = SortCondition.Select(reconfigurationQuery).Cast<SortCondition>().ToArray()
        };
    #endregion
    #region 高亮文本
    /// <summary>
    /// 获取应该高亮的文本
    /// </summary>
    public IReadOnlySet<string> Highlight
        => QueryCondition.
        Select(static x => x.CompareValue).
        OfType<string>().ToHashSet();
    #endregion
}
