﻿namespace System.DataFrancis;

/// <summary>
/// 这个特性表示数据应该被渲染，
/// 并指定了渲染数据的方式
/// </summary>
public sealed class RenderDataAttribute : RenderDataBaseAttribute
{
    #region 显示名称
    /// <summary>
    /// 要在UI上显示的名称
    /// </summary>
    public string Name { get; init; } = "";
    #endregion
    #region 说明
    /// <summary>
    /// 获取对这个字段的说明，
    /// 它被放在这个字段的下方
    /// </summary>
    public string? Describe { get; init; }
    #endregion
    #region 是否在只读状态的时候也显示说明
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示在只读状态的时候，也应该显示说明
    /// </summary>
    public bool ShowDescribeInReadOnly { get; init; }
    #endregion
    #region 是否递归渲染
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则指示这个属性的类型是一个复杂的对象，
    /// 应该进行递归渲染，它把这个属性视为一个新的表单
    /// </summary>
    public bool IsRecursion { get; init; }
    #endregion
    #region 值为null的时候显示的文本
    /// <summary>
    /// 获取值为<see langword="null"/>的时候显示的文本，
    /// 只在只读状态下生效
    /// </summary>
    public string? ValueIfNullText { get; init; }
    #endregion
}
