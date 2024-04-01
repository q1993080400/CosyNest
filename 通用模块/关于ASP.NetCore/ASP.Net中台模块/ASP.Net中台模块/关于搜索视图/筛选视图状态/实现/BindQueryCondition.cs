using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以将值绑定到查询条件
/// </summary>
/// <typeparam name="Property">属性值的类型</typeparam>
/// <param name="render">用来渲染单个查询条件的对象</param>
public sealed class BindQueryCondition<Property>(RenderQueryCondition render) : IGenerateFilter
{
    #region 公开成员
    #region 属性的值
    /// <summary>
    /// 获取属性的值，它会被绑定到查询条件，
    /// 当它为<see langword="null"/>时，
    /// 表示移除这个查询条件
    /// </summary>
    public Property? Value { get; set; } = render.DefaultValue.To<Property>(false);
    #endregion
    #region 生成查询条件
    public DataCondition? GenerateFilter()
        => (Value, Render.ExcludeFilter) switch
        {
            (null, _) => null,
            ({ } value, var exclude)
            when exclude is null || !value.ToString().EqualsIgnore(exclude.ToString())
            => new QueryCondition()
            {
                CompareValue = Value is string text ? text.Trim() : Value,
                LogicalOperator = Render.LogicalOperator,
                PropertyAccess = Render.PropertyAccess,
                IsVirtually = Render.IsVirtually
            },
            _ => null
        };
    #endregion
    #endregion
    #region 重写的成员
    #region 重写GetHashCode
    public override int GetHashCode()
        => ToolEqual.CreateHash(Render.PropertyAccess, Render.LogicalOperator);
    #endregion
    #region 重写Equals
    public override bool Equals(object? obj)
        => obj is BindQueryCondition<Property>
        {
            Render:
            {
                PropertyAccess: { } access,
                LogicalOperator: { } logical
            }
        } &&
        access == Render.PropertyAccess &&
        logical == Render.LogicalOperator;
    #endregion
    #endregion
    #region 内部成员
    /// <summary>
    /// 用来渲染单个查询条件的对象
    /// </summary>
    private RenderQueryCondition Render { get; } = render;
    #endregion
}
