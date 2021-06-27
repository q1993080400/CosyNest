namespace System.Time
{
    /// <summary>
    /// 这个静态类可以用来创建定时器
    /// </summary>
    public static class CreateTimer
    {
        #region 创建定时器
        #region 调用Start后才会启动
        /// <summary>
        /// 创建一个定时器，并返回
        /// </summary>
        /// <param name="interval">定时器触发的间隔</param>
        /// <param name="repeat">指定定时器应该重复多少次，
        /// 如果为<see langword="null"/>，代表无限重复</param>
        /// <returns>新创建的定时器，在显式调用它的<see cref="ITimer.Start"/>方法后才会开始运行</returns>
        public static ITimer Timer(TimeSpan interval, int? repeat = 1)
            => new TimerSimple(interval, repeat);
        #endregion
        #region 在指定的时间自动启动
        /// <summary>
        /// 创建一个在指定时间自动启动的定时器
        /// </summary>
        /// <param name="interval">定时器触发的间隔</param>
        /// <param name="repeat">指定定时器应该重复多少次，
        /// 如果为<see langword="null"/>，代表无限重复</param>
        /// <param name="begin">定时器的开始时间，
        /// 如果为<see langword="null"/>，代表立即启动</param>
        /// <returns>新创建的定时器，当到达<paramref name="begin"/>后，它会自动启动</returns>
        public static ITimer TimerAuto(TimeSpan interval, int? repeat = 1, DateTimeOffset? begin = null)
        {
            var timer = Timer(interval, repeat);
            if (begin is null)
                timer.Start();
            else
            {
                ExceptionIntervalOut.Check(DateTimeOffset.Now, null, begin.Value);
                var delay = Timer(begin.Value - DateTimeOffset.Now, 1);
                delay.Due += timer.Start;
                delay.Start();
            }
            return timer;
        }
        #endregion
        #region 使用委托确定启动时间
        /// <summary>
        /// 创建一个复杂的计时器，
        /// 它通过委托而不是固定的间隔确定触发时间
        /// </summary>
        /// <param name="nextDate">调用这个委托可以确定下次触发定时器的时间</param>
        /// <returns>新创建的计时器，它何时触发，是否重复取决于<paramref name="nextDate"/>的返回结果</returns>
        public static ITimer Timer(NextDate nextDate)
            => new TimerComplex(nextDate);
        #endregion
        #endregion
    }
}
