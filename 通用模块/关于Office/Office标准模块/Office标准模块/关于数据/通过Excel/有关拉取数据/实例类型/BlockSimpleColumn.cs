using System.Office.Excel;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 该记录封装了一个Blcok列的元数据，
/// 它可以告诉程序如何获取和填充列
/// </summary>
public sealed record BlockSimpleColumn
{
#pragma warning disable CS8618

    #region 列的名称
    /// <summary>
    /// 获取列的名称
    /// </summary>
    public string ColumnName { get; init; }
    #endregion
    #region 是否必填
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表当前为必填列，如果必填列找不到值，就会停止提取
    /// </summary>
    public bool IsRequired { get; init; } = true;
    #endregion
    #region 获取列的值
    /// <summary>
    /// 这个委托的参数是列所在的单元格，
    /// 返回值是列的值
    /// </summary>
    public Func<IExcelCells, object?> GetValue { get; init; }
        = static cell => cell.Value.Content;
    #endregion
    #region 解构列
    /// <summary>
    /// 将这个列解构
    /// </summary>
    /// <param name="columnName">列的名称</param>
    /// <param name="isRequired">如果这个值为<see langword="true"/>，
    /// 代表当前为必填列，如果必填列找不到值，就会停止提取</param>
    /// <param name="getValue">这个委托的参数是列所在的单元格，
    /// 返回值是列的值</param>
    public void Deconstruct(out string columnName, out bool isRequired, out Func<IExcelCells, object?> getValue)
    {
        columnName = this.ColumnName;
        isRequired = this.IsRequired;
        getValue = this.GetValue;
    }
    #endregion
}
