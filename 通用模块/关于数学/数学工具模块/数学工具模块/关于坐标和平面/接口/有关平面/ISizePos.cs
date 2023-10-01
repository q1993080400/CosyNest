namespace System.MathFrancis.Plane;

/// <summary>
/// 这个接口代表一个具有位置的二维平面
/// </summary>
public interface ISizePos : ISize, IBoundary, IEquatable<ISizePos>
{
    #region 二维平面的位置
    /// <summary>
    /// 返回二维平面左上角的坐标
    /// </summary>
    IPoint Position { get; }
    #endregion
    #region 转换为像素平面
    /// <summary>
    /// 将这个对象转换为带位置的像素平面
    /// </summary>
    /// <param name="pixelSize">指定单个像素的大小，
    /// 如果为<see langword="null"/>，默认长宽为1</param>
    /// <param name="rounding">当这个平面的宽高不能整除像素的大小时，
    /// 如果这个值为<see langword="true"/>，代表将多余的部分抛弃，
    /// 否则代表将多余的部分补齐为一个像素</param>
    /// <returns></returns>
    ISizePosPixel ToSizePosPixel(ISize? pixelSize = null, bool rounding = true)
    {
        pixelSize ??= CreateMath.Size(1, 1);
        var size = ToSizePixel(pixelSize, rounding);
        var (r, t) = Position;
        var (pw, ph) = pixelSize;
        var point = CreateMath.Point(
            ToolArithmetic.Sim(r * pw, isProgressive: !rounding),
            ToolArithmetic.Sim(t * ph, isProgressive: !rounding));
        return CreateMath.SizePosPixel(point, size);
    }
    #endregion
    #region 返回平面的中心
    /// <summary>
    /// 返回这个平面中心的点坐标
    /// </summary>
    IPoint Center
    {
        get
        {
            var (w, h) = Size;
            return Position.Move(w / 2, h / -2);
        }
    }
    #endregion
    #region 有关变换平面
    #region 复杂方法
    /// <summary>
    /// 偏移和扩展平面，并返回偏移后的新平面
    /// </summary>
    /// <param name="extendedWidth">扩展平面的宽度，加上原有的宽度以后不能为负值</param>
    /// <param name="extendedHeight">扩展平面的高度，加上原有的高度以后不能为负值</param>
    /// <param name="offsetRight">平面左上角向右偏移的坐标</param>
    /// <param name="offsetTop">平面左上角向上偏移的坐标</param>
    /// <returns></returns>
    ISizePos Transform(Num extendedWidth, Num extendedHeight, Num offsetRight, Num offsetTop)
        => CreateMath.SizePos(Position.Move(offsetRight, offsetTop),
            Extend(extendedWidth, extendedHeight));
    #endregion
    #region 简单方法
    /// <summary>
    /// 将平面移动到新位置，并赋予一个新大小，
    /// 然后返回一个新的平面
    /// </summary>
    /// <param name="position">新位置，它位于新平面的左上角，
    /// 如果为<see langword="null"/>，则不改变</param>
    /// <param name="size">新大小，如果为<see langword="null"/>，则不改变</param>
    /// <returns></returns>
    ISizePos Transform(IPoint? position = null, ISize? size = null)
        => CreateMath.SizePos(position ?? Position, size ?? this);
    #endregion
    #endregion
    #region 解构ISizePos
    /// <summary>
    /// 将本对象解构为宽，高和位置
    /// </summary>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <param name="pos">位置</param>
    void Deconstruct(out Num width, out Num height, out IPoint pos)
    {
        var (w, h) = Size;
        width = w;
        height = h;
        pos = Position;
    }
    #endregion
}
