namespace System.Office.Excel;

/// <summary>
/// 这个枚举指示在返回单元格的文本地址时，
/// 所遵循的模式
/// </summary>
public enum AddressTextMod
{
    /// <summary>
    /// 表示仅返回单元格地址
    /// </summary>
    Simple,
    /// <summary>
    /// 表示还应该包含工作表的名称
    /// </summary>
    WithSheetName,
    /// <summary>
    /// 表示还应该包含文件的名称
    /// </summary>
    WithFileName
}
