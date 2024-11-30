using System.Management;

namespace System.Underlying.PC;

/// <summary>
/// 这个类型是<see cref="IHardwareInfo"/>的实现，
/// 可以视为一个PC平台的硬件信息
/// </summary>
sealed class HardwareInfoPC : IHardwareInfo
{
    #region 公开成员
    #region CPU信息
    public IReadOnlyList<CPUInfo> CPUInfo { get; }
    #endregion
    #region 主板信息
    public MotherboardInfo MotherboardInfo { get; }
    #endregion
    #region 硬盘信息
    public IReadOnlyList<HardDiskInfo> HardDiskInfo { get; }
    #endregion
    #endregion
    #region 构造函数
    public HardwareInfoPC()
    {
        using var cpuInfo = new ManagementClass("Win32_Processor");
        CPUInfo = cpuInfo.GetInstances().Cast<ManagementObject>().
            Select(static x => new CPUInfo()
            {
                Model = (x["Name"].ToString() ?? "").Split("@")[0].Trim(),
                Number = x["ProcessorId"]?.ToString() ?? ""
            }).ToArray();
        using var motherboardInfo = new ManagementClass("Win32_BaseBoard");
        MotherboardInfo = motherboardInfo.GetInstances().OfType<ManagementObject>().
            Select(static x => new MotherboardInfo()
            {
                Model = x["Product"].ToString() ?? "",
                Number = x["SerialNumber"].ToString() ?? ""
            }).SingleOrDefaultSecure() ?? new()
            {
                Model = "",
                Number = ""
            };
        using var hardDiskInfo = new ManagementClass("Win32_DiskDrive");
        HardDiskInfo = hardDiskInfo.GetInstances().OfType<ManagementObject>().
            Select(static x => new HardDiskInfo()
            {
                Number = x["SerialNumber"].ToString() ?? "",
                Model = x["Model"].ToString() ?? ""
            }).ToArray();
        var hash = CPUInfo.Join(static x => x.Number, ";") + MotherboardInfo.Number + HardDiskInfo.Join(static x => x.Number, ";");
    }
    #endregion
}
