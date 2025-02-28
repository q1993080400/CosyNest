using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型允许以桥接的方式绑定表单视图属性，
/// 它支持类型转换，能够与不兼容的属性类型进行适配
/// </summary>
/// <typeparam name="Adapter">用来进行适配的类型，
/// 它是UI组件能够识别的类型</typeparam>
/// <typeparam name="Bottom">底层的类型，
/// 它是能够与表单视图的模型兼容的类型</typeparam>
/// <param name="formModel">要渲染的模型</param>
/// <param name="property">要渲染的属性</param>
/// <param name="convert">进行转换的委托</param>
/// <param name="convertReverse">进行反向转换的委托</param>
sealed class BindPropertyInfoAdapter<Adapter, Bottom>
    (object formModel, PropertyInfo property,
    Func<Bottom?, Adapter?> convert, Func<Adapter?, Bottom?> convertReverse) : IBindProperty<Adapter>
{
    #region 属性的值
    public Adapter? Value
    {
        get => convert(property.GetValue<Bottom>(formModel));
        set
        {
            var setValue = convertReverse(value);
            property.SetValue(formModel, setValue);
        }
    }
    #endregion
}
