using System.IO;
using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Office.Excel;

namespace System.Office
{
    /// <summary>
    /// 这个静态类为创建使用EPPlus实现的Office对象提供帮助
    /// </summary>
    public static class CreateEPPlusOffice
    {
        #region 返回受支持的文件类型
        /// <summary>
        /// 返回受本模块支持的Excel文件类型
        /// </summary>
        public static IFileType SupportExcel { get; }
        = CreateIO.FileType("受EPPlusOffice模块支持的Excel文件类型",
            OfficeFileCom.Excel2007,
            OfficeFileCom.Excel2007Macro);
        #endregion
        #region 创建Excel工作簿
        #region 使用路径
        /// <summary>
        /// 根据路径，创建一个<see cref="IExcelBook"/>
        /// </summary>
        /// <param name="path">Excel工作簿的路径，
        /// 如果为<see langword="null"/>，则不从文件中加载，而是创建一个新的工作簿</param>
        /// <returns></returns>
        public static IExcelBook ExcelBook(PathText? path = null)
            => new ExcelBookEPPlus(path);
        #endregion
        #region 使用流
        /// <summary>
        /// 根据流，创建一个<see cref="IExcelBook"/>
        /// </summary>
        /// <param name="stream">Excel工作簿的流</param>
        /// <returns></returns>
        public static IExcelBook ExcelBook(Stream stream)
            => new ExcelBookEPPlus(stream);
        #endregion
        #endregion
    }
}
