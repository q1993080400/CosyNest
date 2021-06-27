namespace System.Collections.Generic
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一个双向映射表
    /// </summary>
    /// <typeparam name="A">要映射的第一个对象类型</typeparam>
    /// <typeparam name="B">要映射的第二个对象类型</typeparam>
    public interface ITwoWayMap<A, B>
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
        B this[A a] { get; }
        #endregion
        #region 不会引发异常
        /// <summary>
        /// 将A对象映射为B对象，
        /// 如果不存在此映射，不会引发异常
        /// </summary>
        /// <param name="key">要映射的A对象</param>
        /// <param name="notFound">如果不存在此映射，则通过这个延迟对象返回一个默认值</param>
        /// <returns></returns>
        (bool Exist, B? Value) TryGetValue(A key, LazyPro<B>? notFound = null);
        #endregion
        #endregion
        #region 将B映射为A
        #region 会引发异常
        /// <summary>
        /// 将B对象映射为A对象
        /// </summary>
        /// <param name="b">要映射的B对象</param>
        /// <returns></returns>
        A this[B b] { get; }
        #endregion
        #region 不会引发异常
        /// <summary>
        /// 将B对象映射为A对象，
        /// 如果不存在此映射，不会引发异常
        /// </summary>
        /// <param name="key">要映射的B对象</param>
        /// <param name="noFound">如果不存在此映射，则通过这个延迟对象返回一个默认值</param>
        /// <returns></returns>
        (bool Exist, A? Value) TryGetValue(B key, LazyPro<A>? noFound = null);
        #endregion
        #endregion
        #endregion
        #region 注册映射
        #region 注册双向映射
        /// <summary>
        /// 注册一个双向映射，双向映射指的是：
        /// 两个对象的映射只能是一对一的，
        /// 而且能够通过任意一个找到另一个
        /// </summary>
        /// <param name="map">这个元组的两个项会互相映射</param>
        void RegisteredTwo(params (A a, B b)[] map);
        #endregion
        #region 注册从A到B的单向映射
        /// <summary>
        /// 注册从A到B的单向映射，单向映射指的是：
        /// 只能通过A找到B，但是这个映射可以是一对多的
        /// </summary>
        /// <param name="to">传入下个参数中的任意一个A对象，
        /// 都会映射到这个B对象</param>
        /// <param name="from">通过本集合的任意一个A对象，
        /// 都可以找到上个参数的B对象</param>
        void RegisteredOne(B to, params A[] from);
        #endregion
        #region 注册从B到A的单向映射
        /// <summary>
        /// 注册从B到A的单项映射，单项映射指的是：
        /// 只能通过B找到A，但是这个映射可以是一对多的
        /// </summary>
        /// <param name="to">传入下个参数中的任意一个B对象，
        /// 都会映射到这个A对象</param>
        /// <param name="from">通过本集合的任意一个B对象，
        /// 都可以找到上个参数的A对象</param>
        void RegisteredOne(A to, params B[] from);
        #endregion
        #endregion
    }
}
