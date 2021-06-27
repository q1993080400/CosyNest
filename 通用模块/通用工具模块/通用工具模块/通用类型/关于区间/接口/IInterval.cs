namespace System
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个区间，它具备一个上限和下限
    /// </summary>
    /// <typeparam name="Obj">位于区间中的对象，
    /// 它通过<see cref="IComparable{T}"/>进行比较</typeparam>
    public interface IInterval<Obj>
    {
        #region 区间的下限
        /// <summary>
        /// 返回区间的下限，
        /// 如果为<see langword="null"/>，代表没有下限
        /// </summary>
        IComparable<Obj>? Min { get; }
        #endregion
        #region 区间的上限
        /// <summary>
        /// 返回区间的上限，
        /// 如果为<see langword="null"/>，代表没有上限
        /// </summary>
        IComparable<Obj>? Max { get; }
        #endregion
        #region 返回是否为封闭区间
        /// <summary>
        /// 返回是否为封闭区间，
        /// 也就是区间同时具有下限和上限
        /// </summary>
        bool IsClosed
            => Min is { } && Max is { };
        #endregion
        #region 解构区间
        /// <summary>
        /// 将区间解构为下限和上限
        /// </summary>
        /// <param name="min">用来接收区间下限的对象</param>
        /// <param name="max">用来接收区间上限的对象</param>
        void Deconstruct(out IComparable<Obj>? min, out IComparable<Obj>? max)
        {
            min = this.Min;
            max = this.Max;
        }
        #endregion
        #region 检查对象是否位于区间中
        /// <summary>
        /// 检查一个对象是否位于区间中
        /// </summary>
        /// <param name="obj">待检查的对象</param>
        /// <returns>一个枚举，它指示对象在区间中的位置</returns>
        IntervalPosition CheckInInterval(Obj obj)
        {
            if (Max is { } && Max.CompareTo(obj) < 0)
                return IntervalPosition.Overflow;
            return Min is { } && Min.CompareTo(obj) > 0 ? IntervalPosition.Insufficient : IntervalPosition.Located;
        }
        #endregion
    }

    #region 指示区间位置的枚举
    /// <summary>
    /// 这个枚举指示一个对象在区间中的位置
    /// </summary>
    public enum IntervalPosition
    {
        /// <summary>
        /// 代表该对象大于区间的最大值
        /// </summary>
        Overflow,
        /// <summary>
        /// 代表该对象小于区间的最小值
        /// </summary>
        Insufficient,
        /// <summary>
        /// 代表该对象正好位于区间中
        /// </summary>
        Located
    }
    #endregion
}
