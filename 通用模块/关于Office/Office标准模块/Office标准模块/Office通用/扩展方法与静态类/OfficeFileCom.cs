using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace System.Office
{
    /// <summary>
    /// 这个静态类储存了Office不同版本所支持的文件类型
    /// </summary>
    public static class OfficeFileCom
    {
        #region Word文件类型
        #region Word2003文件
        /// <summary>
        /// 获取Word2003的文件类型
        /// </summary>
        public static IFileType Word2003 { get; }
        = CreateIO.FileType("Word2003文件", "doc");
        #endregion
        #region Word2007文件
        /// <summary>
        /// 获取Word2007的文件类型
        /// </summary>
        public static IFileType Word2007 { get; }
        = CreateIO.FileType("Word2007文件", "docx");
        #endregion
        #region Word2007文件（启用宏）
        /// <summary>
        /// 获取Word2007启用宏的文件类型
        /// </summary>
        public static IFileType Word2007Macro { get; }
        = CreateIO.FileType("Word2007启用宏的文件", "docm");
        #endregion
        #endregion
        #region Excel文件类型
        #region Excel2003文件
        /// <summary>
        /// 获取Excel2003的文件类型
        /// </summary>
        public static IFileType Excel2003 { get; }
        = CreateIO.FileType("Excel2003文件", "xls");
        #endregion
        #region Excel2007文件
        /// <summary>
        /// 获取Excel2007的文件类型
        /// </summary>
        public static IFileType Excel2007 { get; }
        = CreateIO.FileType("Excel2007文件", "xlsx");
        #endregion
        #region Excel2007文件（启用宏）
        /// <summary>
        /// 获取Excel2007启用宏的文件类型
        /// </summary>
        public static IFileType Excel2007Macro { get; }
        = CreateIO.FileType("Excel2007启用宏的文件", "xlsm");
        #endregion
        #endregion
    }
}
