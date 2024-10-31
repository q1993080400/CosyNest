using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 这个静态类型可以用来帮助创建数学对象
/// </summary>
public static class CreateMath
{
    #region 创建IPoint
    /// <summary>
    /// 使用指定的水平位置和垂直位置创建坐标
    /// </summary>
    /// <typeparam name="Num">用来描述坐标的数字类型</typeparam>
    /// <param name="right">指定的水平位置，这个值越大代表坐标越靠近右边</param>
    /// <param name="top">指定的垂直位置，这个值越大代表坐标越靠近顶部</param>
    public static IPoint<Num> Point<Num>(Num right, Num top)
        where Num : INumber<Num>
        => new Point<Num>
        {
            Right = right,
            Top = top,
        };
    #endregion
    #region 创建平面
    #region 创建ISize
    #region 传入宽和高
    /// <summary>
    /// 用指定的宽和高创建<see cref="ISize{Num}"/>
    /// </summary>
    /// <typeparam name="Num">平面数字的类型</typeparam>
    /// <param name="width">指定的宽</param>
    /// <param name="height">指定的高</param>
    public static ISize<Num> Size<Num>(Num width, Num height)
        where Num : INumber<Num>
        => new Size<Num>()
        {
            Width = width,
            Height = height
        };
    #endregion
    #endregion
    #region 创建ISizePos
    #region 仅限整数
    #region 传入位置和大小
    /// <summary>
    /// 用指定的位置和大小创建<see cref="ISizePos{Num}"/>
    /// </summary>
    /// <typeparam name="Num">用来描述平面的数字类型</typeparam>
    /// <param name="pos">平面左上角的坐标</param>
    /// <param name="size">平面的大小</param>
    /// <returns></returns>
    public static ISizePos<Num> SizePosInteger<Num>(IPoint<Num> pos, ISize<Num> size)
        where Num : IBinaryInteger<Num>
        => new SizePosInteger<Num>()
        {
            Position = pos,
            Size = size
        };
    #endregion
    #region 传入开始位置和结束位置
    /// <summary>
    /// 使用指定的开始位置和结束位置创建<see cref="ISizePos{Num}"/>，
    /// 本方法不要求<paramref name="begin"/>在<paramref name="end"/>的左上方
    /// </summary>
    /// <param name="begin">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <returns></returns>
    /// <inheritdoc cref="SizePosInteger{Num}(IPoint{Num}, ISize{Num})"/>
    public static ISizePos<Num> SizePosInteger<Num>(IPoint<Num> begin, IPoint<Num> end)
        where Num : IBinaryInteger<Num>
    {
        var (br, bt) = begin;
        var (er, et) = end;
        if (er < br)
            (br, er) = (er, br);
        if (bt < et)
            (bt, et) = (et, bt);
        var point = Point(br, bt);
        var size = Size(er - br + Num.One, bt - et + Num.One);
        return SizePosInteger(point, size);
    }
    #endregion
    #region 传入位置，高度，宽度
    /// <summary>
    /// 指定位置，宽度和高度，
    /// 然后创建一个<see cref="ISizePos{Num}"/>
    /// </summary>
    /// <param name="width">平面的宽度</param>
    /// <param name="height">平面的高度</param>
    /// <returns></returns>
    /// <inheritdoc cref="SizePosInteger{Num}(IPoint{Num}, ISize{Num})"/>
    public static ISizePos<Num> SizePosInteger<Num>(IPoint<Num> pos, Num width, Num height)
        where Num : IBinaryInteger<Num>
        => SizePosInteger(pos, Size(width - Num.One, height - Num.One));
    #endregion
    #region 传入X坐标，Y坐标，高度，宽度
    /// <summary>
    /// 指定X坐标，Y坐标，宽度和高度，
    /// 然后创建一个<see cref="ISizePos{Num}"/>
    /// </summary>
    /// <param name="right">平面左上角的X坐标</param>
    /// <param name="top">平面左上角的Y坐标</param>
    /// <returns></returns>
    /// <inheritdoc cref="SizePosInteger{Num}(IPoint{Num}, Num, Num)"/>
    public static ISizePos<Num> SizePosInteger<Num>(Num right, Num top, Num width, Num height)
        where Num : IBinaryInteger<Num>
        => SizePosInteger(Point(right, top), width, height);
    #endregion
    #endregion
    #endregion
    #region 仅限浮点数
    #region 传入位置和大小
    /// <inheritdoc cref="SizePosInteger{Num}(IPoint{Num}, ISize{Num})"/>
    public static ISizePos<Num> SizePosFloatingPoint<Num>(IPoint<Num> pos, ISize<Num> size)
        where Num : IFloatingPoint<Num>
        => new SizePosFloatingPoint<Num>()
        {
            Position = pos,
            Size = size
        };
    #endregion
    #endregion
    #endregion
}
