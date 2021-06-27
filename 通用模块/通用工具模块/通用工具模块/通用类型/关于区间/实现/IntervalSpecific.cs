namespace System
{
    /// <summary>
    /// 这个类型是<see cref="IIntervalSpecific{Obj}"/>的实现，
    /// 可以视为一个具体区间
    /// </summary>
    /// <typeparam name="Obj">位于区间中的对象</typeparam>
    class IntervalSpecific<Obj> : Interval<Obj>, IIntervalSpecific<Obj>
        where Obj : struct, IComparable<Obj>
    {
        #region 构造函数
        /// <summary>
        /// 使用指定的最小值和最大值初始化对象
        /// </summary>
        /// <param name="min">区间的最小值</param>
        /// <param name="max">区间的最大值</param>
        public IntervalSpecific(Obj? min, Obj? max)
        {
            if (min is { } && max is { } && min.Value.CompareTo(max.Value) > 0)
                Tool.Exchange(ref min, ref max);
            this.Min = min;
            this.Max = max;
        }
        #endregion
    }
}
