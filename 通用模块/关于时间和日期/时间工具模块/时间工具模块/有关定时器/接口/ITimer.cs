namespace System.Time
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个定时器
    /// </summary>
    public interface ITimer : IDisposable
    {
        #region 说明文档
        /*在实现本接口时，请遵循以下规范：
          #如果定时器只执行指定的次数，
          则当定时器最后一次执行完毕后，销毁定时器
        
          #虽然Due事件是同步方法，
          但是执行它的时候，不要阻塞当前线程*/
        #endregion
        #region 定时器触发时执行的事件
        /// <summary>
        /// 当定时器到期时，触发这个事件
        /// </summary>
        event Action? Due;
        #endregion
        #region 启动定时器
        /// <summary>
        /// 启动定时器，这个方法只能调用一次
        /// </summary>
        void Start();
        #endregion
    }
}
