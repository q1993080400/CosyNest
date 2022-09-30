using System.Drawing;
using System.Reflection;
using WordRange = Microsoft.Office.Interop.Word.Range;
using System.Maths;
using System.Media.Drawing.Text;
using System.Media.Drawing;

namespace System.Office.Word;

/// <summary>
/// 这个类型代表一个Word文本样式
/// </summary>
class WordTextStyle : ITextStyleVar
{
    #region 封装的文本范围
    /// <summary>
    /// 获取封装的文本范围，
    /// 本类型的功能就是通过它实现的
    /// </summary>
    private WordRange PackRange { get; }
    #endregion
    #region 关于文本样式
    #region 字体格式
    public string FontName
    {
        get => PackRange.Font.Name;
        set => PackRange.Font.Name = value;
    }
    #endregion
    #region 文本颜色
    public IColor TextColor
    {
        get => ColorTranslator.FromOle(PackRange.Font.Fill.ForeColor.RGB).ToColor();
        set => PackRange.Font.Fill.ForeColor.RGB = ColorTranslator.ToOle(value.ToColor());
    }
    #endregion
    #region 字体的大小
    public IUnit<IUTFontSize> Size
    {
        get => IUTFontSize.Pounds(PackRange.Font.Size);
        set => PackRange.Font.Size = value.Convert(IUTFontSize.PoundsMetric);
    }
    #endregion
    #endregion
    #region 重写的方法
    #region 辅助成员
    /// <summary>
    /// 缓存<see cref="ITextStyleVar"/>所有公开的实例属性，
    /// 为重写某些方法提供帮助
    /// </summary>
    private static IEnumerable<PropertyInfo> CacheStylePro { get; }
    = typeof(ITextStyleVar).GetTypeData().Propertys.
        Where(x => !x.IsStatic() && x.IsPublic()).ToArray();
    #endregion
    #region 重写GetHashCode
    public override int GetHashCode()
        => CacheStylePro.Select(x => x.GetValue(this)).
            FirstOrDefault(x => x is { })?.GetHashCode() ?? 0;
    #endregion
    #region 重写Equals
    public override bool Equals(object? obj)
        => obj is ITextStyleVar t &&
        CacheStylePro.All(pro => Equals(pro.GetValue(this), pro.GetValue(t)));
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => $"字体：{FontName}  字号：{Size}";
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的文本范围封装进对象
    /// </summary>
    /// <param name="packRange">被封装的文本范围</param>
    public WordTextStyle(WordRange packRange)
    {
        this.PackRange = packRange;
    }
    #endregion
}
