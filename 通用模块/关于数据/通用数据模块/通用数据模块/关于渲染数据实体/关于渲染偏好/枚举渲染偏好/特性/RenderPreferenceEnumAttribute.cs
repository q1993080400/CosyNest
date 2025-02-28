namespace System.DataFrancis;

/// <summary>
/// 这个特性指示了对枚举数据类型的渲染偏好
/// </summary>
public sealed class RenderPreferenceEnumAttribute : RenderPreferenceAttribute
{
    #region 渲染枚举时的偏好
    /// <summary>
    /// 如果这个特性是枚举，
    /// 指定在渲染枚举时的偏好
    /// </summary>
    public FormEnumRender RenderEnum { get; init; }
    #endregion
    #region 抽象成员实现：返回渲染偏好
    public override RenderPreferenceEnum GetRenderPreference()
        => new()
        {
            RenderEnum = RenderEnum
        };
    #endregion
}
