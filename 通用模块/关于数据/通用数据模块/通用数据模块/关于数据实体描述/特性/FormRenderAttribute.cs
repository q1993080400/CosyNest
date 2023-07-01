namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个特性指定了在表单中渲染属性的方式
/// </summary>
/// <typeparam name="RenderWay">渲染方式枚举</typeparam>
[AttributeUsage(AttributeTargets.Property)]
public abstract class FormRenderAttribute<RenderWay> : Attribute
    where RenderWay : Enum
{
    #region 渲染方式
    /// <summary>
    /// 获取属性的渲染方式
    /// </summary>
    public required RenderWay Render { get; init; }
    #endregion
}
