using System.Maths;
using static System.Maths.CreateBaseMath;
using Font = Microsoft.Office.Interop.Excel.Font;
using ColorTranslator = System.Drawing.ColorTranslator;
using System.Media.Drawing.Text;
using System.Media.Drawing;

namespace System.Office.Excel;

/// <summary>
/// 这个类型代表Excel单元格的文本样式
/// </summary>
class RangeTextStyleMicrosoft : ITextStyleVar
{
    #region 封装的字体对象
    /// <summary>
    /// 获取封装的字体对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Font PackFont { get; }
    #endregion
    #region 字体名称
    public string FontName
    {
        get => PackFont.Name;
        set => PackFont.Name = value;
    }
    #endregion
    #region 字体的大小
    public IUnit<IUTFontSize> Size
    {
        get => Unit(PackFont.Size, IUTFontSize.PoundsMetric);
        set => PackFont.Size = (float)value.Convert(IUTFontSize.PoundsMetric);
    }
    #endregion
    #region 文本颜色
    public IColor TextColor
    {
        get => ColorTranslator.FromOle((int)PackFont.Color).ToColor();
        set => PackFont.Color = ColorTranslator.ToOle(value.ToColor());
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 用指定的字体初始化对象
    /// </summary>
    /// <param name="font">指定的字体对象</param>
    public RangeTextStyleMicrosoft(Font font)
    {
        PackFont = font;
    }
    #endregion
}
