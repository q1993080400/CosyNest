using System.Maths;
using System.Maths.Tree;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个类型是<see cref="IDrive"/>的实现，
/// 可以视为一个驱动器
/// </summary>
/// <remarks>
/// 使用指定的驱动器初始化对象
/// </remarks>
/// <param name="drive">被封装的驱动器</param>
sealed class DriveRealize(DriveInfo drive) : IDrive
{
    #region 封装的驱动器对象
    /// <summary>
    /// 获取封装的驱动器对象，
    /// 本类型的功能就是通过它实现的
    /// </summary>
    private DriveInfo PackDrive { get; } = drive;
    #endregion
    #region 获取名称
    public string Name
        => PackDrive.Name[0].ToString();
    #endregion
    #region 获取驱动器格式
    public DriveFormat DriveFormat
        => PackDrive.DriveFormat switch
        {
            "FAT32" => DriveFormat.FAT32,
            "NTFS" => DriveFormat.NTFS,
            "FACDRomT32" => DriveFormat.CDRom,
            _ => DriveFormat.Unknown
        };
    #endregion
    #region 格式化驱动器
    public void Format(DriveFormat format, string label = "")
    {
        if (CreateIO.DriveFormatRealize is null)
            throw new NotImplementedException($"请先设置{nameof(CreateIO)}.{nameof(CreateIO.DriveFormatRealize)}属性，" +
                $"然后才能使用本方法");
        if (format is DriveFormat.Unknown)
            throw new ArgumentException($"不能将驱动器格式化为未知格式");
        CreateIO.DriveFormatRealize(this, format, label);
    }
    #endregion
    #region 关于容量
    #region 获取总容量
    public IUnit<IUTStorage> SizeTotal
        => CreateBaseMath.Unit(PackDrive.TotalSize, IUTStorage.ByteMetric);
    #endregion
    #region 获取已用容量
    public IUnit<IUTStorage> SizeUsed
        => CreateBaseMath.Unit
        (PackDrive.TotalSize - PackDrive.TotalFreeSpace, IUTStorage.ByteMetric);
    #endregion
    #endregion
    #region 关于文件系统树
    #region 获取父节点
    public INode? Father
        => CreateIO.FileSystem;
    #endregion
    #region 获取子节点
    public IEnumerable<INode> Son
        => new[] { CreateIO.Directory(PackDrive.RootDirectory.FullName) };
    #endregion
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => Name;

    #endregion
}
