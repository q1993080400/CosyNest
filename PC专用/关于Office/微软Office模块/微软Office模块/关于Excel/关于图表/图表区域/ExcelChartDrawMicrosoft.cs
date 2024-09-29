using System.Drawing;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace System.Office;

/// <summary>
/// 这个类型表示一个底层由微软COM组件实现的Excel图表绘图区域
/// </summary>
/// <param name="plotArea">绘图区格式</param>
sealed class ExcelChartDrawMicrosoft(PlotArea plotArea) : IOfficeChartDraw
{
    #region 绘图区填充
    public Color? Fill
    {
        get => throw new NotImplementedException();
        set
        {
            if (value is not null)
                throw new NotSupportedException("本属性暂时只允许写入null，即无填充");
            plotArea.Fill.Visible = MsoTriState.msoFalse;
        }
    }
    #endregion 
}
