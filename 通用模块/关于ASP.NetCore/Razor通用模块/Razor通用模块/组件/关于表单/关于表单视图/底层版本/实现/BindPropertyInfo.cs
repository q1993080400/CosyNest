using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录允许绑定表单属性
/// </summary>
/// <param name="FormModel">要渲染的模型</param>
/// <param name="Property">要渲染的属性</param>
/// <inheritdoc cref="IBindProperty{Obj}"/>
sealed record BindPropertyInfo<Obj>(object FormModel, PropertyInfo Property) : IBindProperty<Obj>
{
    #region 公开成员
    #region 属性的值
    public Obj? Value
    {
        get => (Obj?)Property.GetValue(FormModel);
        set
        {
            Property.SetValue(FormModel, value.To(Property.PropertyType));
        }
    }
    #endregion
    #endregion
}
