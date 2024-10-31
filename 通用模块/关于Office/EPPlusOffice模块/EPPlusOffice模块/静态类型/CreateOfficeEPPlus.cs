using System.Office.Excel;

namespace System.Office;

/// <summary>
/// 这个静态类为创建使用EPPlus实现的Office对象提供帮助
/// </summary>
public static class CreateOfficeEPPlus
{
    #region 创建Excel工作簿
    #region 创建空工作簿
    /// <summary>
    /// 创建一个空的工作簿
    /// </summary>
    /// <returns></returns>
    public static IExcelBook ExcelBook()
        => new ExcelBookEPPlus((string?)null);
    #endregion
    #region 使用路径
    /// <summary>
    /// 根据路径，创建一个<see cref="IExcelBook"/>
    /// </summary>
    /// <param name="path">Excel工作簿的路径，
    /// 如果为<see langword="null"/>，则不从文件中加载，而是创建一个新的工作簿</param>
    /// <param name="autoSave">如果这个值为<see langword="true"/>，
    /// 释放工作簿的时候，会自动保存</param>
    /// <returns></returns>
    public static IExcelBook ExcelBook(string? path, bool autoSave)
        => new ExcelBookEPPlus(path)
        {
            AutoSave = autoSave
        };
    #endregion
    #region 使用流
    /// <summary>
    /// 根据流，创建一个<see cref="IExcelBook"/>
    /// </summary>
    /// <param name="stream">Excel工作簿的流</param>
    /// <returns></returns>
    /// <inheritdoc cref="ExcelBook(string?, bool)"/>
    public static IExcelBook ExcelBook(Stream stream, bool autoSave)
        => new ExcelBookEPPlus(stream)
        {
            AutoSave = autoSave
        };
    #endregion
    #endregion
}
