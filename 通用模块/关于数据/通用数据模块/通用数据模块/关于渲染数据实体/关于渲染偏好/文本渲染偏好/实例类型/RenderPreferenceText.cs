namespace System.DataFrancis;

/// <summary>
/// 这个记录指示对文本数据类型的渲染偏好
/// </summary>
public sealed record RenderPreferenceText : RenderPreference, ICreate<RenderPreferenceText>
{
    #region 静态抽象成员实现：创建对象
    public static RenderPreferenceText Create()
        => new()
        {
            RenderText = default,
            RenderLongTextRows = 4,
            ShowCopyText = false
        };
    #endregion
    #region 渲染字符串时的偏好
    /// <summary>
    /// 在渲染字符串时的偏好
    /// </summary>
    public required FormTextRender RenderText { get; init; }
    #endregion
    #region 渲染长文本时的行数
    /// <summary>
    /// 指示在渲染长文本时，
    /// 最多应该渲染的行数
    /// </summary>
    public required int RenderLongTextRows { get; init; }
    #endregion
    #region 是否显示复制文字
    /// <summary>
    /// 如果这个值为<see langword="true"/>，则显示复制文字按钮
    /// </summary>
    public required bool ShowCopyText { get; init; }
    #endregion
    #region 抽象实现：返回值的文本
    public override string? RenderToText(object value)
        => value switch
        {
            string text => text,
            var v => throw new NotSupportedException($"无法按照渲染偏好返回{v}的文本，它只支持文本")
        };
    #endregion
}
