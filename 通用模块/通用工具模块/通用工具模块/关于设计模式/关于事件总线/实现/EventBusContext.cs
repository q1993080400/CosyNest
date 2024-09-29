using System.Collections.Immutable;

namespace System.Design;

/// <summary>
/// 这个类型是<see cref="IEventBusContext{Event}"/>的实现，
/// 可以视为一个事件总线上下文
/// </summary>
/// <inheritdoc cref="IEventBusContext{Event}"/>
sealed class EventBusContext<Event> : IEventBusContext<Event>
    where Event : Delegate
{
    #region 公开成员
    #region 返回所有事件
    public IReadOnlyCollection<Event> AllEvent()
        => AllEventCache.Values.ToArray();
    #endregion
    #region 创建事件总线注册器
    public IEventBusRegister<Event> CreateRegister()
        => new EventBusRegister<Event>(this, Guid.NewGuid());
    #endregion
    #endregion
    #region 内部成员
    #region 缓存所有事件
    /// <summary>
    /// 缓存目前的所有事件
    /// </summary>
    internal ImmutableDictionary<Guid, Event> AllEventCache { get; set; }
        = ImmutableDictionary<Guid, Event>.Empty;
    #endregion
    #endregion
}
