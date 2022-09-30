using System.Drawing;
using System.IOFrancis.FileSystem;
using System.Maths;
using System.Media.Drawing;
using System.Media.Drawing.Graphics;
using System.Office.Chart;
using System.Office.Excel.Realize;
using System.Underlying.PC;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel;

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
    public override IExcelSheet Copy(Func<string, int, string>? renamed = null, IExcelSheetCollection? collection = null)
    {
        collection ??= this.Book.Sheets;
        var book = collection.Book.To<ExcelBookMicrosoft>();
        if (!ReferenceEquals(book.PackExcel, Book.To<ExcelBookMicrosoft>().PackExcel))
            throw new NotSupportedException(@"两个工作表位于不同的Excel进程，无法互相复制它们");
        var name = renamed is null ? Name : collection.Select(x => x.Name).Distinct(Name, renamed);
        var sheets = book.PackBook.Sheets;
        PackSheet.Copy(After: sheets.Last());
        return new ExcelSheetMicrosoft(book, sheets.Last())
        {
            Name = name
        };
    }
    #endregion
    #endregion
    #region 获取页面对象
    private IPageSheet? PageField;
    public override IPageSheet Page
        => PageField ??= new PageSheet(PackSheet);
    #endregion
    #region 关于单元格
    #region 根据行列号返回Range
    public override IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1]
    {
        get
        {
            var add = ExcelRealizeHelp.GetAddress(beginRow, beginColumn, endRow, endColumn);
            return new ExcelCellsMicrosoft(this, PackSheet.Range[add]);
        }
    }
    #endregion
    #region 返回行或者列
    public override IExcelRC GetRC(int begin, int? end, bool isRow)
    {
        var end2 = end ?? begin;
        var range = PackSheet.Range[ExcelRealizeHelp.GetAddressRC(begin, end2, isRow)];
        return new ExcelRCMicrosoft(this, range, isRow, begin, end2);
    }
    #endregion
    #region 返回所有单元格
    public override IExcelCells Cell
    {
        get
        {
            var (_, _, er, ec) = PackSheet.UsedRange.GetAddress();
            var range = PackSheet.Range[ExcelRealizeHelp.GetAddress(0, 0, er, ec)];
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
    #region 指定图像
    public override IExcelObj<IImage> CreateImage(IImage image)
        => CreateImage(MSOfficeRealize.SaveImage(image).Path);
    #endregion
    #region 指定路径
    #region 指定图像路径
    public override IExcelObj<IImage> CreateImage(PathText path)
    {
        using var i = Image.FromFile(path);
        var (w, h) = (i.Width, i.Height);
        #region 转换单位的本地函数
        static float Conver(Num num, IUTLength ut)
            => CreateBaseMath.Unit(num, ut).Convert(DrawingUnitsCom.LengthPoint);
        #endregion
        var newImage = PackSheet.Shapes.AddPicture
            (path.Path, MsoTriState.msoFalse, MsoTriState.msoTrue, 0, 0,
            Conver(w, CreateHardwarePC.Screen.LengthPixelX),
            Conver(h, CreateHardwarePC.Screen.LengthPixelY));
        return new ExcelImageObj(this, newImage);
    }
    #endregion
    #endregion
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
