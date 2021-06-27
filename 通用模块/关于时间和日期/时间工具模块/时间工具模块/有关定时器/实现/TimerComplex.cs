using System.Threading;

namespace System.Time
{
    /// <summary>
    /// 表示一个复杂的定时器，
    /// 它不是通过固定间隔，
    /// 而是使用一个委托确定下次执行的时间
    /// </summary>
    class TimerComplex : ITimer
    {
        #region 封装的对象
        #region 计时器
        /// <summary>
        /// 获取封装的计时器，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private Timer? PackTimer { get; set; }
        #endregion
        #region 确定下次执行时间的委托
        /// <summary>
        /// 调用这个委托可以确定下次触发定时器的时间
        /// </summary>
        private NextDate NextDate { get; }
        #endregion
        #endregion
        #region 接口实现
        #region 计时器到期触发的事件
        public event Action? Due;
        #endregion 
        #region 销毁计时器
        public void Dispose()
            => PackTimer?.Dispose();
        #endregion 
        #region 启动计时器
        public void Start()
        {
            if (PackTimer is null)
            {
                #region 用来确定时间差的本地函数
                static TimeSpan Fun(DateTimeOffset date)
                    => date - DateTimeOffset.Now;
                #endregion
                var delay = NextDate.NextAtNow();
                if (delay is { })
                {
                    var period = TimeSpan.FromMilliseconds(-1);
                    PackTimer = new Timer(x =>
                    {
                        Due?.Invoke();
                        var next = NextDate.NextAtNow();
                        if (next is null)
                            Dispose();
                        else PackTimer!.Change(Fun(next.Value), period);
                    }, null, Fun(delay.Value), period);
                }
            }
        }
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="nextDate">调用这个委托可以确定下次触发定时器的时间</param>
        public TimerComplex(NextDate nextDate)
        {
            this.NextDate = nextDate ?? throw new ArgumentNullException(nameof(nextDate));
        }
        #endregion
    }
}
