namespace System.DataFrancis;

/// <summary>
/// 这个记录是所有渲染表单组件时的偏好的基类
/// </summary>
public abstract record RenderPreference
{
    #region 静态成员：转换或创建
    /// <summary>
    /// 尝试将抽象的渲染偏好转换为具体的渲染偏好类型，
    /// 如果转换失败，则创建一个空白的渲染偏好
    /// </summary>
    /// <typeparam name="Preference">具体渲染偏好的类型</typeparam>
    /// <param name="renderPreference">要转换的抽象渲染偏好</param>
    /// <returns></returns>
    public static Preference ConvertOrCreate<Preference>(RenderPreference? renderPreference)
        where Preference : RenderPreference, ICreate<Preference>
        => (renderPreference as Preference) ?? Preference.Create();
    #endregion
    #region 抽象方法：返回值的文本
    /// <summary>
    /// 根据渲染偏好，将属性的值转换为文本
    /// </summary>
    /// <param name="value">要转换的属性</param>
    /// <returns></returns>
    public abstract string? RenderToText(object value);
    #endregion
}
