using System.IOFrancis.FileSystem;

namespace System.Office;

/// <summary>
/// 这个静态类可以用来帮助实现Office对象
/// </summary>
static class MicrosoftOfficeRealize
{
    #region 判断打印文件时应该使用哪个打印机
    /// <summary>
    /// 当执行打印到文件的功能时，
    /// 根据打印的目标路径，判断应该使用哪个打印机名称
    /// </summary>
    /// <param name="filePath">打印的目标路径</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string GetPrinterName(string filePath)
        => ToolPath.SplitFilePath(filePath).Extended switch
        {
            "pdf" => "Microsoft Print to PDF",
            "xps" or "oxps" => "Microsoft XPS Document Writer",
            var e => throw new ArgumentException($"由于无法识别扩展名为{e}的路径，程序不知道应该使用哪个打印机进行打印")
        };
    #endregion
}
