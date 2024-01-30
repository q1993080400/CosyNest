using System.IOFrancis.FileSystem;
using System.MathFrancis.Plane;
using System.Underlying;

namespace System.Office.Excel;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Excel页面对象，
/// 它可以管理Excel工作表的页面设置和打印
/// </summary>
public interface IPageSheet : IPage
{
    #region 获取或设置打印区域
    /// <summary>
    /// 获取或设置打印区域，
    /// 如果为<see langword="null"/>，代表没有打印区域
    /// </summary>
    ISizePosPixel? PrintRegional { get; set; }
    #endregion
    #region 打印到纸张（按照打印区域）
    /// <summary>
    /// 按照打印区域，将这个工作表打印到纸张
    /// </summary>
    /// <param name="regional">打印区域，
    /// 如果为<see langword="null"/>，则遵照<see cref="PrintRegional"/>属性设置的打印区域</param>
    /// <inheritdoc cref="IOfficePrint.PrintFromPage(Range?, int, IPrinter?)"/>
    Task PrintFromRegional(ISizePosPixel? regional = null, int number = 1, IPrinter? printer = null);
    #endregion
    #region 打印到文件（按照打印区域）
    /// <summary>
    /// 按照打印区域，将这个Excel工作表打印到文件
    /// </summary>
    ///<param name="regional">打印区域，
    /// 如果为<see langword="null"/>，则遵照<see cref="PrintRegional"/>属性设置的打印区域</param>
    /// <inheritdoc cref="IOfficePrint.PrintFromPageToFile(PathText, Range?)"/>
    Task PrintFromRegionalToFile(ISizePosPixel? regional, PathText filePath);
    #endregion
}
