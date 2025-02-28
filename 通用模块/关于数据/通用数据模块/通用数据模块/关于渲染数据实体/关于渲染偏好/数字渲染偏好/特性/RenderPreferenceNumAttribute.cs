namespace System.DataFrancis;

/// <summary>
/// 这个特性指示了对数字数据类型的渲染偏好
/// </summary>
public sealed class RenderPreferenceNumAttribute : RenderPreferenceAttribute, IRenderHasFormat
{
    #region 格式化字符串
    public string? Format { get; init; }
    #endregion
    #region 渲染数字时的偏好
    /// <summary>
    /// 在渲染数字时的偏好
    /// </summary>
    public FormNumRender RenderNum { get; init; }
    #endregion
    #region 抽象成员实现：返回渲染偏好
    public override RenderPreferenceNum GetRenderPreference()
        => new()
        {
            RenderNum = RenderNum,
            Format = Format
        };
    #endregion
}
