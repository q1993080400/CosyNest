namespace System;

/// <summary>
/// 这个特性可以用来描述枚举的值，
/// 应该在UI上映射为什么文本
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class EnumDescribeAttribute : Attribute
{
    #region 描述
    /// <summary>
    /// 获取枚举对应的文本描述
    /// </summary>
    public required string Describe { get; init; }
    #endregion
    #region 排序
    /// <summary>
    /// 获取枚举在UI中的排序，
    /// 以升序排列
    /// </summary>
    public int Order { get; init; }
    #endregion
}
