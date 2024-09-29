namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个事件总线注册器，
/// 它可以注册事件，并在调用<see cref="IDisposable.Dispose"/>时自动移除事件
/// </summary>
/// <inheritdoc cref="IEventBusContext{Event}"/>
public interface IEventBusRegister<Event> : IDisposable
    where Event : Delegate
{
    #region 注册事件
    /// <summary>
    /// 向事件总线上下文注册事件
    /// </summary>
    /// <param name="event">要注册的事件</param>
    void Register(Event @event);
    #endregion
}
