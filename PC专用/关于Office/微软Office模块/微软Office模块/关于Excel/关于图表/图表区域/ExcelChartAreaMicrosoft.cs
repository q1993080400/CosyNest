using System.Media.Drawing;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace System.Office;

/// <summary>
/// 这个类型表示一个底层由微软COM组件实现的Excel图表图表区域
/// </summary>
/// <param name="chartArea">图表区格式</param>
sealed class ExcelChartAreaMicrosoft(ChartArea chartArea) : IOfficeChartArea
{
    #region 图表区填充
    public IColor? Fill
    {
        get => throw new NotImplementedException();
        set
        {
            if (value is not null)
                throw new NotSupportedException("本属性暂时只允许写入null，即无填充");
            chartArea.Fill.Visible = MsoTriState.msoFalse;
        }
    }
    #endregion 
}
