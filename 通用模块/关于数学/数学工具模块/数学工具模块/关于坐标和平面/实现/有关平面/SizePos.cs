namespace System.Maths
{
    /// <summary>
    /// 这个类型是<see cref="ISizePos"/>的默认实现，
    /// 可以被视为一个具有位置的二维平面
    /// </summary>
    record SizePos(IPoint Position, Num Width, Num Height) : SizeRealize(Width, Height), ISizePos
    {
        #region 重写ToString
        public override string ToString()
        {
            var (w, h) = (ISize)this;
            return $"宽：{w}      高：{h}      位置：{Position}";
        }
        #endregion
    }
}
