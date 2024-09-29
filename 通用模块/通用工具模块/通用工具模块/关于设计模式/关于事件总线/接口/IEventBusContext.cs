namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个事件总线上下文，
/// 它管理在自己范围内的事件
/// </summary>
/// <typeparam name="Event">事件委托的类型</typeparam>
public interface IEventBusContext<Event>
    where Event : Delegate
{
    #region 返回所有事件
    /// <summary>
    /// 返回目前的所有事件，
    /// 如果有需要，可以调用它们
    /// </summary>
    /// <returns></returns>
    IReadOnlyCollection<Event> AllEvent();
    #endregion
    #region 创建事件总线注册器
    /// <summary>
    /// 创建一个事件总线注册器，
    /// 它可以注册事件，并在并在调用<see cref="IDisposable.Dispose"/>时自动移除事件
    /// </summary>
    /// <returns></returns>
    IEventBusRegister<Event> CreateRegister();
    #endregion
}
