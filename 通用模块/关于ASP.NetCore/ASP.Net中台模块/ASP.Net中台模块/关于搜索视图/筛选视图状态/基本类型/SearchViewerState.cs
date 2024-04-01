using System.Collections.Immutable;
using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是筛选视图的状态，
/// 通过它可以调整筛选条件，
/// 并生成最终的筛选结果
/// </summary>
public sealed class SearchViewerState
{
    #region 公开成员
    #region 生成筛选条件
    /// <summary>
    /// 生成筛选条件
    /// </summary>
    /// <returns></returns>
    public DataFilterDescription GenerateFilter()
    {
        var (sortNull, sortNotNull) = SortCondition.
            Select(x => (x.GenerateFilter(), x)).
            Split(x => x.Item1 is null);
        SortCondition = SortCondition.RemoveRange(sortNull.Select(x => x.x));
        return new()
        {
            QueryCondition = QueryCondition.Select(x => x.GenerateFilter()).
            Where(x => x is { }).Cast<QueryCondition>().ToArray(),
            SortCondition = sortNotNull.Select(x => x.Item1).Cast<SortCondition>().ToArray(),
        };
    }
    #endregion
    #region 绑定筛选条件
    #region 绑定查询条件
    /// <summary>
    /// 返回一个查询条件绑定，
    /// 将值绑定到它的<see cref="BindQueryCondition{Property}.Value"/>就可以绑定为查询条件
    /// </summary>
    /// <typeparam name="Property">属性的值的类型</typeparam>
    /// <param name="render">描述如何渲染查询条件的对象</param>
    /// <returns></returns>
    public BindQueryCondition<Property> Bind<Property>(RenderQueryCondition render)
    {
        var bind = new BindQueryCondition<Property>(render);
        if (QueryCondition.FirstOrDefault(x => Equals(x, bind)) is BindQueryCondition<Property> existBind)
            return existBind;
        QueryCondition = QueryCondition.Add(bind);
        return bind;
    }
    #endregion
    #region 绑定排序条件
    /// <summary>
    /// 返回一个排序条件绑定，
    /// 将值绑定到它的<see cref="BindSortCondition.SortMode"/>就可以绑定为排序条件
    /// </summary>
    /// <param name="render">描述如何渲染排序条件的对象</param>
    /// <returns></returns>
    public BindSortCondition Bind(RenderSortCondition render)
    {
        var bind = new BindSortCondition(render);
        if (SortCondition.FirstOrDefault(x => Equals(x, bind)) is BindSortCondition existBind)
            return existBind;
        SortCondition = SortCondition.Add(bind);
        return bind;
    }
    #endregion
    #endregion
    #region 清空筛选条件
    /// <summary>
    /// 清除所有查询或排序条件
    /// </summary>
    /// <param name="clearQuery">如果这个值为<see langword="true"/>，则清除查询条件</param>
    /// <param name="clearSort">如果这个值为<see langword="true"/>，则清除排序条件</param>
    public void Clear(bool clearQuery = true, bool clearSort = true)
    {
        if (clearQuery)
            QueryCondition = [];
        if (clearSort)
            SortCondition = [];
    }
    #endregion
    #endregion
    #region 内部成员
    #region 所有查询条件
    /// <summary>
    /// 获取所有查询条件
    /// </summary>
    private ImmutableList<IGenerateFilter> QueryCondition { get; set; } = [];
    #endregion
    #region 所有排序条件
    /// <summary>
    /// 获取所有排序条件
    /// </summary>
    private ImmutableList<IGenerateFilter> SortCondition { get; set; } = [];
    #endregion
    #endregion
}
