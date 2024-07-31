﻿namespace System.DataFrancis;

/// <summary>
/// 这个特性可以用来描述如何渲染数据
/// </summary>
public sealed class RenderDataAttribute : RenderDataBaseAttribute
{
    #region 显示名称
    /// <summary>
    /// 要在UI上显示的名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
    #region 格式化字符串
    /// <summary>
    /// 这个属性指示格式化字符串，
    /// 只对部分数据类型有效
    /// </summary>
    public string? Format { get; init; }
    #endregion
    #region 渲染长文本时的行数
    /// <summary>
    /// 指示在渲染长文本时，
    /// 最多应该渲染的行数
    /// </summary>
    public int RenderLongTextRows { get; init; } = 4;
    #endregion
}
