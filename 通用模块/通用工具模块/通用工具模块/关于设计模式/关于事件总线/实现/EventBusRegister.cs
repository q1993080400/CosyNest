namespace System.Design;

/// <summary>
/// 这个类型是<see cref="IEventBusRegister{Event}"/>的实现，
/// 可以视为一个事件总线注册器
/// </summary>
/// <param name="eventBusContext">这个事件总线注册器所在的事件总线上下文</param>
/// <param name="id">赋予这个对象的ID，它决定应该从事件总线上下文中移除哪个事件</param>
/// <inheritdoc cref="IEventBusContext{Event}"/>
sealed class EventBusRegister<Event>(EventBusContext<Event> eventBusContext, Guid id) : IEventBusRegister<Event>
    where Event : Delegate
{
    #region 注册事件
    public void Register(Event @event)
    {
        eventBusContext.AllEventCache = eventBusContext.AllEventCache.SetItem(id, @event);
    }
    #endregion
    #region 释放对象
    public void Dispose()
    {
        eventBusContext.AllEventCache = eventBusContext.AllEventCache.Remove(id);
    }
    #endregion 
}
