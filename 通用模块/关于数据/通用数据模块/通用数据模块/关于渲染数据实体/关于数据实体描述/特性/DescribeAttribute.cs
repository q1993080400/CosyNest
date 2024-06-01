namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个特性用来生成对数据属性的描述
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class DescribeAttribute : Attribute
{
    #region 对数据属性的描述
    /// <summary>
    /// 获取对数据属性的描述
    /// </summary>
    public required string Describe { get; init; }
    #endregion
    #region 折叠描述
    /// <summary>
    /// 如果这个值不为<see langword="null"/>，
    /// 则数据描述会折叠显示，且这个属性作为折叠栏的标题
    /// </summary>
    public string? CollapseTitle { get; init; }
    #endregion
}
