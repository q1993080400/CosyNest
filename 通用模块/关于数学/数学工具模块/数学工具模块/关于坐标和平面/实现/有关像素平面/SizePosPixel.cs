namespace System.Maths.Plane;

/// <summary>
/// 这个类型是<see cref="ISizePosPixel"/>的实现，
/// 可以视为一个具有位置的像素平面
/// </summary>
sealed record SizePosPixel : SizePixel, ISizePosPixel
{
    #region 指定第一个像素的位置
    public IPoint FirstPixel { get; }
    #endregion
    #region 返回全部四个顶点
    private IReadOnlyList<IPoint>? VertexField;

    public IReadOnlyList<IPoint> Vertex
    {
        get
        {
            if (VertexField is { })
                return VertexField;
            var (h, v) = this.To<ISizePosPixel>();
            h = ToolArithmetic.Limit(true, h - 1, 0);
            v = -ToolArithmetic.Limit(true, v - 1, 0);
            return VertexField = new[]
            {
                    FirstPixel,
                    FirstPixel.Move(h,0),
                    FirstPixel.Move(h,v),
                    FirstPixel.Move(0,v)
                };
        }
    }
    #endregion
    #region IEquatable的实现
    public bool Equals(ISizePosPixel? other)
    {
        if (other is null)
            return false;
        var (topLeft, bottomRight) = this.To<ISizePosPixel>().Boundaries;
        var (topLeft1, bottomRight1) = other.Boundaries;
        return topLeft.Equals(topLeft1) && bottomRight.Equals(bottomRight1);
    }
    #endregion
    #region 重写ToString
    public override string ToString()
    {
        var (w, h) = (ISizePosPixel)this;
        return $"宽：{w}，高：{h}，位置：{FirstPixel}";
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="firstPixel">左上角第一个像素的位置</param>
    /// <param name="horizontal">水平方向像素的数量</param>
    /// <param name="vertical">垂直方向像素的数量</param>
    public SizePosPixel(IPoint firstPixel, int horizontal, int vertical)
        : base(horizontal, vertical)
    {
        var (r, t) = firstPixel;
        this.FirstPixel = CreateMath.Point(r.Rounding(), t.Rounding());
    }
    #endregion
}
