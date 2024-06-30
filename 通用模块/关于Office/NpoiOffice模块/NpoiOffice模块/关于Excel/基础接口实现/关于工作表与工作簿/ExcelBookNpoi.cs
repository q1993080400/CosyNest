using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Office.Excel.Realize;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace System.Office.Excel;

/// <summary>
/// 这个类型是<see cref="IExcelBook"/>的实现，
/// 它是一个用Npoi实现的Excel工作簿
/// </summary>
sealed class ExcelBookNpoi : ExcelBook
{
    #region 封装的工作簿
    /// <summary>
    /// 获取封装的工作簿，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    public IWorkbook WorkBook { get; }
    #endregion
    #region 关于工作簿
    #region 默认格式
    protected override string DefaultFormat
        => "xlsx";
    #endregion
    #region 是否为2007格式
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表工作簿为2007格式，否则为2003格式
    /// </summary>
    internal bool Is2007 { get; }
    #endregion
    #region 保存工作簿
    protected override Task SaveRealize(string path, bool isSitu)
    {
        if (SheetManage.Count is 0)
            return Task.CompletedTask;
        if (isSitu)
        {
            using var cache = ToolTemporaryFile.CreateTemporaryFile();
            var cacheFile = cache.TemporaryObj;
            using var stream = cacheFile.GetBitPipe().Write.ToStream();
            WorkBook.Write(stream);
            WorkBook.Close();
            stream.Dispose();
            cacheFile.Path = path;
            #region 说明文档
            /*此处可能有Bug，因为在保存工作簿后，会将其释放，
              这是因为Npoi的傻逼设计造成的，它会在释放前一直独占工作簿文件，
              如果在此之后继续操作工作簿，会引发异常，
              但是考虑到保存工作簿这个操作一般是放在最后，
              绝大多数情况下不会出现问题*/
            #endregion
        }
        else
        {
            using var stream = new FileStream(path, FileMode.Create);
            WorkBook.Write(stream);
        }
        return Task.CompletedTask;
    }
    #endregion
    #region 释放工作簿
    protected override ValueTask DisposeAsyncActualRealize()
    {
        WorkBook.Close();
        return ValueTask.CompletedTask;
    }
    #endregion
    #region 获取工作表容器
    public override IExcelSheetManage SheetManage { get; }
    #endregion
    #region 获取打印对象
    public override IOfficePrint Print => throw CreateException.NotSupported();
    #endregion
    #region 是否启用自动计算
    public override bool AutoCalculation
    {
        get => false;
        set
        {

        }
    }

    /*说明文档
      这个属性对于NPOI来说非常关键，
      因为NPOI对冷门Excel函数的支持非常不完善，
      当使用这些函数时，开启自动计算会损坏工作表*/
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
        : base(null)
    {
        WorkBook = isExcel2007 ? new XSSFWorkbook(stream) : new HSSFWorkbook(stream);
        SheetManage = new ExcelSheetManageNpoi(this);
        Is2007 = isExcel2007;
    }
    #endregion
    #region 指定路径
    /// <summary>
    /// 根据路径，创建一个工作簿
    /// </summary>
    /// <param name="path">工作簿所在的路径，
    /// 如果为<see langword="null"/>，则创建一个尚未保存到内存的xlsx工作簿</param>
    public ExcelBookNpoi(string? path)
        : base(path)
    {
        path = path is null ? null : IO.Path.GetFullPath(path);
        path = File.Exists(path) ? path : null;
        Is2007 = path switch
        {
            null => true,
            var p => ToolPath.SplitFilePath(p).Extended switch
            {
                "xls" => false,
                "xlsx" or "xlsm" => true,
                _ => throw IOExceptionFrancis.BecauseFileType(p, CreateOfficeNpoi.SupportExcel)
            }
        };
        FileStream? file = null;
        WorkBook = (Is2007, path) switch
        {
            (true, null) => new XSSFWorkbook(),
            (true, { } p) => new XSSFWorkbook(p),
            (false, null) => new HSSFWorkbook(),
            (false, { } p) => new HSSFWorkbook(file = new(p, FileMode.Open))
        };
        file?.Dispose();
        SheetManage = new ExcelSheetManageNpoi(this);
    }
    #endregion
    #endregion
}
