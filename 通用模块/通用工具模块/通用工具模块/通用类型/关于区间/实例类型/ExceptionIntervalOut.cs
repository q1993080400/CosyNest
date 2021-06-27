namespace System
{
    /// <summary>
    /// 表示由于对象不在指定区间中所引发的异常
    /// </summary>
    /// <typeparam name="Obj">位于区间中的对象，
    /// 它通过<see cref="IComparable{T}"/>进行比较</typeparam>
    public class ExceptionIntervalOut<Obj> : Exception
    {
        #region 对象的合法区间
        /// <summary>
        /// 返回对象的合法区间
        /// </summary>
        public IInterval<Obj> Interval { get; }
        #endregion
        #region 引发异常的对象
        /// <summary>
        /// 获取引发异常的对象
        /// </summary>
        public Obj ExceptionObj { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的对象和区间初始化异常
        /// </summary>
        /// <param name="exceptionObj">引发异常的对象</param>
        /// <param name="interval">对象的合法区间</param>
        public ExceptionIntervalOut(Obj exceptionObj, IInterval<Obj> interval)
            : base($"{exceptionObj}不在区间中，合法{interval}")
        {
            this.ExceptionObj = exceptionObj;
            this.Interval = interval;
        }
        #endregion
    }
    #region 静态辅助类型
    /// <summary>
    /// 这个静态类可以用来帮助创建因区间所引发的异常
    /// </summary>
    public static class ExceptionIntervalOut
    {
        #region 创建异常
        /// <summary>
        /// 使用指定的对象和区间初始化异常
        /// </summary>
        /// <param name="exceptionObj">引发异常的对象</param>
        /// <param name="interval">对象的合法区间</param>
        /// <returns>新创建的异常对象</returns>
        public static ExceptionIntervalOut<Obj> Create<Obj>(Obj exceptionObj, IInterval<Obj> interval)
            => new(exceptionObj, interval);
        #endregion
        #region 检查对象是否位于区间中
        #region 传入区间
        /// <summary>
        /// 检查一个数组中的全部对象是否都位于合法区间中，如果不是，则引发异常
        /// </summary>
        /// <typeparam name="Obj">位于区间中的对象，
        /// 它通过<see cref="IComparable{T}"/>进行比较</typeparam>
        /// <param name="interval">对象的合法区间</param>
        /// <param name="objs">待检查的对象数组</param>
        /// <exception cref="ExceptionIntervalOut{Obj}"><paramref name="objs"/>中的一个或多个对象不在区间中</exception>
        public static void Check<Obj>(IInterval<Obj> interval, params Obj[] objs)
        {
            foreach (var obj in objs)
            {
                if (interval.CheckInInterval(obj) != IntervalPosition.Located)
                    throw Create(obj, interval);
            }
        }
        #endregion
        #region 直接传入下限和上限
        /// <summary>
        /// 检查一个数组中的全部对象是否都位于合法区间中，如果不是，则引发异常
        /// </summary>
        /// <typeparam name="Obj">位于区间中的对象，
        /// 它通过<see cref="IComparable{T}"/>进行比较</typeparam>
        /// <param name="min">区间的下限，
        /// 如果为<see langword="null"/>，代表没有下限</param>
        /// <param name="max">区间的上限，
        /// 如果为<see langword="null"/>，代表没有上限</param>
        /// <param name="objs">待检查的对象数组</param>
        /// <exception cref="ExceptionIntervalOut{Obj}"><paramref name="objs"/>中的一个或多个对象不在区间中</exception>
        public static void Check<Obj>(IComparable<Obj>? min, IComparable<Obj>? max, params Obj[] objs)
            => Check(IInterval.Create(min, max), objs);
        #endregion
        #endregion
    }
    #endregion
}
