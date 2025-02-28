namespace System.DataFrancis;

/// <summary>
/// 这个特性指示了对日期数据类型的渲染偏好
/// </summary>
public sealed class RenderPreferenceDateAttribute : RenderPreferenceAttribute, IRenderHasFormat
{
    #region 格式化字符串
    public string? Format { get; init; }
    #endregion
    #region 抽象成员实现：返回渲染偏好
    public override RenderPreferenceDate GetRenderPreference()
        => new()
        {
            Format = Format
        };
    #endregion
}
