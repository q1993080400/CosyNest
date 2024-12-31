using System.DataFrancis;

using Microsoft.AspNetCore;

namespace System;

/// <summary>
/// 有关ASP.Net前后端通用的扩展方法全部放在这个类型中
/// </summary>
public static class ExtendASP
{
    #region 获取逻辑运算符
    /// <summary>
    /// 将渲染用逻辑运算符转换为逻辑运算符
    /// </summary>
    /// <param name="renderLogicalOperator">待转换的渲染用逻辑运算符</param>
    /// <param name="filterObjectType">筛选对象的类型，
    /// 根据它的不同，可用的逻辑运算符也不同</param>
    /// <returns></returns>
    public static LogicalOperator ToLogicalOperator(this RenderLogicalOperator renderLogicalOperator, FilterObjectType filterObjectType)
        => (renderLogicalOperator, filterObjectType) switch
        {
            (RenderLogicalOperator.None, FilterObjectType.Text) => LogicalOperator.Contain,
            (RenderLogicalOperator.None, _) => LogicalOperator.Equal,
            (RenderLogicalOperator.Equal, _) => LogicalOperator.Equal,
            (RenderLogicalOperator.NotEqual, _) => LogicalOperator.NotEqual,
            (RenderLogicalOperator.Contain, _) =>
            filterObjectType is FilterObjectType.Text ?
            LogicalOperator.Contain :
            throw new NotSupportedException($"{filterObjectType}不能使用{RenderLogicalOperator.Contain}逻辑运算符"),
            (_, FilterObjectType.Enum or FilterObjectType.Num or FilterObjectType.Date) => renderLogicalOperator switch
            {
                RenderLogicalOperator.GreaterThan => LogicalOperator.GreaterThan,
                RenderLogicalOperator.GreaterThanOrEqual => LogicalOperator.GreaterThanOrEqual,
                RenderLogicalOperator.LessThan => LogicalOperator.LessThan,
                RenderLogicalOperator.LessThanOrEqual => LogicalOperator.LessThanOrEqual,
                _ => throw new NotSupportedException($"不能将运算符{renderLogicalOperator}用于类型{filterObjectType}")
            },
            _ => throw new NotSupportedException($"无法将{renderLogicalOperator}和{filterObjectType}的组合正确地转换为逻辑运算符")
        };
    #endregion
}
