using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;

namespace System.IOFrancis;

/// <summary>
/// 这个静态类可以用来创建和IO有关的对象
/// </summary>
public static class CreateIO
{
    #region 关于文件系统
    #region 返回当前文件系统
    /// <summary>
    /// 返回当前文件系统，
    /// 它是所有文件，目录，驱动器的根
    /// </summary>
    public static IFileSystem FileSystem { get; }
    = new FileSystemRealize();
    #endregion
    #region 用来格式化驱动器的委托
    /// <summary>
    /// 这个委托用来执行格式化驱动器的操作，
    /// 它的第一个参数是要格式化的驱动器，
    /// 第二个参数是格式化后的文件系统格式，第三个参数是格式化后的卷标，
    /// 只有当它被设置以后，<see cref="IDrive.Format(DriveFormat,string)"/>方法才能正常调用
    /// </summary>
    public static Action<IDrive, DriveFormat, string>? DriveFormatRealize { get; set; }

    /*问：为什么需要本属性？
      答：格式化驱动器的方法是一个接口成员，但是，
      该方法的实现与各个平台是紧密耦合的，
      如果为每一个平台提供独立的IDrive实现，这非常麻烦，
      因此作者将该方法的实现单独抽离出来放在这里*/
    #endregion
    #region 创建文件或目录对象
    #region 创建一个文件对象
    /// <summary>
    /// 用指定的路径初始化文件对象，
    /// 不允许指定不存在的路径
    /// </summary>
    /// <param name="path">指定的路径</param>
    /// <param name="checkExist">在文件不存在的时候，如果这个值为<see langword="true"/>，
    /// 则抛出一个异常，为<see langword="false"/>，则不会抛出异常，而是会创建一个新文件</param>
    public static IFile File(string path, bool checkExist = true)
        => new FileRealize(path, checkExist);
    #endregion
    #region 创建一个目录对象
    /// <summary>
    /// 用指定的路径初始化目录对象
    /// </summary>
    /// <param name="path">指定的路径</param>
    /// <param name="checkExist">在路径不存在的时候，如果这个值为<see langword="true"/>，会抛出一个异常，
    /// 如果为<see langword="false"/>，则不会抛出异常，而是会创建这个目录</param>
    public static IDirectory Directory(string path, bool checkExist = true)
        => new DirectoryRealize(path, checkExist);
    #endregion
    #region 根据路径，返回IO对象
    #region 非泛型方法
    /// <summary>
    /// 如果一个路径是文件，返回文件对象，
    /// 是目录，返回目录对象，
    /// 不存在，返回默认值
    /// </summary>
    /// <param name="path">要检查的路径</param>
    /// <returns></returns>
    public static IIO? IO(string path)
    {
        if (System.IO.File.Exists(path))
            return new FileRealize(path);
        return System.IO.Directory.Exists(path) ?
            new DirectoryRealize(path) : null;
    }
    #endregion
    #region 泛型方法
    /// <typeparam name="IO">返回值会被转换为这个类型</typeparam>
    /// <inheritdoc cref="IO(string)"/>
    public static IO? IO<IO>(string path)
        where IO : IIO
        => CreateIO.IO(path) is IO i ? i : default;
    #endregion
    #endregion
    #endregion
    #region 创建临时路径
    /// <summary>
    /// 创建一个<see cref="IDisposable"/>，
    /// 当它被释放的时候，会自动删除临时路径上面的文件或目录
    /// </summary>
    /// <param name="path">临时文件或目录的路径</param>
    /// <returns></returns>
    public static IDisposable TemporaryPath(string path)
        => FastRealize.Disposable(() => IO(path)?.Delete());
    #endregion
    #endregion
    #region 创建管道
    #region 创建全双工管道
    /// <summary>
    /// 创建一个全双工管道，
    /// 它可以同时进行读写
    /// </summary>
    /// <param name="read">用来读取的管道</param>
    /// <param name="write">用来写入的管道</param>
    /// <returns></returns>
    public static IFullDuplex FullDuplex(IBitRead read, IBitWrite write)
        => new FullDuplex(read, write);
    #endregion
    #region 创建读写文件的管道
    /// <summary>
    /// 创建一个用来读取文件的管道
    /// </summary>
    /// <param name="path">文件所在的路径</param>
    /// <param name="mod">指定打开文件的方式</param>
    /// <returns></returns>
    public static IFullDuplex FullDuplexFile(string path, FileMode mod = FileMode.OpenOrCreate)
    {
        var extended = ToolPath.SplitFilePath(path).Extended;
        return new FileStream(path, mod).ToBitPipe(extended);
    }
    #endregion
    #region 创建临时文件管道
    /// <summary>
    /// 创建一个能够读取且仅能读取临时文件的管道，
    /// 当它被释放掉以后，这个临时文件会被删除
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="BitReadTemporaryFile.BitReadTemporaryFile(PathText)"/>
    public static IBitRead BitReadTemporaryFile(string path)
        => new BitReadTemporaryFile(path);
    #endregion
    #region 创建读写内存的管道
    /// <summary>
    /// 创建一个管道，它可以从内存中读写数据
    /// </summary>
    /// <returns></returns>
    public static IFullDuplex FullDuplexMemory()
        => new MemoryStream().ToBitPipe();
    #endregion
    #endregion
    #region 创建流
    #region 通过迭代器枚举数据
    #region 同步迭代器
    /// <summary>
    /// 创建一个<see cref="Stream"/>，
    /// 它通过迭代器获取二进制数据
    /// </summary>
    /// <param name="data">用来获取数据的迭代器</param>
    /// <returns></returns>
    public static Stream StreamEnumerable(IEnumerable<byte[]> data)
        => new EnumerableStream(data.GetEnumerator());
    #endregion
    #region 异步迭代器
    #region 适配枚举字节集合的迭代器
    /// <inheritdoc cref="StreamEnumerable(IEnumerable{byte[]})"/>
    public static Stream StreamEnumerable(IAsyncEnumerable<byte[]> data)
        => new EnumerableStream(data.ToBlockingEnumerable().GetEnumerator());
    #endregion
    #endregion
    #endregion
    #region 创建文件流
    /// <summary>
    /// 创建一个文件流，它打开或创建一个文件路径，
    /// 与原生方法不同的是，它避免了一些可能错误
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="mod">指定打开文件的方式</param>
    /// <returns></returns>
    public static FileStream FileStream(string path, FileMode mod = FileMode.OpenOrCreate)
    {
        ToolIO.CreateFather(path);
        return new(path, mod);
    }
    #endregion
    #endregion
}
