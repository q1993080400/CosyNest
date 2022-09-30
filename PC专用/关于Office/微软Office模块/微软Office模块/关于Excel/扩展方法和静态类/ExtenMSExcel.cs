using System.Maths;
using System.Maths.Plane;
using System.Office.Chart;
using System.Office.Excel;
using System.Office.Excel.Chart;

using Microsoft.Office.Interop.Excel;

using EXRange = Microsoft.Office.Interop.Excel.Range;

namespace System;

/// <summary>
/// 所有关于微软ExcelCOM组件的扩展方法全部放在这里
/// </summary>
static class ExtenMSExcel
{
    #region 关于工作表和工作簿
    #region 返回最后一张工作表
    /// <summary>
    /// 返回一个工作表集合的最后一张工作表
    /// </summary>
    /// <param name="sheets">待返回最后工作表的集合</param>
    /// <returns></returns>
    public static Worksheet Last(this Sheets sheets)
        => (Worksheet)sheets[sheets.Count];
    #endregion
    #endregion
    #region 关于单元格
    #region 返回单元格的开始和结束行列数
    /// <summary>
    /// 返回单元格的开始和结束行列数
    /// </summary>
    /// <param name="range">待返回行列数的单元格</param>
    /// <returns></returns>
    public static (int BR, int BC, int ER, int EC) GetAddress(this EXRange range)
    {
        var br = range.Row - 1;
        var bc = range.Column - 1;
        return (br, bc,
            br + range.Rows.Count - 1,
            bc + range.Columns.Count - 1);
    }
    #endregion
    #region 返回指定索引处的单元格
    /// <summary>
    /// 返回一个单元格内部指定索引处的单元格
    /// </summary>
    /// <param name="range">待返回子单元格的单元格</param>
    /// <param name="index">子单元格的索引</param>
    /// <returns></returns>
    public static EXRange ElementAt(this EXRange range, int index)
        => range.Cells.OfType<EXRange>().ElementAt(index);
    #endregion
    #endregion
    #region 关于形状
    #region 获取形状集合中的所有形状
    /// <summary>
    /// 获取形状集合中的所有形状
    /// </summary>
    /// <param name="shapes">待获取元素的形状集合</param>
    /// <returns></returns>
    public static IEnumerable<Shape> GetShapes(this Shapes shapes)
        => shapes.OfType<Shape>();
    #endregion
    #region 关于形状的大小
    #region 获取形状的大小
    /// <summary>
    /// 获取形状的大小
    /// </summary>
    /// <param name="shape">待获取大小的形状</param>
    /// <returns></returns>
    public static ISize GetSize(this Shape shape)
        => CreateMath.Size(shape.Width, shape.Height);
    #endregion
    #region 写入形状的大小
    /// <summary>
    /// 写入形状的大小
    /// </summary>
    /// <param name="shape">待写入大小的形状</param>
    /// <param name="size">要写入的新大小</param>
    public static void SetSize(this Shape shape, ISize size)
    {
        var (width, height) = size;
        shape.Width = width;
        shape.Height = height;
    }
    #endregion
    #endregion
    #region 对形状的判断
    #region 判断图表
    /// <summary>
    /// 如果形状对象是图表，则返回<see langword="true"/>
    /// </summary>
    /// <param name="shape">待判断的形状对象</param>
    /// <returns></returns>
    public static bool IsChart(this Shape shape)
        => shape.Type is Microsoft.Office.Core.MsoShapeType.msoChart;
    #endregion
    #region 判断图片
    /// <summary>
    /// 如果形状对象是图片，则返回<see langword="true"/>
    /// </summary>
    /// <param name="shape">待判断的形状对象</param>
    /// <returns></returns>
    public static bool IsImage(this Shape shape)
        => shape.Type is Microsoft.Office.Core.MsoShapeType.msoPicture;
    #endregion
    #endregion
    #endregion
    #region 关于图表
    #region 将形状对象转换为Excel图表
    /// <summary>
    /// 将形状对象转换为Excel图表
    /// </summary>
    /// <param name="shape">封装Excel图表的形状对象</param>
    /// <param name="sheet">Excel图表所在的工作表</param>
    /// <exception cref="ArgumentException">该形状没有包含图表</exception>
    /// <exception cref="ArgumentException">图表的类型无法识别</exception>
    /// <returns></returns>
    public static IExcelObj<IOfficeChart> ToChart(this Shape shape, IExcelSheet sheet)
        => shape.Chart.ChartType switch
        {
            var t when ExcelChartLine.Map.TryBMapA(t).Exist => new ExcelObjChart<IOfficeChartLine>(sheet, shape, new ExcelChartLine(shape, sheet)),
            var t when ExcelChartXY.Map.TryBMapA(t).Exist => new ExcelObjChart<IOfficeChartXY>(sheet, shape, new ExcelChartXY(shape, sheet)),
            _ => new ExcelObjChart<IOfficeChart>(sheet, shape, new ExcelChartBase(shape, sheet))
        };
    #endregion
    #endregion
}
