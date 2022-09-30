namespace System.Media.Drawing;

/// <summary>
/// 这个类型是<see cref="IColor"/>的实现，可以用来表示颜色
/// </summary>
sealed record Color : IColor
{
    #region 公开成员
    #region 红色
    public byte R { get; }
    #endregion
    #region 绿色
    public byte G { get; }
    #endregion
    #region 蓝色值
    public byte B { get; }
    #endregion
    #region 透明度
    public byte A { get; }
    #endregion
    #region 色系
    private ColourSystem? ColourSystemFiled;

    public ColourSystem ColourSystem
        => ColourSystemFiled ??= (R, G, B) switch
        {
            (var r, var g, var b) when Math.Abs(r - g) <= 30 && Math.Abs(g - b) <= 30 => ColourSystem.None,
            (var r, var g, var b) when r >= g && r >= b => ColourSystem.R,
            (var r, var g, var b) when g >= r && g >= b => ColourSystem.G,
            (var r, var g, var b) when b >= r && b >= g => ColourSystem.B,
            _ => throw new ArgumentException("无法识别的颜色模式")
        };
    #endregion
    #region 格式化文本
    private string? FormatTextField;

    public string FormatText
        => FormatTextField ??= Convert.ToHexString(new[] { R, G, B, A });
    #endregion
    #region 重写的方法
    #region 重写ToString
    public override string ToString()
        => $"R:{R} G:{G} B:{B} A:{A}";
    #endregion
    #region 重写GetHashCode
    public override int GetHashCode()
        => ToolEqual.CreateHash(R, G, B, A);
    #endregion
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的红色，绿色，蓝色和透明度初始化对象
    /// </summary>
    /// <param name="r">指定的红色值</param>
    /// <param name="g">指定的绿色值</param>
    /// <param name="b">指定的蓝色值</param>
    /// <param name="a">指定的透明度</param>
    public Color(byte r, byte g, byte b, byte a = 255)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }
    #endregion
}
