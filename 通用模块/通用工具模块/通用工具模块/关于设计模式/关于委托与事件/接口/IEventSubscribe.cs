namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个事件订阅器
/// </summary>
/// <typeparam name="Event">要订阅的事件的类型</typeparam>
public interface IEventSubscribe<Event>
    where Event : Delegate
{
    #region 添加委托
    /// <summary>
    /// 向调用列表添加一个委托
    /// </summary>
    /// <param name="delegate">要添加的委托</param>
    void Add(Event @delegate);
    #endregion
    #region 移除委托
    /// <summary>
    /// 向调用列表中移除委托
    /// </summary>
    /// <param name="delegate">要移除的委托</param>
    void Remove(Event @delegate);
    #endregion
}
