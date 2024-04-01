using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace System.Office.Excel.Realize;

/// <summary>
/// 在实现<see cref="IExcelBook"/>时，可以继承本类型，
/// 以减少重复的工作
/// </summary>
/// <remarks>
/// 用指定的文件路径初始化Excel工作簿
/// </remarks>
/// <param name="path">工作簿所在的文件路径，
/// 如果不是通过文件创建的，则为<see langword="null"/></param>
public abstract class ExcelBook(PathText? path) : FromIO(path), IExcelBook
{
    #region 返回IExcelBook接口
    /// <summary>
    /// 返回这个对象的接口形式，
    /// 它可以用来访问一些显式实现的成员
    /// </summary>
    protected IExcelBook Interface => this;
    #endregion
    #region 关于工作簿
    #region 返回打印对象
    public abstract IOfficePrint Print { get; }
    #endregion
    #region 开启或关闭自动计算
    public abstract bool AutoCalculation { get; set; }
    #endregion
    #endregion
    #region 返回工作表的容器
    public abstract IExcelSheetManage SheetManage { get; }
    #endregion
}
