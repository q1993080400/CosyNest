using System.Office.Chart;
using System.Office.Excel;
using System.Office.Word.Realize;

using Microsoft.Office.Interop.Word;

namespace System.Office.Word.Chart;

/// <summary>
/// 这个类型代表一个封装了图表的Word段落
/// </summary>
/// <typeparam name="TChart">图表的类型</typeparam>
class WordParagraphChart<TChart> : WordParagraphObjMicrosoft<TChart>
    where TChart : IOfficeChart
{
    #region 封装的图表
    /// <summary>
    /// 获取被封装的图表，
    /// 本类型的功能就是通过它实现的
    /// </summary>
    private Microsoft.Office.Interop.Word.Chart PackChart
        => PackShape.Chart;
    #endregion
    #region Word对象的内容
    public override TChart Content { get; }
    #endregion
    #region 复制Word图表
    #region 复制到工作表
    public override IExcelObj<TChart> Copy(IExcelSheet target)
    {
        var newChart = target.To<ExcelSheetMicrosoft>().PackSheet.Shapes.AddChart2();
        PackChart.Copy();
        newChart.Chart.Paste();
        newChart.Chart.ChartType = PackChart.ChartType.To<Microsoft.Office.Interop.Excel.XlChartType>();
        return (IExcelObj<TChart>)newChart.ToChart(target);
    }
    #endregion
    #region 复制到文档
    public override IWordParagraphObj<TChart> Copy(IWordDocument target, Index? pos = null)
    {
        var doc = target.To<WordDocumentMicrosoft>();
        var index = doc.ToUnderlying(doc.ToIndexActual(pos), false);
        var packDoc = doc.PackDocument;
        var newChart = packDoc.InlineShapes.AddChart2(Range: packDoc.Range(index));
        PackChart.Copy();
        newChart.Chart.Paste();
        newChart.Chart.ChartType = PackChart.ChartType;
        return (IWordParagraphObj<TChart>)newChart.ToChart(doc);
    }
    #endregion
    #endregion
    #region 构造函数与创建对象
    /// <summary>
    /// 使用指定的文档和形状初始化段落
    /// </summary>
    /// <param name="document">这个段落所在的文档</param>
    /// <param name="packShape">这个段落所封装的形状对象</param>
    /// <param name="content">Word对象所封装的图表</param>
    public WordParagraphChart(WordDocument document, InlineShape packShape, TChart content)
        : base(document, packShape)
    {
        if (packShape.HasChart is not Microsoft.Office.Core.MsoTriState.msoTrue)
            throw new Exception("此形状没有包含图表");
        this.Content = content;
    }
    #endregion
}
