namespace System;

/// <summary>
/// 关于绘图的扩展方法全部放在这里
/// </summary>
public static class ExtendDrawing
{
    #region 解构Color
    /// <summary>
    /// 将一个<see cref="Color"/>解构为RGBA
    /// </summary>
    /// <param name="color">待解构的颜色</param>
    /// <param name="r">这个对象接收颜色的R值</param>
    /// <param name="g">这个对象接收颜色的G值</param>
    /// <param name="b">这个对象接收颜色的B值</param>
    /// <param name="a">这个对象接收颜色的A值</param>
    public static void Deconstruct(this Drawing.Color color, out byte r, out byte g, out byte b, out byte a)
    {
        r = color.R;
        g = color.G;
        b = color.B;
        a = color.A;
    }
    #endregion
}
