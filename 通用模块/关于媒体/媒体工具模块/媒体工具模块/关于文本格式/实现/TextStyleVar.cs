using System.Maths;

namespace System.Media.Drawing.Text;

/// <summary>
/// 这个类型是<see cref="ITextStyleVar"/>的实现，
/// 可以作为一个文本样式
/// </summary>
sealed class TextStyleVar : ITextStyleVar
{
    #region 字体名称
    public string FontName { get; set; }
    #endregion
    #region 字体的大小
    public IUnit<IUTFontSize> Size { get; set; }
    #endregion
    #region 文本颜色
    public IColor TextColor { get; set; }
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => $"字体：{FontName}  字号：{Size}";
    #endregion
    #region 构造函数
    /// <summary>
    /// 用指定的参数初始化文本样式
    /// </summary>
    /// <param name="fontName">指定的字体</param>
    /// <param name="size">字体的大小</param>
    /// <param name="color">文本的颜色</param>
    public TextStyleVar(string fontName, IUnit<IUTFontSize> size, IColor color)
    {
        FontName = fontName;
        Size = size;
        TextColor = color;
    }
    #endregion
}
