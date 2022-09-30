using System.DataFrancis;
using System.DataFrancis.Office.Block;
using System.Drawing;
using System.Office.Excel;

namespace System;

/// <summary>
/// 这个静态类可以用来创建将数据推送到数据源的对象
/// </summary>
public static class CreatePipeTo
{
    #region 返回推送到Excel数据管道
    /// <summary>
    /// 创建一个将数据推送到Excel的管道
    /// </summary>
    /// <param name="sheet">数据所在的表</param>
    /// <returns></returns>
    public static IDataPipeTo Excel(IExcelSheet sheet)
        => CreateBlock.BlockToSimple(sheet);
    #endregion
    #region 返回推送到Excel数据管道，对90后学生标记红色
    public static IDataPipeTo ExcelMark(IExcelSheet sheet)
        => CreateBlock.BlockToSimple(sheet, (name, value, data, cell) =>
        {
            cell.Value = new(value);
            var birthday = data.GetValue<DateTime>(nameof(Student.Birthday));
            if (birthday >= new DateTime(1990, 1, 1) && birthday < new DateTime(2000, 1, 1))
                cell.Style.BackColor = Color.Red.ToColor();
        });
    #endregion
}
