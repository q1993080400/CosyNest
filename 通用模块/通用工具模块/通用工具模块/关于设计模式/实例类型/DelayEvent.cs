namespace System.Design
{
    /// <summary>
    /// 表示一个延迟加载的事件，
    /// 当事件第一次被注册时，执行一个初始化操作，
    /// 事件中的委托被全部移除时，执行一个清理操作
    /// </summary>
    public abstract class DelayEvent<Event> : IDisposable
        where Event : Delegate
    {
        #region 运算符重载
        #region 说明文档
        /*问：这两个运算符为什么返回动态类型？
          答：因为作者的设想是，添加和移除延迟事件的方式和普通事件相同，
          直接使用+=和-=运算符即可，但是这样又产生了一个新问题：
          由于该运算符不知道派生类的具体类型，所以只能返回DelayEvent<Event>，
          但是这样一来，给字段赋值的时候可能需要类型转换，因此只能使用动态类型来规避这个问题*/
        #endregion
        #region 重载+
        /// <summary>
        /// 向事件中注册新委托，
        /// 如果注册导致委托不为<see langword="null"/>，
        /// 则执行初始化方法
        /// </summary>
        /// <param name="delayEvent">待注册的事件</param>
        /// <param name="event">要注册进事件的委托</param>
        /// <returns>原路返回参数<paramref name="delayEvent"/></returns>
        public static DelayEvent<Event> operator +(DelayEvent<Event> delayEvent, Event? @event)
        {
            var de = delayEvent.Delegate;
            if (de is null && @event is { })
                delayEvent.Initialization();
            delayEvent.Delegate = (Event?)System.Delegate.Combine(de, @event);
            return delayEvent;
        }
        #endregion
        #region 重载-
        /// <summary>
        /// 从事件中移除委托，
        /// 如果移除了全部委托，则执行清理方法
        /// </summary>
        /// <param name="delayEvent">待移除委托的事件</param>
        /// <param name="event">要从事件中移除的委托</param>
        /// <returns>原路返回参数<paramref name="delayEvent"/></returns>
        public static DelayEvent<Event> operator -(DelayEvent<Event> delayEvent, Event? @event)
        {
            var de = delayEvent.Delegate;
            var @delegate = System.Delegate.RemoveAll(de, @event);
            if (@delegate is null && de is { })
                delayEvent.Dispose();
            delayEvent.Delegate = (Event?)@delegate;
            return delayEvent;
        }
        #endregion
        #endregion
        #region 封装的事件委托
        /// <summary>
        /// 获取封装的事件委托
        /// </summary>
        public Event? Delegate { get; private set; }
        #endregion
        #region 初始化事件
        /// <summary>
        /// 在事件被第一次注册后，
        /// 执行这个方法初始化事件所需要的对象
        /// </summary>
        protected abstract void Initialization();
        #endregion
        #region 清理事件
        /// <summary>
        /// 当事件中的委托被全部移除时，
        /// 执行这个方法清理事件所依赖的对象
        /// </summary>
        public abstract void Dispose();
        #endregion
    }
}
