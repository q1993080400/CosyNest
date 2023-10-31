namespace System.DataFrancis;

/// <summary>
/// 这个记录描述排序实体的条件
/// </summary>
/// <typeparam name="Obj">排序实体的类型</typeparam>
public sealed record SortCondition<Obj> : DataCondition<Obj>
{
    #region 是否升序
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示排序为升序，否则为降序
    /// </summary>
    public required bool IsAscending { get; init; }
    #endregion
    #region 排序优先级
    /// <summary>
    /// 获取排序的优先级，
    /// 这个数字越低，代表优先级越高
    /// </summary>
    public required int Priority { get; init; }
    #endregion
}
