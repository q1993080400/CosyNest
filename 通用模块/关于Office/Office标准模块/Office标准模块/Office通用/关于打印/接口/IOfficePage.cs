using System.Underlying;

namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个页面对象，
/// 它可以管理Word或Excel的页面设置和打印
/// </summary>
public interface IOfficePage
{
    #region 关于打印
    #region 打印到纸张
    /// <summary>
    /// 按照页码将这个Excel工作表，工作簿，或Word文档打印到纸张，
    /// 注意：将打印任务发送到打印机以后，本方法会立即返回，
    /// 不会等待打印任务全部完成
    /// </summary>
    /// <param name="page">这个参数指示打印的页码范围，
    /// 如果为<see langword="null"/>，代表全部打印，页码从0开始</param>
    /// <param name="number">打印的份数</param>
    /// <param name="printer">执行打印的打印机，如果为<see langword="null"/>，则使用默认打印机</param>
    /// <returns>这个元组的第一个项是打印的所有页数，通过它可以估算打印任务的完成时间，
    /// 第二个项是一个估算的，用于等待打印完成的<see cref="Task"/></returns>
    (int PageCount, Task Wait) PrintFromPage(Range? page = null, int number = 1, IPrinter? printer = null);
    #endregion
    #region 打印到文件
    /// <summary>
    /// 按照页码将这个Excel工作表，工作簿或Word文档打印到文件
    /// </summary>
    /// <param name="filePath">指定打印的目标文件路径，
    /// 函数会根据该路径的扩展名自动判断应使用哪个打印机</param>
    /// <inheritdoc cref="PrintFromPage(Range?, int, IPrinter?)"/>
    (int PageCount, Task Wait) PrintFromPageToFile(string filePath, Range? page = null);
    #endregion
    #endregion
}
