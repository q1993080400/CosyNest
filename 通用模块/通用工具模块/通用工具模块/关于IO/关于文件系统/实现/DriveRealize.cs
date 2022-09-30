using System.Maths;
using System.Maths.Tree;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个类型是<see cref="IDrive"/>的实现，
/// 可以视为一个驱动器
/// </summary>
sealed class DriveRealize : IDrive
{
    #region 封装的驱动器对象
    /// <summary>
    /// 获取封装的驱动器对象，
    /// 本类型的功能就是通过它实现的
    /// </summary>
    private DriveInfo PackDrive { get; }
    #endregion
    #region 获取名称
    public string Name
        => PackDrive.Name[0].ToString();
    #endregion
    #region 获取驱动器格式
    public string Format
        => PackDrive.DriveFormat;
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
    #region 构造函数
    /// <summary>
    /// 使用指定的驱动器初始化对象
    /// </summary>
    /// <param name="drive">被封装的驱动器</param>
    public DriveRealize(DriveInfo drive)
    {
        PackDrive = drive;
    }
    #endregion
}
