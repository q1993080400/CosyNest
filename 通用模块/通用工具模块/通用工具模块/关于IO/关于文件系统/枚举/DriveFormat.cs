namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个枚举表示驱动器文件系统的格式
/// </summary>
public enum DriveFormat
{
    /// <summary>
    /// 未知格式
    /// </summary>
    Unknown,
    /// <summary>
    /// FAT32格式
    /// </summary>
    FAT32,
    /// <summary>
    /// NTFS格式
    /// </summary>
    NTFS,
    /// <summary>
    /// CDRom格式
    /// </summary>
    CDRom
}
