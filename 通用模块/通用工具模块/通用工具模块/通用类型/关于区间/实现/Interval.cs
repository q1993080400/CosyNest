namespace System
{
    /// <summary>
    /// 表示一个区间，它具有一个上限和下限
    /// </summary>
    /// <typeparam name="Obj">位于区间中的对象，
    /// 它通过<see cref="IComparable{T}"/>进行比较</typeparam>
    class Interval<Obj> : IInterval<Obj>
    {
        #region 区间的下限
        public IComparable<Obj>? Min { get; init; }
        #endregion
        #region 区间的上限
        public IComparable<Obj>? Max { get; init; }
        #endregion
        #region 重写的ToString方法
        public override string ToString()
            => $"区间上限：{Min?.ToString() ?? "不受限制"}，区间下限：{Max?.ToString() ?? "不受限制"}";
        #endregion
    }
}
