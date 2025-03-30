namespace System.DataFrancis;

/// <summary>
/// 这个记录指示对日期数据类型的渲染偏好
/// </summary>
public sealed record RenderPreferenceDate : RenderPreference, IRenderHasFormat, ICreate<RenderPreferenceDate>
{
    #region 静态抽象成员实现：创建对象
    public static RenderPreferenceDate Create()
        => new()
        {
            Format = null
        };
    #endregion
    #region 格式化字符串
    public required string? Format { get; init; }
    #endregion
    #region 抽象实现：返回值的文本
    public override string? RenderToText(object value)
        => value switch
        {
            DateTimeOffset date => date.ToString(Format),
            DateOnly dateOnly => dateOnly.ToString(Format),
            TimeOnly timeOnly => timeOnly.ToString(Format),
            var v => throw new NotSupportedException($"无法按照渲染偏好返回{v}的文本，它只支持日期")
        };
    #endregion
}
