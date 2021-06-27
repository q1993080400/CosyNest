namespace System.Maths.Geometric
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个圆形，
    /// 它可以是正圆或椭圆
    /// </summary>
    public interface ICircular : IGeometric
    {
        #region 获取圆心
        /// <summary>
        /// 获取圆心的坐标
        /// </summary>
        IPoint Center
            => Boundaries.Center;
        #endregion
        #region 获取横轴直径
        /// <summary>
        /// 获取这个圆形的横轴直径
        /// </summary>
        Num DiameterHorizontal
            => Boundaries.Size.Width;
        #endregion
        #region 获取纵轴直径
        /// <summary>
        /// 获取这个圆形的纵轴直径
        /// </summary>
        Num DiameterVertical
            => Boundaries.Size.Height;
        #endregion
        #region 获取是否为正圆
        /// <summary>
        /// 如果这个属性返回<see langword="true"/>，
        /// 则代表这个圆是正圆，否则为椭圆
        /// </summary>
        bool IsPositive
        {
            get
            {
                var (w, h) = Boundaries;
                return w == h;
            }
        }
        #endregion
    }
}
