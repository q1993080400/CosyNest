namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个硬件信息
/// </summary>
public interface IHardwareInfo
{
    #region CPU信息
    /// <summary>
    /// 获取有关CPU的信息，
    /// CPU可能有多个
    /// </summary>
    IReadOnlyList<CPUInfo> CPUInfo { get; }
    #endregion
    #region 硬盘信息
    /// <summary>
    /// 获取有关硬盘的信息，
    /// 硬盘可能有多个
    /// </summary>
    IReadOnlyList<HardDiskInfo> HardDiskInfo { get; }
    #endregion
    #region 主板信息
    /// <summary>
    /// 获取有关主板的信息
    /// </summary>
    MotherboardInfo MotherboardInfo { get; }
    #endregion
}
