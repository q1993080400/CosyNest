namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来根据参数创建一个对象实例
/// </summary>
/// <typeparam name="Obj">要创建的对象的类型</typeparam>
/// <typeparam name="Parameter">用来创建对象的参数的类型</typeparam>
public interface ICreate<out Obj, in Parameter>
    where Obj : class
{
    #region 根据参数创建对象
    /// <summary>
    /// 根据参数创建对象，并返回
    /// </summary>
    /// <param name="parameter">用来创建对象的参数</param>
    /// <returns></returns>
    static abstract Obj Create(Parameter parameter);
    #endregion
}
