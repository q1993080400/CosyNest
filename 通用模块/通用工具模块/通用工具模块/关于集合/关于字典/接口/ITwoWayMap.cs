namespace System.Collections.Generic;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个双向映射表
/// </summary>
/// <typeparam name="A">要映射的第一个对象类型</typeparam>
/// <typeparam name="B">要映射的第二个对象类型</typeparam>
public interface ITwoWayMap<A, B> : IReadOnlyCollection<(A, B)>
    where A : notnull
    where B : notnull
{
    #region 获取映射的值
    #region 将A映射为B
    #region 会引发异常
    /// <summary>
    /// 将A对象映射为B对象
    /// </summary>
    /// <param name="a">要映射的A对象</param>
    /// <returns></returns>
    B AMapB(A a);
    #endregion
    #region 不会引发异常
    /// <summary>
    /// 将A对象映射为B对象，
    /// 如果不存在此映射，不会引发异常
    /// </summary>
    /// <param name="key">要映射的A对象</param>
    /// <param name="notFound">如果不存在此映射，则通过这个延迟对象返回一个默认值</param>
    /// <returns></returns>
    (bool Exist, B? Value) TryAMapB(A key, LazyPro<B>? notFound = null);
    #endregion
    #endregion
    #region 将B映射为A
    #region 会引发异常
    /// <summary>
    /// 将B对象映射为A对象
    /// </summary>
    /// <param name="b">要映射的B对象</param>
    /// <returns></returns>
    A BMapA(B b);
    #endregion
    #region 不会引发异常
    /// <summary>
    /// 将B对象映射为A对象，
    /// 如果不存在此映射，不会引发异常
    /// </summary>
    /// <param name="key">要映射的B对象</param>
    /// <param name="noFound">如果不存在此映射，则通过这个延迟对象返回一个默认值</param>
    /// <returns></returns>
    (bool Exist, A? Value) TryBMapA(B key, LazyPro<A>? noFound = null);
    #endregion
    #endregion
    #endregion
    #region 注册映射
    #region 注册单个映射
    /// <summary>
    /// 注册一个双向映射，双向映射指的是：
    /// 两个对象的映射只能是一对一的，
    /// 而且能够通过任意一个找到另一个
    /// </summary>
    /// <param name="a">映射的A对象</param>
    /// <param name="b">映射的B对象</param>
    void RegisteredMap(A a, B b);
    #endregion
    #region 批量注册映射
    /// <summary>
    /// 批量注册映射
    /// </summary>
    /// <param name="maps">映射的A对象和B对象</param>
    void RegisteredMapRange(IEnumerable<(A A, B B)> maps)
    {
        foreach (var (a, b) in maps)
        {
            RegisteredMap(a, b);
        }
    }
    #endregion
    #endregion
}
