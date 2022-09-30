using Microsoft.Office.Interop.Word;
using WordRange = Microsoft.Office.Interop.Word.Range;
using System.Maths;
using System.Office.Word.Realize;
using System.Office.Chart;
using Microsoft.Office.Core;
using System.Office.Word.Chart;
using System.Maths.Plane;

namespace System.Office.Word;

/// <summary>
/// 这个类型为实现微软Word模块提供帮助
/// </summary>
static class MicrosoftWordRealize
{
    #region 关于Range
    #region 将范围按照文本样式拆分
    /// <summary>
    /// 将范围按照文本样式拆分成若干部分，
    /// 每个部分的都具有相同的文本格式
    /// </summary>
    /// <param name="range">待拆分的范围</param>
    /// <returns></returns>
    public static IEnumerable<WordRange> SplitFromStyle(this WordRange range)
    {
        var end = range.End - 1;
        var chars = range.Characters.ToArray<WordRange>();
        var r = chars[0];                                   //这个变量用来保存待迭代的Range
        if (chars.Length < 3)
        {
            yield return r;
            yield break;
        }
        var style = new WordTextStyle(range);
        foreach (var item in chars[1..^1])              //跳过片段的开头和最后部分，因为开头已经遍历过，而末尾一定是不可见的换行符
        {
            var newStyle = new WordTextStyle(item);
            if (style.Equals(newStyle) && r.Hyperlinks.Count == item.Hyperlinks.Count)      //如果待迭代的Range与当前字符格式一样
                r.MoveEnd();                   //则将它扩展一个字符
            else
            {
                yield return r;      //如果格式不同，则将Range迭代
                r = item;
                style = newStyle;
            }
            if (r.End == end)                   //如果遍历到了片段的末尾，则立即进行一次迭代，并退出
            {
                yield return r;
                break;
            }
        }
    }
    #endregion
    #region 返回范围是否具有一致的格式
    /// <summary>
    /// 返回范围是否具有一致的格式，
    /// 也就是它的每一部分文本都格式相同
    /// </summary>
    /// <param name="range">待判断的范围</param>
    /// <returns></returns>
    public static bool IsConsistent(this WordRange range)
    {
        var style = new WordTextStyle(range.Characters[1]);
        return !range.SplitFromStyle().Any(x => !style.Equals(new WordTextStyle(x)));
    }
    #endregion
    #endregion
    #region 关于格式
    #region 用于映射对齐方式的字典
    /// <summary>
    /// 获取一个用于双向映射对齐方式的字典
    /// </summary>
    public static ITwoWayMap<OfficeAlignment, WdParagraphAlignment> MapAlignment { get; }
    = CreateCollection.TwoWayMap(
        (OfficeAlignment.Center, WdParagraphAlignment.wdAlignParagraphCenter),
        (OfficeAlignment.LeftOrTop, WdParagraphAlignment.wdAlignParagraphLeft),
        (OfficeAlignment.RightOrBottom, WdParagraphAlignment.wdAlignParagraphRight),
        (OfficeAlignment.Ends, WdParagraphAlignment.wdAlignParagraphJustify));
    #endregion
    #endregion
    #region 关于形状
    #region 关于形状的大小
    #region 获取形状的大小
    /// <summary>
    /// 获取形状的大小
    /// </summary>
    /// <param name="shape">待获取大小的形状</param>
    /// <returns></returns>
    public static ISize GetSize(this InlineShape shape)
        => CreateMath.Size(shape.Width, shape.Height);
    #endregion
    #region 写入形状的大小
    /// <summary>
    /// 写入形状的大小
    /// </summary>
    /// <param name="shape">待写入大小的形状</param>
    /// <param name="size">要写入的新大小</param>
    public static void SetSize(this InlineShape shape, ISize size)
    {
        var (width, height) = size;
        shape.Width = width;
        shape.Height = height;
    }
    #endregion
    #endregion
    #region 判断是否为图表
    /// <summary>
    /// 如果形状对象是图表，则返回<see langword="true"/>
    /// </summary>
    /// <param name="shape">待判断的形状对象</param>
    /// <returns></returns>
    public static bool IsChart(this InlineShape shape)
        => shape.HasChart is MsoTriState.msoTrue;
    #endregion
    #endregion
    #region 关于图表
    #region 将形状对象转换为Word图表
    /// <summary>
    /// 将形状对象转换为Word图表
    /// </summary>
    /// <param name="shape">封装Word图表的形状对象</param>
    /// <param name="document">Word图表所在的文档</param>
    /// <exception cref="ArgumentException">该形状没有包含图表</exception>
    /// <exception cref="ArgumentException">图表的类型无法识别</exception>
    /// <returns></returns>
    public static IWordParagraphObj<IOfficeChart> ToChart(this InlineShape shape, WordDocument document)
        => shape.Chart.ChartType switch
        {
            XlChartType.xlLineMarkers or XlChartType.xlLine => new WordParagraphChart<IOfficeChartLine>(document, shape, new WordChartLine(shape)),
            _ => new WordParagraphChart<IOfficeChart>(document, shape, new WordChartBase(shape))
        };
    #endregion
    #endregion
    #region 静态构造函数
    static MicrosoftWordRealize()
    {
        MapAlignment.RegisteredOne(OfficeAlignment.Ends,
            WdParagraphAlignment.wdAlignParagraphJustifyHi,
            WdParagraphAlignment.wdAlignParagraphJustifyLow,
            WdParagraphAlignment.wdAlignParagraphJustifyMed,
            WdParagraphAlignment.wdAlignParagraphThaiJustify);
    }
    #endregion
}
