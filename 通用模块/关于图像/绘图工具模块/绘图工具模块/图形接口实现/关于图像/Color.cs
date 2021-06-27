namespace System.DrawingFrancis
{
    /// <summary>
    /// 这个类型是<see cref="IColor"/>的实现，可以用来表示颜色
    /// </summary>
    record Color : IColor
    {
        #region 红色
        public byte R { get; }
        #endregion
        #region 绿色
        public byte G { get; }
        #endregion
        #region 蓝色值
        public byte B { get; }
        #endregion
        #region 透明度
        public byte A { get; }
        #endregion
        #region 重写ToString
        public override string ToString()
            => $"R:{R} G:{G} B:{B} A:{A}";
        #endregion 
        #region 构造函数
        /// <summary>
        /// 使用指定的红色，绿色，蓝色和透明度初始化对象
        /// </summary>
        /// <param name="r">指定的红色值</param>
        /// <param name="g">指定的绿色值</param>
        /// <param name="b">指定的蓝色值</param>
        /// <param name="a">指定的透明度</param>
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
        #endregion
    }
}
