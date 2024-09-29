namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来创建事件总线上下文
/// </summary>
public interface IEventBusFactory
{
    #region 创建事件总线上下文
    /// <summary>
    /// 创建一个事件总线上下文，并返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IEventBusContext{Event}"/>
    IEventBusContext<Event> Create<Event>()
        where Event : Delegate;
    #endregion
}
