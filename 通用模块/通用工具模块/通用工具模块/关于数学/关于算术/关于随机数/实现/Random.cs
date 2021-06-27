namespace System.Maths
{
    /// <summary>
    /// 这个类型是<see cref="IRandom"/>的实现，
    /// 可以用来生成对安全要求不严格的随机数
    /// </summary>
    class Random : IRandom
    {
        #region 封装的随机数生成器
        /// <summary>
        /// 获取封装的随机数生成器，
        /// 本类型的功能就是通过它实现的
        /// </summary>
        private System.Random PackRandom { get; }
        #endregion
        #region 生成大于0小于1的随机数
        public Num Rand()
        {
            lock (PackRandom)
            {
                return PackRandom.NextDouble();
            }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的种子初始化随机数生成器
        /// </summary>
        /// <param name="seed">用来生成随机数的种子，
        /// 如果为<see langword="null"/>，则默认使用时间作为种子</param>
        public Random(int? seed = null)
        {
            PackRandom = seed is null ? new System.Random() : new System.Random(seed.Value);
        }
        #endregion
    }
}
