namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来创建一个默认的对象实例
/// </summary>
/// <typeparam name="Obj">要创建的默认对象实例的类型</typeparam>
public interface ICreate<out Obj>
    where Obj : class
{
    #region 创建空白对象
    /// <summary>
    /// 创建一个默认的对象实例，并返回
    /// </summary>
    /// <returns></returns>
    static abstract Obj Create();
    #endregion
}
