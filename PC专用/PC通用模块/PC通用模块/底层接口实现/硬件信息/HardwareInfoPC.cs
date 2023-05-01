using System.Management;
using System.SafetyFrancis;

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
    #region 硬件哈希
    public string HardwareHash { get; }
    #endregion
    #endregion
    #region 构造函数
    public HardwareInfoPC()
    {
        using var cpuInfo = new ManagementClass("Win32_Processor");
        CPUInfo = cpuInfo.GetInstances().Cast<ManagementObject>().
            Select(x => new CPUInfo()
            {
                Model = (x["Name"].ToString() ?? "").Split("@")[0].Trim(),
                Number = x["ProcessorId"]?.ToString() ?? ""
            }).ToArray();
        using var motherboardInfo = new ManagementClass("Win32_BaseBoard");
        MotherboardInfo = motherboardInfo.GetInstances().OfType<ManagementObject>().
            Select(x => new MotherboardInfo()
            {
                Model = x["Product"].ToString() ?? "",
                Number = x["SerialNumber"].ToString() ?? ""
            }).SingleOrDefault() ?? new()
            {
                Model = "",
                Number = ""
            };
        using var hardDiskInfo = new ManagementClass("Win32_DiskDrive");
        HardDiskInfo = hardDiskInfo.GetInstances().OfType<ManagementObject>().
            Select(x => new HardDiskInfo()
            {
                Number = x["SerialNumber"].ToString() ?? "",
                Model = x["Model"].ToString() ?? ""
            }).ToArray();
        var hash = CPUInfo.Join(x => x.Number, ";") + MotherboardInfo.Number + HardDiskInfo.Join(x => x.Number, ";");
        HardwareHash = Task.Run(() => CreateSafety.HashText()(hash)).Result;
    }
    #endregion
}
