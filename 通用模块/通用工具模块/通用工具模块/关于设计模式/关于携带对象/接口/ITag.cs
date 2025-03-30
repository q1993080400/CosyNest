namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个携带资源的对象
/// </summary>
/// <typeparam name="Obj">携带的对象的类型</typeparam>
public interface ITag<Obj>
{
    #region 对象内容
    /// <summary>
    /// 获取这个对象封装的内容
    /// </summary>
    Obj? Content { get; }
    #endregion
    #region 获取对象内容，不可为null
    /// <summary>
    /// 获取对象封装的内容，
    /// 如果为<see langword="null"/>，
    /// 会引发一个异常
    /// </summary>
    /// <returns></returns>
    Obj CheckContent();
    #endregion
}
