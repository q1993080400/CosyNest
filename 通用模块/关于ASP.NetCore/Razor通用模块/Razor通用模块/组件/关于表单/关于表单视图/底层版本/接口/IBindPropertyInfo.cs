namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来绑定表单视图属性
/// </summary>
/// <typeparam name="Obj">属性的类型</typeparam>
public interface IBindPropertyInfo<Obj>
{
    #region 属性的值
    /// <summary>
    /// 获取或设置这个属性的值，
    /// 它会反映到表单模型中
    /// </summary>
    Obj? Value { get; set; }
    #endregion
}
