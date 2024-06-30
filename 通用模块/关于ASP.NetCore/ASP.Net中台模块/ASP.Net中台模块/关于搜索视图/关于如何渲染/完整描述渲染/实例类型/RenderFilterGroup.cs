using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录表示成组的渲染筛选条件
/// </summary>
public sealed record RenderFilterGroup : IHasFilterIdentification
{
    #region 排序
    /// <summary>
    /// 获取渲染顺序，它以升序排列
    /// </summary>
    public required int Order { get; init; }
    #endregion
    #region 描述
    /// <summary>
    /// 对要筛选的对象的描述文本
    /// </summary>
    public required string Describe { get; init; }
    #endregion
    #region 查询条件
    /// <summary>
    /// 获取要渲染的查询条件
    /// </summary>
    public required RenderFilter<FilterTarget, FilterActionQuery> RenderFilterQuery { get; init; }
    #endregion
    #region 标识
    public string Identification
    {
        get
        {
            var queryIdentification = RenderFilterQuery.FilterTarget.Identification;
            if (RenderFilterSort is null)
                return queryIdentification;
            var sortIdentification = RenderFilterSort.FilterTarget.Identification;
            return queryIdentification == sortIdentification ?
                queryIdentification :
                throw new NotSupportedException("要查询的属性的标识，和要排序的属性的标识不相等");
        }
    }
    #endregion
    #region 类型
    /// <summary>
    /// 描述要筛选的对象的类型
    /// </summary>
    public FilterObjectType FilterObjectType
    {
        get
        {
            var queryObjectType = RenderFilterQuery.FilterTarget.FilterObjectType;
            if (RenderFilterSort is null)
                return queryObjectType;
            var sortObjectType = RenderFilterSort.FilterTarget.FilterObjectType;
            return queryObjectType == sortObjectType ?
                queryObjectType :
                throw new NotSupportedException("要查询的属性的类型，和要排序的属性的类型不相等");
        }
    }
    #endregion
    #region 排序条件
    /// <summary>
    /// 获取要渲染的排序条件，
    /// 如果没有排序条件，则为<see langword="null"/>
    /// </summary>
    public required RenderFilter<FilterTargetSingle, FilterActionSort>? RenderFilterSort { get; init; }
    #endregion
    #region 是否存在自定义默认值
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示存在自定义默认值，需要进行处理
    /// </summary>
    public bool HasDefaultValue
        => RenderFilterQuery.FilterTarget.HasDefaultValue ||
        RenderFilterSort is { FilterTarget.HasDefaultValue: true };
    #endregion
}
