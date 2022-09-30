using System.Office.Chart;
using System.Office.Word;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型代表了封装图表的Excel对象
/// </summary>
/// <typeparam name="ChartType">图表的类型</typeparam>
class ExcelObjChart<ChartType> : ExcelObj<ChartType>
    where ChartType : IOfficeChart
{
    #region Excel对象的内容
    public override ChartType Content { get; }
    #endregion
    #region 复制Excel对象
    #region 复制到工作表
    public override IExcelObj<ChartType> Copy(IExcelSheet target)
    {
        PackShape.Copy();
        var newChart = target.To<ExcelSheetMicrosoft>().PackSheet.Shapes.AddChart2();
        newChart.Chart.Paste();
        newChart.Chart.ChartType = PackShape.Chart.ChartType;
        return (IExcelObj<ChartType>)newChart.ToChart(Sheet);
    }
    #endregion
    #region 复制到Word文档
    public override IWordParagraphObj<ChartType> Copy(IWordDocument target, Index? pos = null)
    {
        var doc = target.To<WordDocumentMicrosoft>();
        var index = doc.ToUnderlying(doc.ToIndexActual(pos), false);
        var packDoc = doc.PackDocument;
        var newChart = packDoc.InlineShapes.AddChart2(Range: packDoc.Range(index));
        PackShape.Copy();
        newChart.Chart.Paste();
        newChart.Chart.ChartType = PackShape.Chart.ChartType.To<Microsoft.Office.Core.XlChartType>();
        return (IWordParagraphObj<ChartType>)newChart.ToChart(doc);
    }
    #endregion
    #endregion
    #region 关于构造函数
    /// <inheritdoc cref="ExcelObj{Obj}.ExcelObj(IExcelSheet, Shape)"/>
    /// <param name="content">被封装的Office图表</param>
    public ExcelObjChart(IExcelSheet sheet, Shape packShape, ChartType content)
        : base(sheet, packShape)
    {
        if (!packShape.IsChart())
            throw new ArgumentException("指定的形状不包含图表");
        this.Content = content;
    }
    #endregion
}
