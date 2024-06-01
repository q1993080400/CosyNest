using System.IOFrancis.FileSystem;

namespace System.IOFrancis;

/// <summary>
/// 该静态类处理有关临时文件的事务
/// </summary>
public static class ToolTemporaryFile
{
    #region 关于临时文件目录
    #region 缓存目录
    /// <summary>
    /// 获取缓存目录
    /// </summary>
    private static IDirectory TemporaryDirectory { get; }
        = CreateIO.Directory(Path.GetTempPath()).Create<IDirectory>();
    #endregion
    #region 清除所有临时文件
    /// <summary>
    /// 手动清除所有临时文件
    /// </summary>
    public static void ClearTemporaryFile()
        => TemporaryDirectory.Clear();
    #endregion
    #endregion
    #region 关于临时文件
    #region 创建临时文件
    /// <summary>
    /// 创建一个临时文件
    /// </summary>
    /// <param name="extension">临时文件的扩展名，不要带上点号</param>
    /// <returns>新创建的临时文件，该文件会在程序退出时自动删除</returns>
    public static ITemporaryFilePack<IFile> CreateTemporaryFile(string? extension = null)
    {
        var file = TemporaryDirectory.CreateFile<IFile>(nameExtended: extension);
        return new TemporaryFilePackPathIO<IFile>(file);
    }
    #endregion
    #region 创建临时文件夹
    /// <summary>
    /// 创建一个临时文件夹
    /// </summary>
    /// <returns></returns>
    public static ITemporaryFilePack<IDirectory> CreateTemporaryDirectory()
    {
        var directory = TemporaryDirectory.Create<IDirectory>();
        return new TemporaryFilePackPathIO<IDirectory>(directory);
    }
    #endregion
    #region 创建临时路径
    /// <summary>
    /// 创建一个临时文件路径，
    /// 但是不创建文件本身
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="CreateTemporaryFile(string?)"/>
    public static ITemporaryFilePack<string> CreateTemporaryPath(string? extension = null)
    {
        var path = Path.Combine(TemporaryDirectory.Path, ToolPath.GetFullName(null, extension));
        return new TemporaryFilePackPath(path);
    }
    #endregion
    #endregion 
}
