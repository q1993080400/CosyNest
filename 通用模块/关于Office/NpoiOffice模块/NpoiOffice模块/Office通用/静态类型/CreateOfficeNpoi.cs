using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Office.Excel;

namespace System.Office;

/// <summary>
/// 这个静态类为创建通过Npoi实现的Office对象提供帮助
/// </summary>
public static class CreateOfficeNpoi
{
    #region 返回受支持的文件类型
    /// <summary>
    /// 返回受本模块支持的Excel文件类型
    /// </summary>
    public static IFileType SupportExcel { get; }
    = CreateIO.FileType("受NpoiOffice模块支持的Excel文件类型",
        OfficeFileCom.Excel2003,
        OfficeFileCom.Excel2007,
        OfficeFileCom.Excel2007Macro);
    #endregion
    #region 关于Excel
    #region 根据流
    /// <inheritdoc cref="ExcelBookNpoi(Stream, bool)"/>
    public static IExcelBook ExcelBook(Stream stream, bool isExcel2007)
        => new ExcelBookNpoi(stream, isExcel2007);
    #endregion
    #region 根据路径
    /// <inheritdoc cref="ExcelBookNpoi(PathText?)"/>
    public static IExcelBook ExcelBook(PathText? path = null)
        => new ExcelBookNpoi(path);
    #endregion
    #endregion
}
