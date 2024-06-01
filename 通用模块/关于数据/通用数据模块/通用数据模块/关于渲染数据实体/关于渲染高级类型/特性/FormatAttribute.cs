namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个特性指示属性的格式化字符串
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class FormatAttribute : Attribute
{
    #region 格式化字符串
    /// <summary>
    /// 指示属性的格式化字符串
    /// </summary>
    public required string Format { get; init; }
    #endregion
}
