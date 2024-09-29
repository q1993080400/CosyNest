namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将用来绑定一个单一属性
/// </summary>
/// <typeparam name="Property">要绑定属性的类型</typeparam>
public interface IBindProperty<Property> : IBind<Property>
{
    #region 要绑定的值
    /// <summary>
    /// 获取要绑定的值，
    /// 当它被修改的时候，会同步更新
    /// </summary>
    Property? Value { get; set; }
    #endregion
}
