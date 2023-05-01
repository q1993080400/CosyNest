namespace System.Maths.Plane;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个具有位置的像素平面
/// </summary>
public interface ISizePosPixel : ISizePixel, IBoundary, IEquatable<ISizePosPixel>
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #本接口的所有API所返回的IPoint对象不是坐标，
      而是通过像素来表示位置，例如(1,1)表示该像素位于第2行第2列，
      而不是X和Y坐标都为1

      #基于上一条，虽然IPoint使用有理数类型Num来表示，
      但是本接口所有API所返回的IPoint对象都只能出现整数，
      这是因为像素是原子化不可分割的*/
    #endregion
    #region 第一个像素的位置
    /// <summary>
    /// 返回左上角第一个像素的位置
    /// </summary>
    IPoint FirstPixel { get; }
    #endregion
    #region 将像素平面转换为坐标平面
    /// <summary>
    /// 将像素平面转换为具有位置的坐标平面
    /// </summary>
    /// <param name="pixelSize">每个像素的大小，
    /// 如果为<see langword="null"/>，则默认为(1,1)</param>
    /// <returns></returns>
    ISizePos ToSizePos(ISize? pixelSize = null)
    {
        var (width, height) = pixelSize ??= CreateMath.Size(1, 1);
        var (r, t) = FirstPixel;
        var size = ToSize(pixelSize);
        return CreateMath.SizePos(
            CreateMath.Point(width * r, height * t), size);
    }
    #endregion
    #region 有关变换像素平面
    #region 复杂方法
    /// <summary>
    /// 偏移和扩展像素平面，并返回偏移后的新像素平面
    /// </summary>
    /// <param name="extendedHorizontal">水平方向扩展的像素数量，
    /// 在加上原有的像素数量后不能为负值</param>
    /// <param name="extendedVertical">垂直方向扩展的像素数量，
    /// 在加上原有的像素数量后不能为负值</param>
    /// <param name="offsetRight">平面左上角向右偏移的像素数量</param>
    /// <param name="offsetTop">平面左上角向上偏移的像素数量</param>
    /// <returns></returns>
    ISizePosPixel Transform(int extendedHorizontal = 0, int extendedVertical = 0, int offsetRight = 0, int offsetTop = 0)
        => CreateMath.SizePosPixel(
            FirstPixel.Move(offsetRight, offsetTop),
            Extend(extendedHorizontal, extendedVertical));
    #endregion
    #region 简单方法
    /// <summary>
    /// 将像素平面移动到新位置，并赋予一个新大小，
    /// 然后返回一个新的像素平面
    /// </summary>
    /// <param name="position">新位置，它位于新平面的左上角，
    /// 如果为<see langword="null"/>，则不改变</param>
    /// <param name="size">新大小，如果为<see langword="null"/>，则不改变</param>
    /// <returns></returns>
    ISizePosPixel Transform(IPoint? position = null, ISizePixel? size = null)
        => CreateMath.SizePosPixel(position ?? FirstPixel, size ?? this);
    #endregion
    #endregion
    #region 解构ISizePosPixel
    /// <summary>
    /// 解构本对象
    /// </summary>
    /// <param name="horizontal">用来接收水平方向像素点数量的对象</param>
    /// <param name="vertical">用来接收垂直方向像素点数量的对象</param>
    /// <param name="firstPixel">用来接收左上角第一个像素的位置的对象</param>
    void Deconstruct(out int horizontal, out int vertical, out IPoint firstPixel)
    {
        var (h, v) = this;
        horizontal = h;
        vertical = v;
        firstPixel = FirstPixel;
    }
    #endregion
}
