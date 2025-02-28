namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个事件总线注册器
/// </summary>
/// <inheritdoc cref="IEventBusContext{Event}"/>
public interface IEventBusRegister<Event>
    where Event : Delegate
{
    #region 注册事件
    /// <summary>
    /// 将事件注册进事件总线中，
    /// 并返回一个可以移除事件的<see cref="IDisposable"/>
    /// </summary>
    /// <param name="event">要注册的事件</param>
    /// <returns></returns>
    IDisposable Register(Event @event);
    #endregion
}
