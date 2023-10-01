namespace System;

/// <summary>
/// 关于性能的扩展方法全部放在这个类中，通常无需专门调用
/// </summary>
public static class ExtendPerformance
{
    #region 关于弱引用
    #region 删除掉一个弱引用集合中所有丢失引用的元素
    /// <summary>
    /// 删除一个弱引用集合中，所有目标引用丢失的元素
    /// </summary>
    /// <typeparam name="Weak">集合元素的类型</typeparam>
    public static void RemoveNull<Weak>(this ICollection<Weak> weakList)
        where Weak : WeakReference
        => weakList.RemoveWhere(x => x.Target is null);
    #endregion
    #endregion
    #region 关于IDisposable
    #region 求值，然后自动释放IDisposable
    /// <summary>
    /// 对一个<see cref="IDisposable"/>应用一个委托，
    /// 返回委托的返回值，然后释放掉这个<see cref="IDisposable"/>
    /// </summary>
    /// <typeparam name="Obj">待求值和释放的对象类型</typeparam>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="obj">待求值和释放的对象</param>
    /// <param name="delegate">用于获取返回值的委托</param>
    /// <returns>执行<paramref name="delegate"/>所获取到的返回值</returns>
    public static Ret AutoRelease<Obj, Ret>(this Obj obj, Func<Obj, Ret> @delegate)
        where Obj : IDisposable
    {
        using (obj)
        {
            return @delegate(obj);
        }
    }
    #endregion
    #region 批量释放IDisposable
    /// <summary>
    /// 批量释放<see cref="IDisposable"/>
    /// </summary>
    /// <param name="collections">待释放的<see cref="IDisposable"/></param>
    public static void DisposableAll(this IEnumerable<IDisposable> collections)
        => collections.ForEach(x => x.Dispose());
    #endregion
    #endregion
    #region 关于IAsyncDisposable
    #region 同步释放IAsyncDisposable
    /// <summary>
    /// 同步释放一个<see cref="IAsyncDisposable"/>，
    /// 注意：本方法只是将<see cref="IAsyncDisposable"/>放入线程池排队释放，
    /// 在本方法返回时，不能保证它已经释放完毕
    /// </summary>
    /// <param name="obj">待释放的对象</param>
    public static void RequestDispose(this IAsyncDisposable obj)
        => Task.Run(obj.DisposeAsync);
    #endregion
    #endregion
}
