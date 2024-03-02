using System.IOFrancis.FileSystem;
using System.MathFrancis;

namespace System.IOFrancis;

/// <summary>
/// 该静态类处理有关临时文件的事务
/// </summary>
public static class ToolTemporaryFile
{
    #region 说明文档
    /*这些临时文件在程序退出时自动删除，
      即便没有自动删除，由于它们位于系统临时文件夹中，
      在清理时也会被系统顺便删除*/
    #endregion
    #region 关于临时文件目录
    #region 缓存目录
    private static IDirectory? TemporaryDirectoryField;

    /// <summary>
    /// 获取缓存目录，
    /// 在这个目录中的文件会在程序退出时自动删除
    /// </summary>
    private static IDirectory TemporaryDirectory
        => TemporaryDirectoryField ??= CreateIO.Directory(Path.GetTempPath()).Create<IDirectory>();
    #endregion
    #region 指定临时目录
    /// <summary>
    /// 指定目录为临时目录，在程序退出时，
    /// 它会被删除，注意，它不受<see cref="TemporaryLimit"/>属性的影响
    /// </summary>
    public static IList<IDirectory> Temporary { get; }
    = [];
    #endregion
    #region 指定临时文件上限
    /// <summary>
    /// 指定临时文件的大小上限，
    /// 当临时文件达到这个上限后，会将其清空，
    /// 如果为<see langword="null"/>，代表没有上限
    /// </summary>
    public static IUnit<IUTStorage>? TemporaryLimit { get; set; }
    #endregion
    #region 清除所有临时文件
    /// <summary>
    /// 手动清除所有临时文件
    /// </summary>
    public static void ClearTemporaryFile()
        => Temporary.Prepend(TemporaryDirectoryField).ForEach(x => x?.Delete());
    #endregion
    #endregion
    #region 关于临时文件
    #region 创建临时文件
    /// <summary>
    /// 创建一个临时文件
    /// </summary>
    /// <param name="extension">临时文件的扩展名</param>
    /// <returns>新创建的临时文件，该文件会在程序退出时自动删除</returns>
    public static IFile CreateTemporaryFile(string? extension = null)
    {
        if (TemporaryLimit is { } && TemporaryDirectory.Size > TemporaryLimit)
            TemporaryDirectory.Clear();
        return TemporaryDirectory.CreateFile<IFile>(nameExtended: extension);
    }
    #endregion
    #region 创建临时文件夹
    /// <summary>
    /// 创建一个临时文件夹
    /// </summary>
    /// <returns></returns>
    public static IDirectory CreateTemporaryDirectory()
    {
        if (TemporaryLimit is { } && TemporaryDirectory.Size > TemporaryLimit)
            TemporaryDirectory.Clear();
        return TemporaryDirectory.Create<IDirectory>();
    }
    #endregion
    #region 创建临时路径
    /// <summary>
    /// 创建一个临时文件路径，
    /// 但是不创建文件本身
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="CreateTemporaryFile(string?)"/>
    public static string CreateTemporaryPath(string? extension = null)
        => Path.Combine(TemporaryDirectory.Path, ToolPath.GetFullName(null, extension));
    #endregion
    #endregion 
    #region 静态构造函数
    static ToolTemporaryFile()
    {
        #region 注意事项
        /*如果程序不是自然退出，而是通过停止调试退出，
          则这个事件不会触发，这意味着清除临时文件这个功能在ASPNet上无法工作，
          因为服务器进程默认就是永不退出的，在这种情况下，请执行手动清理*/
        #endregion
        AppDomain.CurrentDomain.ProcessExit += static (_, _) => ClearTemporaryFile();               //退出时清除缓存
    }
    #endregion
}
