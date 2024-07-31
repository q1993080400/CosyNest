using System.Office.Excel.Realize;

using Microsoft.Office.Interop.Excel;

namespace System;

/// <summary>
/// 这个静态类是有关微软COM组件实现的Excel的工具类
/// </summary>
static class ExtentExcelMicrosoft
{
    #region 关于单元格
    #region 返回单元格的开始和结束行列数
    /// <summary>
    /// 返回单元格的开始和结束行列数
    /// </summary>
    /// <param name="range">待返回行列数的单元格</param>
    /// <returns></returns>
    public static (int BR, int BC, int ER, int EC) GetAddress(this MSExcelRange range)
    {
        var br = range.Row - 1;
        var bc = range.Column - 1;
        return (br, bc,
            br + range.Rows.Count - 1,
            bc + range.Columns.Count - 1);
    }
    #endregion
    #endregion
    #region 将地址转换为A1或R1CR地址
    /// <summary>
    /// 将地址转换为A1或R1CR地址
    /// </summary>
    /// <param name="application">用来转换的Excel对象</param>
    /// <param name="address">要转换的地址</param>
    /// <param name="toR1C1">如果这个值为<see langword="true"/>，
    /// 表示要将其转换为R1C1地址，否则转换为A1地址</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static string ConvertAddress(this Application application, string address, bool toR1C1)
    {
        var isR1CR = ExcelRealizeHelp.IsR1C1Address(address) ??
            throw new NotSupportedException($"{address}不是一个合法的Excel地址");
        return application.ConvertFormula(address,
            isR1CR ? XlReferenceStyle.xlR1C1 : XlReferenceStyle.xlA1,
            toR1C1 ? XlReferenceStyle.xlR1C1 : XlReferenceStyle.xlA1);
    }
    #endregion
}
