using System.DataFrancis;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录可以将组件的值直接绑定到查询条件中
/// </summary>
/// <typeparam name="Property">属性的值的类型</typeparam>
/// <typeparam name="Obj">实体类的值的类型</typeparam>
public sealed record QueryBind<Property, Obj>
{
    #region 公开成员
    #region 要绑定的属性
    /// <summary>
    /// 组件可以直接绑定这个属性，
    /// 对它的改动也会影响到最终生成的查询条件
    /// </summary>
    public Property? Bind
    {
        get
        {
            var key = RenderSearchViewerInfo<int>.GenerateQueryKey(PropertyAccess, LogicalOperator);
            return Info.QueryCondition.TryGetValue(key).Value is { CompareValue: Property v } ? v : default;
        }
        set => Info.AdjustQuery(PropertyAccess, LogicalOperator, value);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 渲染参数
    /// <summary>
    /// 获取这个对象所依附的渲染参数，
    /// 本对象就是为它服务的
    /// </summary>
    private RenderSearchViewerInfo<Obj> Info { get; }
    #endregion
    #region 属性访问表达式
    /// <summary>
    /// 获取属性访问表达式，
    /// 它决定了查询条件应该查询哪个属性
    /// </summary>
    private string PropertyAccess { get; }
    #endregion
    #region 逻辑运算符
    /// <summary>
    /// 获取查询的逻辑运算符
    /// </summary>
    private LogicalOperator LogicalOperator { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="info">这个对象所依附的渲染参数，本对象就是为它服务的</param>
    /// <param name="propertyAccess">属性访问表达式，它决定了查询条件应该查询哪个属性</param>
    /// <param name="logicalOperator">查询的逻辑运算符</param>
    internal QueryBind(RenderSearchViewerInfo<Obj> info, string propertyAccess, LogicalOperator logicalOperator)
    {
        Info = info;
        PropertyAccess = propertyAccess;
        LogicalOperator = logicalOperator;
    }
    #endregion
}
