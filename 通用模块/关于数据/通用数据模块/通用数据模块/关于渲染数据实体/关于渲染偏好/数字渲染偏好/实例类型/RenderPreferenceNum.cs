namespace System.DataFrancis;

/// <summary>
/// 这个记录指示对数字数据类型的渲染偏好
/// </summary>
public sealed record RenderPreferenceNum : RenderPreference, IRenderHasFormat, ICreate<RenderPreferenceNum>
{
    #region 静态抽象成员实现：创建对象
    public static RenderPreferenceNum Create()
        => new()
        {
            RenderNum = default,
            Format = null
        };
    #endregion
    #region 渲染数字时的偏好
    /// <summary>
    /// 在渲染数字时的偏好
    /// </summary>
    public required FormNumRender RenderNum { get; init; }
    #endregion
    #region 格式化字符串
    public required string? Format { get; init; }
    #endregion
    #region 抽象实现：返回值的文本
    public override string? RenderToText(object value)
        => value switch
        {
            IFormattable formattable => formattable.ToString(Format, null),
            var v => throw new NotSupportedException($"无法按照渲染偏好返回{v}的文本，它只支持数字")
        };
    #endregion
}
