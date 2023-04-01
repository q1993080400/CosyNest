namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录可以将组件的值直接绑定到排序条件中
/// </summary>
/// <inheritdoc cref="QueryBind{Property, Obj}"/>
public sealed record SortBind<Property, Obj>
{
    #region 公开成员
    #region 要绑定的属性
    /// <summary>
    /// 组件可以直接绑定这个属性，
    /// 对它的改动也会影响到最终生成的排序条件
    /// </summary>
    public Property? Bind
    {
        get
        {
            var isAscending = Info.SortCondition.TryGetValue(PropertyAccess).Value?.IsAscending;
            return FromSortCondition(isAscending);
        }
        set => Info.AdjustSort(PropertyAccess, ToSortCondition(value));
    }
    #endregion
    #endregion
    #region 内部成员
    #region 渲染参数
    /// <summary>
    /// 获取这个对象所依附的渲染参数，
    /// 本对象就是为它服务的
    /// </summary>
    private SearchViewerInfo<Obj> Info { get; }
    #endregion
    #region 属性访问表达式
    /// <summary>
    /// 获取属性访问表达式，
    /// 它决定了查询条件应该查询哪个属性
    /// </summary>
    private string PropertyAccess { get; }
    #endregion
    #region 从属性转换为排序模式
    /// <summary>
    /// 这个委托的输入是绑定属性的值，
    /// 返回值是将它转换为对排序模式的描述，
    /// 为<see langword="true"/>，表示升序，
    /// 为<see langword="false"/>，表示降序，为<see langword="null"/>，表示删除这个排序条件
    /// </summary>
    private Func<Property?, bool?> ToSortCondition { get; }
    #endregion
    #region 将排序模式转换为属性
    /// <summary>
    /// 这个委托的输入是排序模式，
    /// 为<see langword="true"/>，表示升序，
    /// 为<see langword="false"/>，表示降序，为<see langword="null"/>，表示删除这个排序条件，
    /// 返回值是将它转换为的绑定属性的值
    /// </summary>
    private Func<bool?, Property?> FromSortCondition { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="info">这个对象所依附的渲染参数，本对象就是为它服务的</param>
    /// <param name="propertyAccess">属性访问表达式，它决定了查询条件应该查询哪个属性</param>
    /// <param name="toSortCondition">这个委托的输入是绑定属性的值，
    /// 返回值是将它转换为对排序模式的描述，
    /// 为<see langword="true"/>，表示升序，
    /// 为<see langword="false"/>，表示降序，为<see langword="null"/>，表示删除这个排序条件</param>
    /// <param name="fromSortCondition">这个委托的输入是排序模式，
    /// 为<see langword="true"/>，表示升序，
    /// 为<see langword="false"/>，表示降序，为<see langword="null"/>，表示删除这个排序条件，
    /// 返回值是将它转换为的绑定属性的值</param>
    internal SortBind(SearchViewerInfo<Obj> info, string propertyAccess, Func<Property?, bool?> toSortCondition, Func<bool?, Property?> fromSortCondition)
    {
        Info = info;
        PropertyAccess = propertyAccess;
        ToSortCondition = toSortCondition;
        FromSortCondition = fromSortCondition;
    }
    #endregion
}
