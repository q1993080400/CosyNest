using System.Collections.Generic;
using System.DrawingFrancis;
using System.DrawingFrancis.Graphics;
using System.Office.Chart;
using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是<see cref="ExcelSheet"/>的实现，
    /// 是一个通过EPPlus实现的Excel工作表
    /// </summary>
    class ExcelSheetEPPlus : ExcelSheet
    {
        #region 封装的对象
        #region 工作表
        /// <summary>
        /// 获取封装的工作表，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private ExcelWorksheet Sheet { get; }
        #endregion
        #region 工作表集合
        /// <summary>
        /// 获取封装的工作表集合，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private ExcelWorksheets Sheets
            => Sheet.Workbook.Worksheets;
        #endregion
        #endregion
        #region 关于工作表
        #region 名称
        public override string Name
        {
            get => Sheet.Name;
            set => Sheet.Name = value;
        }
        #endregion
        #region 删除工作表
        public override void Delete()
            => Sheets.Delete(Sheet);
        #endregion
        #region 复制工作表
        public override IExcelSheet Copy(IExcelSheetCollection collection, Func<string, int, string>? renamed = null)
        {
            var sheets = collection.To<ExcelSheetCollectionEPPlus>().Sheets;
            var newSheet = sheets.Add(ExcelRealize.SheetRepeat(Book.Sheets, Name, renamed), Sheet);
            return new ExcelSheetEPPlus(collection.Book, newSheet);
        }
        #endregion
        #endregion
        #region 关于单元格
        #region 返回用户范围
        public override IExcelCells RangUser { get; }
        #endregion
        #region 返回行或列
        public override IExcelRC GetRC(int begin, int? end, bool isRow)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
        #region 关于打印和Excel对象
        #region 获取页面对象
        public override IPageSheet Page => throw new NotImplementedException();
        #endregion
        #region 获取图表创建器
        public override ICreateExcelChart CreateChart => throw new NotImplementedException();
        #endregion
        #region 获取图表集合
        public override IEnumerable<IExcelObj<IOfficeChart>> Charts => throw new NotImplementedException();
        #endregion
        #region 获取图片集合
        public override IEnumerable<IExcelObj<IImage>> Images => throw new NotImplementedException();
        #endregion
        #region 获取图片创建器
        public override IExcelObj<IImage> CreateImage(IImage image)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 获取画布
        public override ICanvas Canvas => throw new NotImplementedException();
        #endregion
        #endregion
        #region 构造函数
        /// <inheritdoc cref="ExcelSheet(IExcelBook)"/>
        /// <param name="sheet">封装的工作表，本对象的功能就是通过它实现的</param>
        public ExcelSheetEPPlus(IExcelBook book, ExcelWorksheet sheet)
            : base(book)
        {
            Sheet = sheet;
            RangUser = new ExcelCellsEPPlus(this, Sheet.Cells);
        }
        #endregion
    }
}
