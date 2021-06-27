namespace System
{
    /// <summary>
    /// 凡是实现这个接口的类型，都可以视为一个具体区间，
    /// 它可以直接获取区间的上下限，而不是只能通过<see cref="IComparable{T}"/>与上下限进行比较
    /// </summary>
    /// <typeparam name="Obj">位于区间中的对象</typeparam>
    public interface IIntervalSpecific<Obj> : IInterval<Obj>
         where Obj : struct, IComparable<Obj>
    {
        #region 说明文档
        /*实现本接口时，请遵循以下规范：
          #如果区间的最小值比最大值大，则交换它们*/
        #endregion
        #region 区间的下限
        /// <summary>
        /// 返回区间的下限，
        /// 如果为<see langword="null"/>，代表没有下限
        /// </summary>
        new Obj? Min
            => this.To<IInterval<Obj>>().Min.To<Obj?>();
        #endregion
        #region 区间的上限
        /// <summary>
        /// 返回区间的上限，
        /// 如果为<see langword="null"/>，代表没有上限
        /// </summary>
        new Obj? Max
             => this.To<IInterval<Obj>>().Max.To<Obj?>();
        #endregion
        #region 解构区间
        /// <summary>
        /// 将区间解构为下限和上限
        /// </summary>
        /// <param name="min">用来接收区间下限的对象</param>
        /// <param name="max">用来接收区间上限的对象</param>
        void Deconstruct(out Obj? min, out Obj? max)
        {
            min = this.Min;
            max = this.Max;
        }
        #endregion
    }
}
