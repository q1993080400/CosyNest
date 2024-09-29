namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来绑定属性
/// </summary>
/// <typeparam name="Contain">包含的对象的类型</typeparam>
public interface IBind<Contain>
{
    #region 强行转为绑定属性
    /// <summary>
    /// 将本对象强制转换为<see cref="IBindProperty{Property}"/>，
    /// 如果不能转换，会引发异常
    /// </summary>
    /// <returns></returns>
    IBindProperty<Contain> ToBindProperty()
        => this is IBindProperty<Contain> bindProperty ?
        bindProperty :
        throw new NotImplementedException($"这个{nameof(IBind<Contain>)}不是{nameof(IBindProperty<Contain>)}");
    #endregion
    #region 强行转为绑定范围
    /// <summary>
    /// 将本对象强制转换为<see cref="IBindRange{RangeValue}"/>，
    /// 如果不能转换，会引发异常
    /// </summary>
    /// <returns></returns>
    IBindRange<Contain> ToBindRange()
        => this is IBindRange<Contain> bindRange ?
        bindRange :
        throw new NotImplementedException($"这个{nameof(IBind<Contain>)}不是{nameof(IBindRange<Contain>)}");
    #endregion
}
