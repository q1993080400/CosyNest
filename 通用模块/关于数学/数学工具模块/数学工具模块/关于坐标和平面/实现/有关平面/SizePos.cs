namespace System.Maths.Plane;

/// <summary>
/// 这个类型是<see cref="ISizePos"/>的默认实现，
/// 可以被视为一个具有位置的二维平面
/// </summary>
sealed record SizePos(IPoint Position, Num Width, Num Height) : SizeRealize(Width, Height), ISizePos
{
    #region 返回全部四个顶点
    public IReadOnlyList<IPoint> Vertex
    {
        get
        {
            var (w, h) = this.To<ISizePos>();
            h = -h;
            return new[]
            {
                    Position,
                    Position.Move(w,0),
                    Position.Move(w,h),
                    Position.Move(0,h)
                };
        }
    }
    #endregion
    #region 比较两个对象
    public bool Equals(ISizePos? other)
        => other is { } && Vertex[2].Equals(other.Vertex[2]);
    #endregion
    #region 重写ToString
    public override string ToString()
    {
        var (w, h) = (ISize)this;
        return $"宽：{w}，高：{h}，位置：{Position}";
    }
    #endregion
}
