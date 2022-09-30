namespace System.Maths.Plane;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来描述一个二维平面的范围
/// </summary>
public interface ISize
{
    #region 返回平面的宽度和高度
    /// <summary>
    /// 返回一个元组，它的项分别指示平面的宽度和高度
    /// </summary>
    (Num Width, Num Height) Size { get; }

    /*实现本API请遵循以下规范：
      #Width和Height不允许负值，
      如果在构造时传入负值，请抛出异常*/
    #endregion
    #region 返回平面是横向，正向，还是正方形
    /// <summary>
    /// 如果平面是横向，返回<see langword="true"/>，
    /// 是竖向，返回<see langword="false"/>，
    /// 是正方形，返回<see langword="null"/>
    /// </summary>
    bool? IsIsHorizontal
        => Size switch
        {
            (var w, var h) when w > h => true,
            (var w, var h) when w < h => false,
            _ => null
        };
    #endregion
    #region 扩展平面
    /// <summary>
    /// 将平面进行扩展，并返回扩展后的平面
    /// </summary>
    /// <param name="extendedWidth">扩展的宽度，加上原有的宽度以后不能为负值</param>
    /// <param name="extendedHeight">扩展的高度，加上原有的高度以后不能为负值</param>
    /// <returns></returns>
    ISize Extend(Num extendedWidth, Num extendedHeight)
    {
        var (w, h) = this;
        return CreateMath.Size(w + extendedWidth, h + extendedHeight);
    }
    #endregion
    #region 转换为像素平面
    /// <summary>
    /// 将平面转换为像素平面
    /// </summary>
    /// <param name="pixelSize">指定单个像素的大小</param>
    /// <param name="rounding">当这个平面的宽高不能整除像素的大小时，
    /// 如果这个值为<see langword="true"/>，代表将多余的部分抛弃，
    /// 否则代表将多余的部分补齐为一个像素</param>
    /// <returns></returns>
    ISizePixel ToSizePixel(ISize pixelSize, bool rounding)
    {
        var (w, h) = this;
        var (pw, ph) = pixelSize;
        return CreateMath.SizePixel(
            ToolArithmetic.Sim(w / pw, isProgressive: !rounding),
            ToolArithmetic.Sim(h / ph, isProgressive: !rounding));
    }
    #endregion
    #region 解构ISize
    /// <summary>
    /// 将这个平面大小解构为宽和高
    /// </summary>
    /// <param name="width">平面的宽</param>
    /// <param name="height">平面的高</param>
    void Deconstruct(out Num width, out Num height)
    {
        var (w, h) = Size;
        width = w;
        height = h;
    }
    #endregion
}
