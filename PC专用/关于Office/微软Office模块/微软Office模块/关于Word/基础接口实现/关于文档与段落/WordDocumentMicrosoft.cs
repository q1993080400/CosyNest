using System.IOFrancis.FileSystem;
using System.Media.Drawing;
using System.Office.Word.Chart;
using System.Office.Word.Realize;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

using Task = System.Threading.Tasks.Task;

namespace System.Office.Word;

/// <summary>
/// 这个类型代表由微软COM组件实现的Word文档
/// </summary>
class WordDocumentMicrosoft : WordDocument, IWordDocument
{
    #region 封装的对象
    #region 封装的Application
    /// <summary>
    /// 获取封装的Application对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal Application PackApp { get; }
    #endregion
    #region 封装的Word文档
    /// <summary>
    /// 返回Application对象的唯一一个文档
    /// </summary>
    internal Document PackDocument { get; }

    //根据设计，每个WordDocument对象独占一个Word文档
    #endregion
    #endregion
    #region 关于文档
    #region 释放文档
    protected override ValueTask DisposeAsyncActualRealize()
    {
#if DEBUG
        if (FromActive)
            return ValueTask.CompletedTask;
#endif
        PackApp.DisplayAlerts = WdAlertLevel.wdAlertsAll;
        PackDocument.Close();
        PackApp.Quit();
        return ValueTask.CompletedTask;
    }
    #endregion
    #region 保存文档
    protected override Task SaveRealize(string path, bool isSitu)
    {
        if (isSitu)
        {
            if (!PackDocument.Saved)
                PackDocument.Save();
        }
        else PackDocument.SaveAs(path);
        return Task.CompletedTask;
    }
    #endregion
    #region 返回页面对象
    private IPage? PageField;

    public override IPage Page
        => PageField ??= new WordPage(PackDocument);
    #endregion
    #region 枚举所有段落
    public override IEnumerable<IWordParagraph> Paragraphs
    {
        get
        {
            var paragraphs = PackDocument.Paragraphs.OfType<Paragraph>().Select(x => ((object)x, x.Range.Start));
            var shapes = PackDocument.InlineShapes.OfType<InlineShape>().Select(x => ((object)x, x.Range.Start));
            return paragraphs.Union(shapes).Sort(x => x.Start).Select(x => x.Item1 switch
            {
                Paragraph p => new WordParagraphTextMicrosoft(this, p),
                InlineShape { HasChart: MsoTriState.msoTrue } s => s.ToChart(this),
                _ => (IWordParagraph?)null
            }).Where(x => x is { })!;
        }
    }
    #endregion
    #endregion
    #region 关于文本
    #region 返回无格式文本
    public override string Text
         => PackDocument.Content.Text;
    #endregion
    #region 插入文本
    public override IWordRange CreateWordRange(string text, Index? begin = null)
    {
        var pos = ToIndexActual(begin);
        var range = PackDocument.Range(ToUnderlying(pos, false));
        var oldText = range.Text.TrimEnd('\r');
        range.Text = oldText + text;                                       //防止自动将内容移动到下一行
        if (range.Text.Trim().Length != text.Trim().Length)               //如果Text就是文档的全部内容，则不移动开始位置，至于为什么要移动，我已经有点忘了
            range.Start++;
        CallLengthChange(pos, newText: text);
        return new WordRangeMicrosoft(this, range, false);
    }
    #endregion
    #region 插入段落
    public override IWordParagraphText CreateParagraph(string text, Index? begin = null)
    {
        var pos = ToIndexActual(begin) + 1;     //由于Paragraphs.Add()方法是将段落添加到范围前面
        Paragraph paragraph;                    //但根据规范，新文本应该出现在段落后面，所以位置应该加1
        if (pos == Length)
        {
            paragraph = PackDocument.Paragraphs.Add();
        }
        else
        {
            var range = PackDocument.Range(ToUnderlying(pos, false));
            paragraph = PackDocument.Paragraphs.Add(range);
        }
        paragraph.Range.Text += text;
        CallLengthChange(pos..(pos + 1), text.Length + 1);      //由于新段落的文本自带一个换行符，所以实际增加的文本应加1
        return new WordParagraphTextMicrosoft(this, paragraph);
    }
    #endregion
    #region 获取Word范围
    public override IWordRange this[Range range]
    {
        get
        {
            var (b, e) = range.GetOffsetAndEnd(Length);
            return new WordRangeMicrosoft(this,
                PackDocument.Range(ToUnderlying(b, true), ToUnderlying(e, false)), true);
        }
    }
    #endregion
    #endregion
    #region 关于位置
    #region 将索引计算为实际位置
    /// <summary>
    /// 将索引计算为实际位置
    /// </summary>
    /// <param name="index">待计算的索引，
    /// 如果为<see langword="null"/>，默认为^1</param>
    /// <returns></returns>
    public int ToIndexActual(Index? index)
        => (index is null || index.Value.Equals(^0) ? ^1 : index.Value).GetOffset(Length);
    #endregion
    #region 获取文档的长度
    public override int Length
        => Text.Length;

    /*说明文档：
      如果文档中存在InlineShape对象，
      则Word会在文本中该对象的位置加上"\"作为占位符，
      因此文本的字数和文档的长度是相同的
      PS：这个设计非常怪异*/
    #endregion
    #region 枚举非文本段落的索引
    protected override IEnumerable<WordPos> NotTextIndex
    {
        get
        {
            var arry = PackDocument.InlineShapes.OfType<object>().
                Union(PackDocument.Hyperlinks.OfType<object>()).
                Select(x => x switch
                {
                    InlineShape s => ((object)s, s.Range.Start, s.Range.End),
                    Hyperlink h => (h, h.Range.Start, h.Range.End),
                    _ => default
                }).Sort(x => x.End);
            return arry.AggregateSelect(new WordPos(default, default, default), (range, seed) =>
              {
                  var ((_, textBegin), (_, actualBegin), (_, lastUnderlyingEnd)) = seed;
                  var (range2, underlyingBegin, underlyingEnd) = range;
                  var poor = underlyingBegin - lastUnderlyingEnd;
                  textBegin += poor;
                  actualBegin += poor;
                  var (textEnd, actualEnd) = range2 switch
                  {
                      InlineShape i => (textBegin, actualBegin + 1),
                      Hyperlink h => (textBegin + h.Range.Text.Length, actualBegin + h.Range.Text.Length),
                      _ => default
                  };
                  var @return = new WordPos((textBegin, textEnd), (actualBegin, actualEnd), (underlyingBegin, underlyingEnd));
                  return (@return, @return);
              });
        }
    }
    #endregion
    #region 替换文本
    public override void Replace(string old, string @new)
    {
        var range = PackDocument.Range();
        var find = range.Find;
        range.Select();
        if (PackApp.Version.To<double>() >= 12)
            find.Execute2007(FindText: old, ReplaceWith: @new, Replace: WdReplace.wdReplaceAll);
        else find.Execute(FindText: old, ReplaceWith: @new, Replace: WdReplace.wdReplaceAll);
    }
    #endregion
    #endregion
    #region 关于Office对象
    #region 关于图表
    #region 返回图表创建器
    private ICreateWordChart? CreateChartField;

    public override ICreateWordChart CreateChart
        => CreateChartField ??= new CreateChartWordMicrosoft(this);
    #endregion
    #endregion
    #region 关于图片
    #region 创建图片
    public override IWordParagraphObj<IImage> CreateImage(IImage image, Index? pos = null)
    {
        var range = PackDocument.Range(ToUnderlying(ToIndexActual(pos), false));
        var path = MSOfficeRealize.SaveImage(image);
        var shape = PackDocument.InlineShapes.AddPicture(path.Path, Range: range);
        return new WordParagraphImageMicrosoft(this, shape);
    }
    #endregion
    #endregion
    #endregion
    #region 构造函数
    #region 供调试使用的成员
#if DEBUG
    #region 指示Word对象的创建方式
    /// <summary>
    /// 如果这个值为真，
    /// 代表这个对象是从已经打开的Word窗口中加载的，
    /// 否则代表它是通过程序创建的
    /// </summary>
    private bool FromActive { get; }
    #endregion
    #region 构造函数：指定文档
    /// <summary>
    /// 使用指定的文档初始化对象
    /// </summary>
    /// <param name="document">指定的文档</param>
    public WordDocumentMicrosoft(Document document)
        : base(document.FullName, CreateOfficeMS.SupportWord)
    {
        PackDocument = document;
        PackApp = PackDocument.Application;
        PackApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
        FromActive = true;
    }
    #endregion
#endif
    #endregion
    #region 指定路径
    /// <inheritdoc cref="WordDocument(PathText?, IFileType)"/>
    public WordDocumentMicrosoft(PathText? path = null)
        : base(path, CreateOfficeMS.SupportWord)
    {
        PackApp = new Application()
        {
            Visible = false,
            DisplayAlerts = WdAlertLevel.wdAlertsNone,
            FileValidation = MsoFileValidationMode.msoFileValidationSkip
        };
        var docs = PackApp.Documents;
        PackDocument = path is null || !File.Exists(path) ?
            docs.Add() : docs.Open(path.Path);
    }
    #endregion
    #endregion
}
