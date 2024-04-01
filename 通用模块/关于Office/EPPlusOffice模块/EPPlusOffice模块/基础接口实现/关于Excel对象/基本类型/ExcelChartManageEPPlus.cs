using System.Collections;

using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;

namespace System.Office.Excel;

/// <summary>
/// 底层使用EPPlus实现的Excel图表管理对象
/// </summary>
/// <param name="drawings">Excel对象管理对象</param>
sealed class ExcelChartManageEPPlus(ExcelDrawings drawings) : IOfficeChartManage
{
    #region 公开成员
    #region 枚举所有图表
    public IEnumerator<IOfficeChart> GetEnumerator()
        => drawings.OfType<ExcelChart>().Select(x => new ExcelChartEPPlus(x)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 返回图表的数量
    public int Count
        => drawings.OfType<ExcelChart>().Count();
    #endregion
    #endregion
}
