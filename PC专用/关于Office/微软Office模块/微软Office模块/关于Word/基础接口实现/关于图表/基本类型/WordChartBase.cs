using System.Maths.Plane;
using System.Office.Chart;
using System.Office.Excel;

using Microsoft.Office.Interop.Word;

namespace System.Office.Word;

/// <summary>
/// 这个类型是Word图表的基类
/// </summary>
class WordChartBase : IOfficeChart
{
    #region 封装的对象
    #region 形状
    /// <summary>
    /// 获取封装的形状对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private InlineShape PackShape { get; }
    #endregion
    #region 图表
    /// <summary>
    /// 获取被封装的图表，
    /// 本类型的功能就是通过它实现的
    /// </summary>
    private Microsoft.Office.Interop.Word.Chart PackChart
        => PackShape.Chart;
    #endregion
    #endregion
    #region 图表的信息
    #region 图表的大小
    public ISize Size
    {
        get => PackShape.GetSize();
        set => PackShape.SetSize(value);
    }
    #endregion
    #region 图表的标题
    public string Title
    {
        get => PackChart.ChartTitle.Caption;
        set => PackChart.ChartTitle.Caption = value;
    }
    #endregion
    #endregion
    #region 未实现的成员
    public IEnumerable<ISeries> Seriess => throw new NotImplementedException();

    public ISeries CreateSeries()
    {
        throw new NotImplementedException();
    }

    public void SetValue(IExcelCells value)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的形状封装进图表中
    /// </summary>
    /// <param name="packShape">被封装的形状</param>
    public WordChartBase(InlineShape packShape)
    {
        this.PackShape = packShape;
    }
    #endregion
}
