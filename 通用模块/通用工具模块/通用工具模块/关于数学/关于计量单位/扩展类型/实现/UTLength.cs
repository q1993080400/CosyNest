namespace System.Maths
{
    /// <summary>
    /// 这个类型是<see cref="IUTLength"/>的实现，
    /// 可以视为一个长度单位
    /// </summary>
    class UTLength : UT, IUTLength
    {
        #region 返回单位的类型
        protected override Type UTType
            => typeof(IUTLength);
        #endregion
        #region 构造函数
        #region 使用常数
        /// <inheritdoc cref="UT(string, Num)"/>
        public UTLength(string name, Num size)
            : base(name, size)
        {

        }
        #endregion
        #region 使用委托
        /// <inheritdoc cref="UT(string, Func{Num, Num}, Func{Num, Num}, bool)"/>
        public UTLength(string name, Func<Num, Num> toMetricDelegate, Func<Num, Num> fromMetric, bool isStatic)
            : base(name, toMetricDelegate, fromMetric, isStatic)
        {

        }
        #endregion
        #endregion
    }
}
