using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录允许绑定表单属性
/// </summary>
/// <typeparam name="Obj">属性的类型</typeparam>
public sealed record BindPropertyInfo<Obj>
{
    #region 公开成员
    #region 属性的值
    /// <summary>
    /// 获取或设置这个属性的值，
    /// 它会反映到表单模型中
    /// </summary>
    public Obj? Value
    {
        get => (Obj?)Property.GetValue(FormModel);
        set => Property.SetValue(FormModel, value.To(Property.PropertyType));
    }
    #endregion
    #endregion
    #region 内部成员
    #region 要渲染的模型
    /// <summary>
    /// 获取要渲染的模型
    /// </summary>
    private object FormModel { get; }
    #endregion
    #region 要渲染的属性
    /// <summary>
    /// 获取要渲染的属性
    /// </summary>
    private PropertyInfo Property { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的模型和属性初始化对象
    /// </summary>
    /// <param name="formModel">要渲染的模型</param>
    /// <param name="property">要渲染的属性</param>
    internal BindPropertyInfo(object formModel, PropertyInfo property)
    {
        FormModel = formModel;
        Property = property;
    }
    #endregion
}
