using System.Collections.Immutable;

namespace System.Design;

/// <summary>
/// 这个类型是<see cref="IEventBusContext{Event}"/>的实现，
/// 可以视为一个事件总线上下文
/// </summary>
/// <inheritdoc cref="IEventBusContext{Event}"/>
sealed class EventBusContext<Event> : IEventBusContext<Event>, IEventBusRegister<Event>
    where Event : Delegate
{
    #region 公开成员
    #region 返回所有事件
    public IReadOnlyCollection<Event> AllEvent
        => [.. AllEventCache.Values];
    #endregion
    #region 事件总线注册器
    public IEventBusRegister<Event> Register
        => this;
    #endregion
    #region 注册事件
    IDisposable IEventBusRegister<Event>.Register(Event @event)
    {
        var key = Guid.CreateVersion7();
        AllEventCache = AllEventCache.SetItem(key, @event);
        return FastRealize.Disposable(() => AllEventCache = AllEventCache.Remove(key));
    }
    #endregion
    #region 释放对象
    public void Dispose()
    {
        AllEventCache = ImmutableDictionary<Guid, Event>.Empty;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 缓存所有事件
    /// <summary>
    /// 缓存目前的所有事件
    /// </summary>
    private ImmutableDictionary<Guid, Event> AllEventCache { get; set; }
        = ImmutableDictionary<Guid, Event>.Empty;
    #endregion
    #endregion
}
