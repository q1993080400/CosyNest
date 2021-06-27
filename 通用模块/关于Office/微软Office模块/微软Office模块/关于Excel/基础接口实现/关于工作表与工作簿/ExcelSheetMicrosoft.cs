using System.Collections.Generic;
using System.Drawing;
using System.DrawingFrancis;
using System.DrawingFrancis.Graphics;
using System.Linq;
using System.Maths;
using System.Office.Chart;
using System.Office.Excel.Realize;
using System.Underlying.PC;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型代表通过微软COM组件实现的Excel工作表
    /// </summary>
    class ExcelSheetMicrosoft : ExcelSheet, IExcelSheet
    {
        #region 封装的Excel工作表
        /// <summary>
        /// 获取封装的Excel工作表，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal Worksheet PackSheet { get; }
        #endregion
        #region 关于工作表
        #region 读写工作表的名称
        public override string Name
        {
            get => PackSheet.Name;
            set => PackSheet.Name = value;
        }
        #endregion
        #region 删除工作表
        public override void Delete()
        {
            if (Book.Sheets.Count == 1)
                Book.DeleteBook();
            else
            {
                PackSheet.Unprotect();
                PackSheet.Delete();
            }
        }
        #endregion
        #region 复制工作表
        public override IExcelSheet Copy(IExcelSheetCollection collection, Func<string, int, string>? renamed = null)
        {
            if (renamed is { })
                throw new NotSupportedException("由于时间限制，暂未支持显式指定修改工作表名称的委托，" +
                    "请不要显式指定这个参数，或者抽出时间实现这个功能");
            var book = collection.Book.To<ExcelBookMicrosoft>();
            var sheets = book.PackBook.Sheets;
            PackSheet.Copy(After: sheets.Last());
            return new ExcelSheetMicrosoft(book, sheets.Last());
        }
        #endregion
        #endregion
        #region 获取页面对象
        private IPageSheet? PageField;
        public override IPageSheet Page
            => PageField ??= new PageSheet(PackSheet);
        #endregion
        #region 关于Range
        #region 返回行或者列
        public override IExcelRC GetRC(int begin, int? end, bool isRow)
        {
            var end2 = end ?? begin;
            var range = PackSheet.Range[ExcelRealize.GetAddressRC(begin, end2, isRow)];
            return new ExcelRCMicrosoft(this, range, isRow, begin, end2);
        }
        #endregion
        #region 返回用户范围
        public override IExcelCells RangUser
        {
            get
            {
                var (_, _, er, ec) = PackSheet.UsedRange.GetAddress();
                var range = PackSheet.Range[ExcelRealize.GetAddress(0, 0, er, ec)];
                return new ExcelCellsMicrosoft(this, range);
            }
        }
        #endregion
        #endregion
        #region 关于Excel对象
        #region 关于图表
        #region 枚举所有图表
        public override IEnumerable<IExcelObj<IOfficeChart>> Charts
            => PackSheet.Shapes.GetShapes().
            Where(x => x.IsChart()).
            Select(x => x.ToChart(this));
        #endregion
        #region 获取图表创建器
        private ICreateExcelChart? CreateChartFiled;

        public override ICreateExcelChart CreateChart
            => CreateChartFiled ??= new CreateChartExcelMicrosoft(this);
        #endregion
        #endregion
        #region 关于图片
        #region 枚举所有图片
        public override IEnumerable<IExcelObj<IImage>> Images
            => PackSheet.Shapes.GetShapes().
            Where(x => x.IsImage()).
            Select(x => new ExcelImageObj(this, x));
        #endregion
        #region 添加图片
        public override IExcelObj<IImage> CreateImage(IImage image)
        {
            var path = MSOfficeRealize.SaveImage(image);
            using var i = Image.FromFile(path.Path);
            var (w, h) = (i.Width, i.Height);
            #region 转换单位的本地函数
            static float Conver(Num num, IUTLength ut)
                => CreateBaseMath.Unit(num, ut).ConvertSingle(DrawingUnitsCom.LengthPoint);
            #endregion
            var newImage = PackSheet.Shapes.AddPicture
                (path.Path, MsoTriState.msoFalse, MsoTriState.msoTrue, 0, 0,
                Conver(w, CreateHardwarePC.Screen.LengthPixelX),
                Conver(h, CreateHardwarePC.Screen.LengthPixelY));
            return new ExcelImageObj(this, newImage);
        }
        #endregion
        #endregion
        #region 返回画布
        private ICanvas? CanvasField;

        public override ICanvas Canvas
            => CanvasField ??= new ExcelCanvas(PackSheet.Shapes);
        #endregion
        #endregion
        #region 构造函数
        /// <inheritdoc cref="ExcelSheet(IExcelBook)"/>
        /// <param name="sheet">被封装的工作表</param>
        public ExcelSheetMicrosoft(IExcelBook book, Worksheet sheet)
            : base(book)
        {
            this.PackSheet = sheet;
        }
        #endregion
    }
}
