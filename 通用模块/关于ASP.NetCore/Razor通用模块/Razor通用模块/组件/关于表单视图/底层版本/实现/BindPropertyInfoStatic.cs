using System.DataFrancis;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录允许绑定到表单视图属性，
/// 但是它其实只是静态地返回一个固定值，
/// 它适用于只读模式
/// </summary>
/// <param name="value">固定返回的值</param>
/// <inheritdoc cref="IBindProperty{Obj}"/>
sealed class BindPropertyInfoStatic<Obj>(Obj value) : IBindProperty<Obj>
{
    #region 属性的值
    public Obj? Value
    {
        get => value;
        set { }
    }
    #endregion
}
