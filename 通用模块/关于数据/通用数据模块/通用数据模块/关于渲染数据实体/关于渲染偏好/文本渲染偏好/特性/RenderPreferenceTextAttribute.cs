namespace System.DataFrancis;

/// <summary>
/// 这个特性指示了对文本数据类型的渲染偏好
/// </summary>
public sealed class RenderPreferenceTextAttribute : RenderPreferenceAttribute
{
    #region 渲染长文本时的行数
    /// <summary>
    /// 指示在渲染长文本时，
    /// 最多应该渲染的行数
    /// </summary>
    public int RenderLongTextRows { get; init; } = 4;
    #endregion
    #region 渲染字符串时的偏好
    /// <summary>
    /// 在渲染字符串时的偏好
    /// </summary>
    public FormTextRender RenderText { get; init; }
    #endregion
    #region 是否显示复制文字
    /// <summary>
    /// 如果这个值为<see langword="true"/>，则显示复制文字按钮
    /// </summary>
    public bool ShowCopyText { get; init; }
    #endregion
    #region 抽象成员实现：返回渲染偏好
    public override RenderPreferenceText GetRenderPreference()
        => new()
        {
            RenderText = RenderText,
            RenderLongTextRows = RenderLongTextRows,
            ShowCopyText = ShowCopyText
        };
    #endregion
}
