namespace System.Design;

/// <summary>
/// 这个类型是<see cref="IEventBusFactory"/>的实现，
/// 可以作为一个事件总线工厂
/// </summary>
sealed class EventBusFactory : IEventBusFactory
{
    #region 创建事件总线上下文
    public IEventBusContext<Event> Create<Event>()
        where Event : Delegate
        => new EventBusContext<Event>();
    #endregion
}
