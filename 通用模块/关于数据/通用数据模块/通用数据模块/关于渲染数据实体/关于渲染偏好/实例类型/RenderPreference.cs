namespace System.DataFrancis;

/// <summary>
/// 这个记录声明了渲染表单组件时的偏好
/// </summary>
public sealed record RenderPreference
{
    #region 渲染布尔值时的偏好
    /// <summary>
    /// 在渲染布尔值时的偏好
    /// </summary>
    public FormBoolRender RenderBool { get; init; }
    #endregion
    #region 渲染枚举时的偏好
    /// <summary>
    /// 在渲染枚举时的偏好
    /// </summary>
    public FormEnumRender RenderEnum { get; init; }
    #endregion
    #region 渲染数字时的偏好
    /// <summary>
    /// 在渲染数字时的偏好
    /// </summary>
    public FormNumRender RenderNum { get; init; }
    #endregion
    #region 渲染字符串时的偏好
    /// <summary>
    /// 在渲染字符串时的偏好
    /// </summary>
    public FormStringRender RenderString { get; init; }
    #endregion
    #region 格式化字符串
    /// <summary>
    /// 这个属性指示格式化字符串，
    /// 只对部分数据类型有效
    /// </summary>
    public required string? Format { get; init; }
    #endregion
    #region 渲染长文本时的行数
    /// <summary>
    /// 指示在渲染长文本时，
    /// 最多应该渲染的行数
    /// </summary>
    public required int RenderLongTextRows { get; init; }
    #endregion
}
