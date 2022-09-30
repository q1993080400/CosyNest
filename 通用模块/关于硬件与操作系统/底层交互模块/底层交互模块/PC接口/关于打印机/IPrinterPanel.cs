namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个打印机面板，
/// 它可以枚举和管理当前硬件上的所有打印机
/// </summary>
public interface IPrinterPanel
{
    #region 按名称索引所有打印机
    /// <summary>
    /// 根据名称索引当前硬件上的所有打印机
    /// </summary>
    IReadOnlyDictionary<string, IPrinter> Printers { get; }
    #endregion
    #region 默认打印机
    /// <summary>
    /// 获取或设置默认打印机
    /// </summary>
    IPrinter DefaultPrinter { get; set; }
    #endregion
}
