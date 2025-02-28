using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型允许绑定到表单视图属性
/// </summary>
/// <param name="formModel">要渲染的模型</param>
/// <param name="property">要渲染的属性</param>
/// <inheritdoc cref="IBindProperty{Property}"/>
sealed class BindPropertyInfo<Property>(object formModel, PropertyInfo property) : IBindProperty<Property>
{
    #region 属性的值
    public Property? Value
    {
        get => property.GetValue(formModel).To<Property?>();
        set
        {
            property.SetValue(formModel, value.To(property.PropertyType));
        }
    }
    #endregion
}
