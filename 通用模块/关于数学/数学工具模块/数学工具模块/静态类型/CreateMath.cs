using System.MathFrancis.Plane;

namespace System.MathFrancis;

/// <summary>
/// 这个静态类型可以用来帮助创建数学对象
/// </summary>
public static class CreateMath
{
    #region 创建坐标
    #region 创建IPoint
    #region 指定水平位置和垂直位置
    /// <summary>
    /// 使用指定的水平位置和垂直位置创建坐标
    /// </summary>
    /// <param name="right">指定的水平位置，这个值越大代表坐标越靠近右边</param>
    /// <param name="top">指定的垂直位置，这个值越大代表坐标越靠近顶部</param>
    public static IPoint Point(Num right, Num top)
        => new Point(right, top);
    #endregion
    #region 指定元组
    /// <summary>
    /// 使用指定的位置创建坐标
    /// </summary>
    /// <param name="pos">这个元组封装了坐标的水平位置和垂直位置</param>
    public static IPoint Point((Num Right, Num Top) pos)
        => new Point(pos.Right, pos.Top);
    #endregion
    #endregion
    #region 创建IVector
    #region 指定长度和方向
    /// <summary>
    /// 用指定的长度和方向创建向量
    /// </summary>
    /// <param name="length">向量的长度</param>
    /// <param name="direction">向量的方向</param>
    public static IVector Vector(Num length, IUnit<IUTAngle> direction)
        => new Vector(length, direction);
    #endregion
    #region 指定开始和结束位置
    /// <summary>
    /// 使用指定的开始位置和结束位置创建向量
    /// </summary>
    /// <param name="begin">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <returns></returns>
    public static IVector Vector(IPoint begin, IPoint end)
        => end.ToPC(begin);
    #endregion
    #endregion
    #endregion
    #region 创建平面
    #region 创建ISize
    #region 传入宽和高
    /// <summary>
    /// 用指定的宽和高创建<see cref="ISize"/>
    /// </summary>
    /// <param name="width">指定的宽</param>
    /// <param name="height">指定的高</param>
    public static ISize Size(Num width, Num height)
        => new SizeRealize(width, height);
    #endregion
    #region 传入元组
    /// <summary>
    /// 使用指定的宽和高创建<see cref="ISize"/>
    /// </summary>
    /// <param name="size">这个元组描述二维平面的宽和高</param>
    public static ISize Size((Num Width, Num Height) size)
        => new SizeRealize(size.Width, size.Height);
    #endregion
    #endregion
    #region 创建ISizePos
    #region 传入位置和大小
    /// <summary>
    /// 用指定的位置和大小创建<see cref="ISizePos"/>
    /// </summary>
    /// <param name="pos">指定的位置</param>
    /// <param name="size">指定的大小</param>
    public static ISizePos SizePos(IPoint pos, ISize size)
        => SizePos(pos, size.Size.Width, size.Size.Height);
    #endregion
    #region 传入开始位置和结束位置
    /// <summary>
    /// 使用指定的开始位置和结束位置创建<see cref="ISizePos"/>，
    /// 本方法不要求<paramref name="begin"/>在<paramref name="end"/>的左上角
    /// </summary>
    /// <param name="begin">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <returns></returns>
    public static ISizePos SizePos(IPoint begin, IPoint end)
    {
        var (lx, ly) = begin;
        var (rx, ry) = end;
        if (lx > rx)
            (lx, rx) = (rx, lx);
        if (ly < ry)
            (ly, ry) = (ry, ly);
        return SizePos(Point(lx, ly), rx - lx, ly - ry);
    }
    #endregion
    #region 传入位置，宽度和高度
    /// <summary>
    /// 用指定的位置，宽度和高度创建<see cref="ISizePos"/>
    /// </summary>
    /// <param name="pos">平面的位置</param>
    /// <param name="width">平面的宽度</param>
    /// <param name="height">平面的高度</param>
    public static ISizePos SizePos(IPoint pos, Num width, Num height)
        => new SizePos(pos, width, height);
    #endregion
    #region 传入垂直位置，水平位置，宽度和高度
    /// <summary>
    /// 用指定的垂直位置，水平位置，宽度和高度创建<see cref="ISizePos"/>
    /// </summary>
    /// <param name="top">指定的垂直位置</param>
    /// <param name="right">指定的水平位置</param>
    /// <param name="width">指定的宽度</param>
    /// <param name="height">指定的宽度</param>
    public static ISizePos SizePos(Num top, Num right, Num width, Num height)
        => SizePos(new Point(right, top), width, height);
    #endregion
    #endregion
    #endregion
    #region 创建像素平面
    #region 创建ISizePixel
    /// <summary>
    /// 指定水平和垂直方向的像素数量，
    /// 然后创建一个<see cref="ISizePixel"/>
    /// </summary>
    /// <param name="horizontal">水平方向的像素数量</param>
    /// <param name="vertical">垂直方向的像素数量</param>
    /// <returns></returns>
    public static ISizePixel SizePixel(int horizontal, int vertical)
        => new SizePixel(horizontal, vertical);
    #endregion
    #region 创建ISizePosPixel
    #region 传入位置和大小
    /// <summary>
    /// 指定位置和大小，然后创建一个<see cref="ISizePosPixel"/>
    /// </summary>
    /// <param name="firstPixel">像素平面左上角像素的位置</param>
    /// <param name="size">像素平面的大小</param>
    /// <returns></returns>
    public static ISizePosPixel SizePosPixel(IPoint firstPixel, ISizePixel size)
    {
        var (h, v) = size;
        return new SizePosPixel(firstPixel, h, v);
    }
    #endregion
    #region 传入开始位置和结束位置
    /// <summary>
    /// 使用指定的开始位置和结束位置创建<see cref="ISizePosPixel"/>，
    /// 本方法不要求<paramref name="begin"/>在<paramref name="end"/>的左上方
    /// </summary>
    /// <param name="begin">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <returns></returns>
    public static ISizePosPixel SizePosPixel(IPoint begin, IPoint end)
    {
        var (width, height, pos) = SizePos(begin, end);
        return SizePosPixel(pos, width + 1, height + 1);
    }
    #endregion
    #region 传入位置和像素数量
    /// <summary>
    /// 指定位置，以及水平和垂直方向的像素数量，
    /// 然后创建一个<see cref="ISizePosPixel"/>
    /// </summary>
    /// <param name="firstPixel">像素平面左上角的像素所处于的位置</param>
    /// <param name="horizontal">水平方向的像素数量</param>
    /// <param name="vertical">垂直方向的像素数量</param>
    /// <returns></returns>
    public static ISizePosPixel SizePosPixel(IPoint firstPixel, int horizontal, int vertical)
        => new SizePosPixel(firstPixel, horizontal, vertical);
    #endregion
    #region 传入水平和垂直的位置和大小
    /// <summary>
    /// 指定水平，垂直方向的位置和大小，
    /// 然后创建一个<see cref="ISizePosPixel"/>
    /// </summary>
    /// <inheritdoc cref="Point(Num, Num)"/>
    /// <inheritdoc cref="SizePosPixel(IPoint, int, int)"/>
    public static ISizePosPixel SizePosPixel(int right, int top, int horizontal, int vertical)
        => new SizePosPixel(Point(right, top), horizontal, vertical);
    #endregion
    #endregion
    #endregion
}
