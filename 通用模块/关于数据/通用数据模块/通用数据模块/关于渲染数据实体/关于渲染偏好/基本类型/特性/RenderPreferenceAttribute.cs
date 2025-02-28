namespace System.DataFrancis;

/// <summary>
/// 这个特性是所有表单属性渲染偏好特性的基类
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class RenderPreferenceAttribute : Attribute
{
    #region 抽象方法：返回渲染偏好
    /// <summary>
    /// 返回对渲染偏好的描述
    /// </summary>
    /// <returns></returns>
    public abstract RenderPreference GetRenderPreference();
    #endregion
}
