namespace System.Maths.Plane;

/// <summary>
/// <see cref="ISize"/>的实现，
/// 可以用来描述一个二维平面的范围
/// </summary>
record SizeRealize : ISize
{
    #region 返回平面的高度和宽度
    public (Num Width, Num Height) Size { get; }
    #endregion
    #region 重写ToString
    public override string ToString()
    {
        var (w, h) = (ISize)this;
        return $"宽：{w}，高：{h}";
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 用指定的宽和高初始化对象
    /// </summary>
    /// <param name="width">指定的宽</param>
    /// <param name="height">指定的高</param>
    public SizeRealize(Num width, Num height)
    {
        ExceptionIntervalOut.Check((Num)0, null, width, height);
        Size = (width, height);
    }
    #endregion
}
