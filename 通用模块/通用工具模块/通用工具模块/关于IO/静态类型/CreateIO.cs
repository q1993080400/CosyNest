using System.IOFrancis.Bit;
using System.IOFrancis.Compressed;
using System.IOFrancis.FileSystem;
using System.Text;

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
        => ToolPath.GetPathState(path) switch
        {
            PathState.ExistDirectory => new DirectoryRealize(path),
            PathState.ExistFile => new FileRealize(path),
            PathState.NotPermissions => throw new NotSupportedException($"{path}存在，但是没有权限访问"),
            PathState.NotLegal => throw new NotSupportedException($"{path}不是合法的文件或目录路径"),
            _ => null
        };
    #endregion
    #region 泛型方法
    /// <typeparam name="IO">返回值会被转换为这个类型</typeparam>
    /// <inheritdoc cref="IO(PathText)"/>
    public static IO? IO<IO>(string path)
        where IO : IIO
        => CreateIO.IO(path) is IO i ? i : default;
    #endregion
    #endregion
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
    #region 创建委托
    #region 有关读写字符串
    #region 读取
    /// <summary>
    /// 创建一个<see cref="ObjRead{Obj}"/>，
    /// 它能够通过二进制管道读取文本，文本按行返回
    /// </summary>
    /// <param name="encoder">文本的编码，
    /// 如果为<see langword="null"/>，则默认为UTF8</param>
    /// <returns></returns>
    public static ObjRead<string> ObjReadString(Encoding? encoder = null)
        => read =>
        {
            #region 本地函数
            static async IAsyncEnumerable<string> Fun(Encoding? encoder, IBitRead read)
            {
                using var stream = read.ToStream();
                using StreamReader streamReader = encoder is null ? new(stream) : new(stream, encoder);
                while (true)
                {
                    var text = await streamReader.ReadLineAsync();
                    if (text is null)
                        yield break;
                    yield return text;
                }
            }
            #endregion
            return Fun(encoder ?? Encoding.UTF8, read);
        };
    #endregion
    #region 写入（直接写入）
    /// <summary>
    /// 创建一个<see cref="ObjWrite{Obj}"/>，
    /// 它能够向二进制管道写入文本
    /// </summary>
    /// <param name="encoder">文本的编码，
    /// 如果为<see langword="null"/>，则为UTF8</param>
    /// <returns></returns>
    public static ObjWrite<string> ObjWriteString(Encoding? encoder = null)
    {
        encoder ??= Encoding.UTF8;
        return async (write, text) =>
        {
            var bytes = encoder.GetBytes(text);
            await write.Write(bytes);
        };
    }
    #endregion
    #region 写入（按行写入）
    /// <summary>
    /// 创建一个<see cref="ObjWrite{Obj}"/>，
    /// 它能够向二进制管道按行写入文本
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ObjWriteString(Encoding?)"/>
    public static ObjWrite<string> ObjWriteStringLine(Encoding? encoder = null)
    {
        var write = ObjWriteString(encoder);
        return (w, t) => write(w, t + Environment.NewLine);
    }
    #endregion
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
    #region 创建文件类型
    #region 传入文件名
    /// <summary>
    /// 创建一个新的文件类型，注意：执行此方法会将文件类型注册
    /// </summary>
    /// <param name="description">对文件类型的说明</param>
    /// <param name="extension">指定的扩展名集合，不带点号</param>
    public static IFileType FileType(string description, params string[] extension)
        => new FileType(description, extension);
    #endregion
    #region 传入文件类型
    /// <inheritdoc cref="FileType(string, string[])"/>
    /// <param name="compatible">初始化后的新对象将会兼容以上所有文件类型</param>
    public static IFileType FileType(string description, params IFileType[] compatible)
        => FileType(description,
             compatible.Select(x => x.ExtensionName).UnionNesting(true).ToArray());
    #endregion
    #endregion
    #region 创建压缩包
    #region 说明文档
    /*注意事项
      #本类型所读取的压缩包文件名只支持UTF8，
      但是Windows默认的压缩包编码是GBK，
      这导致在压缩和解压中文文件时会出现乱码

      经过重新设计，这个问题带来的影响被减轻了，
      除无法正常识别文件/目录名以外，不会出现其他问题，但是仍有一个隐患：
      UTF8将所有无法识别的字符统一转换为一个乱码字符，假设有原始路径a和b，a!=b，
      出现乱码后产生新字符串A和B，在非常巧合的情况下，A和B可能是相等的，这会导致程序崩溃
    
      如果需要支持中文，你可以在压缩文件时手动指定编码，方法如下：
      以7Z为例，执行压缩的时候，输入参数：
      cu=on
    
      由于底层设计缺陷，这个问题似乎无法解决，
      如果它带来的影响非常严重，
      作者会考虑使用不会产生乱码的开源库来实现这个接口*/
    #endregion
    #region 根据路径
    /// <summary>
    /// 根据路径，创建一个压缩包
    /// </summary>
    /// <param name="autoSave">如果这个值为<see langword="true"/>，
    /// 则在释放压缩包的时候，还会自动保存它</param>
    /// <inheritdoc cref="CompressedPackage(string)"/>
    public static ICompressedPackage Compressed(string path, bool autoSave)
        => new CompressedPackage(path)
        {
            AutoSave = autoSave
        };
    #endregion
    #region 根据流
    /// <summary>
    /// 根据流，创建一个压缩包
    /// </summary>
    /// <inheritdoc cref="Compressed(string, bool)"/>
    /// <inheritdoc cref="CompressedPackage(Stream)"/>
    public static ICompressedPackage Compressed(Stream stream, bool autoSave)
        => new CompressedPackage(stream)
        {
            AutoSave = autoSave
        };
    #endregion
    #endregion
    #region 创建简单文件路径协议
    /// <summary>
    /// 返回一个简单文件路径协议，
    /// 它可以将文件名加上一个<see cref="Guid"/>前缀，使其永不重名，
    /// 或者解析文件路径，去掉<see cref="Guid"/>前缀，返回原始的文件名（不是文件路径）
    /// </summary>
    public static (GenerateFilePathProtocol<string, string> Generate, AnalysisFilePathProtocol<string, string> Analysis) SimpleFilePathProtocol { get; }
    = (IOFrancis.SimpleFilePathProtocol.GenerateFilePathProtocol, IOFrancis.SimpleFilePathProtocol.AnalysisFilePathProtocol);
    #endregion
}
