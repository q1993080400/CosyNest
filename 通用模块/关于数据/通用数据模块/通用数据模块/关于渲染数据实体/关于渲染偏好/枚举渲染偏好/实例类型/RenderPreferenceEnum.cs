namespace System.DataFrancis;

/// <summary>
/// 这个记录指示对枚举数据类型的渲染偏好
/// </summary>
public sealed record RenderPreferenceEnum : RenderPreference, ICreate<RenderPreferenceEnum>
{
    #region 静态抽象成员实现：创建对象
    public static RenderPreferenceEnum Create()
        => new()
        {
            RenderEnum = default,
            SortByPinyin = false
        };
    #endregion
    #region 渲染枚举时的偏好
    /// <summary>
    /// 在渲染枚举时的偏好
    /// </summary>
    public required FormEnumRender RenderEnum { get; init; }
    #endregion
    #region 是否按拼音排序
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则在渲染枚举时，还会按拼音排序
    /// </summary>
    public required bool SortByPinyin { get; init; }
    #endregion
    #region 抽象实现：返回值的文本
    public override string? RenderToText(object value)
        => value switch
        {
            Enum @enum => @enum.GetDescription(),
            var v => throw new NotSupportedException($"无法按照渲染偏好返回{v}的文本，它只支持枚举")
        };
    #endregion
}
