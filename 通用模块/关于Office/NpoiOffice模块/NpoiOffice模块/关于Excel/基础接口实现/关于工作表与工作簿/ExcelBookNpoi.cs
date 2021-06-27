using System.IO;
using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Linq;
using System.Office.Excel.Realize;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是<see cref="IExcelBook"/>的实现，
    /// 它是一个用Npoi实现的Excel工作簿
    /// </summary>
    class ExcelBookNpoi : ExcelBook
    {
        #region 封装的工作簿
        /// <summary>
        /// 获取封装的工作簿，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        public IWorkbook WorkBook { get; }
        #endregion
        #region 关于工作簿
        #region 保存工作簿
        protected override void SaveRealize(string path)
        {
            if (Sheets.Any())
            {
                using var stream = new FileStream(path, FileMode.Create);
                WorkBook.Write(stream);
            }
        }
        #endregion
        #region 释放工作簿
        protected override void DisposeOfficeRealize()
            => WorkBook.Close();
        #endregion
        #region 获取工作表容器
        public override IExcelSheetCollection Sheets { get; }
        #endregion
        #region 获取打印对象
        public override IOfficePrint Print => throw CreateException.NotSupported();
        #endregion
        #endregion
        #region 构造函数
        #region 指定流
        /// <summary>
        /// 根据流创建一个工作簿
        /// </summary>
        /// <param name="stream">用来读取工作簿的流</param>
        /// <param name="isExcel2007">如果这个值为<see langword="true"/>，
        /// 代表创建Excel2007工作簿，否则代表创建2003工作簿</param>
        public ExcelBookNpoi(Stream stream, bool isExcel2007)
            : base(null, CreateNpoiOffice.SupportExcel)
        {
            WorkBook = isExcel2007 ? new XSSFWorkbook(stream) : new HSSFWorkbook(stream);
            Sheets = new ExcelSheetCollectionNpoi(this);
        }
        #endregion
        #region 指定路径
        /// <summary>
        /// 根据路径，创建一个工作簿
        /// </summary>
        /// <param name="path">工作簿所在的路径，
        /// 如果为<see langword="null"/>，则创建一个尚未保存到内存的xlsx工作簿</param>
        public ExcelBookNpoi(PathText? path)
            : base(path, CreateNpoiOffice.SupportExcel)
        {
            FileStream? file = null;
            WorkBook = path switch
            {
                null => new XSSFWorkbook(),
                var p => ToolPath.Split(p).Extended switch
                {
                    "xls" => new HSSFWorkbook(file = new FileStream(p, FileMode.Open)),
                    "xlsx" or "xlsm" => new XSSFWorkbook(p),
                    _ => throw ExceptionIO.BecauseFileType(p, CreateNpoiOffice.SupportExcel)
                }
            };
            file?.Dispose();
            Sheets = new ExcelSheetCollectionNpoi(this);
        }
        #endregion 
        #endregion
    }
}
