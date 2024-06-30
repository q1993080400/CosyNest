using System.Text.Json.Serialization;

namespace System.DataFrancis;

/// <summary>
/// 这个类型是描述实体数据条件的基类
/// </summary>
[JsonDerivedType(typeof(QueryCondition), nameof(QueryCondition))]
[JsonDerivedType(typeof(SortCondition), nameof(SortCondition))]
public abstract record DataCondition : IHasFilterTarget
{
    #region 筛选标识
    /// <summary>
    /// 如果<see cref="IsVirtually"/>为<see langword="true"/>，
    /// 它表示一个可以区分筛选条件的标识，
    /// 否则它表示一个属性访问表达式，指示如何访问要筛选的属性，
    /// 支持递归，使用.分隔
    /// </summary>
    public required string Identification { get; init; }
    #endregion
    #region 是否为虚拟筛选
    public required bool IsVirtually { get; init; }
    #endregion
}
