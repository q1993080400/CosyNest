using System.Collections;

using Microsoft.Office.Interop.Excel;

namespace System.Office;

/// <summary>
/// 这个类型是底层使用微软COM组件实现的Excel图表管理对象
/// </summary>
/// <param name="shapes">封装的形状集合</param>
sealed class ExcelChartManageMicrosoft(Shapes shapes) : IOfficeObjectManageCommon<IOfficeChart>
{
    #region 公开成员
    #region 图表数量
    public int Count
        => Charts.Count();
    #endregion
    #region 枚举所有图表
    public IEnumerator<IOfficeChart> GetEnumerator()
        => Charts.Select(static x => new ExcelChartMicrosoft(x)).
        GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #endregion
    #region 内部成员
    #region 枚举所有图表
    /// <summary>
    /// 枚举所有图表
    /// </summary>
    private IEnumerable<Shape> Charts
        => shapes.OfType<Shape>().
        Where(x => x.HasChart is Microsoft.Office.Core.MsoTriState.msoTrue);
    #endregion
    #endregion
    #region 未实现的成员
    public IOfficeChart Add()
    {
        throw new NotImplementedException();
    }
    #endregion 
}
