using System.MathFrancis;

namespace System.Media.Drawing.Text;

/// <summary>
/// 这个类型是<see cref="ITextStyleVar"/>的实现，
/// 可以作为一个文本样式
/// </summary>
/// <remarks>
/// 用指定的参数初始化文本样式
/// </remarks>
/// <param name="fontName">指定的字体</param>
/// <param name="size">字体的大小</param>
/// <param name="color">文本的颜色</param>
sealed class TextStyleVar(string fontName, IUnit<IUTFontSize> size, IColor color) : ITextStyleVar
{
    #region 字体名称
    public string FontName { get; set; } = fontName;
    #endregion
    #region 字体的大小
    public IUnit<IUTFontSize> Size { get; set; } = size;
    #endregion
    #region 文本颜色
    public IColor TextColor { get; set; } = color;
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => $"字体：{FontName}  字号：{Size}";

    #endregion
}
