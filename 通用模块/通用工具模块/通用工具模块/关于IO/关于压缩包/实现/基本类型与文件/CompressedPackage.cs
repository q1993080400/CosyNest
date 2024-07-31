using System.IO.Compression;
using System.MathFrancis.Tree;
using System.Text;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 该类型是<see cref="ICompressedPackage"/>的实现，
/// 可以视为一个压缩包
/// </summary>
sealed class CompressedPackage : FromIO, ICompressedPackage
{
    #region 公开成员
    #region 关于文件系统树
    #region 根目录
    public ICompressionDirectory RootDirectory { get; }
    #endregion
    #region 父目录
    public INode? Father => null;
    #endregion
    #region 子目录
    public IEnumerable<INode> Son => [RootDirectory];
    #endregion
    #endregion
    #region 关于保存对象
    #region 读取压缩包的流
    /// <summary>
    /// 获取一个读取压缩包的流
    /// </summary>
    private Stream Stream { get; }
    #endregion
    #region 正式方法
    protected override async Task SaveRealize(string path, bool isSitu)
    {
        Zip.Dispose();
        if (!isSitu)
        {
            Stream.Reset();
            using var file = new FileStream(path, FileMode.Create);
            await Stream.CopyToAsync(file);
        }
    }
    #endregion 
    #endregion
    #region 释放对象
    protected override ValueTask DisposeAsyncActualRealize()
    {
        Zip.Dispose();
        Stream.Dispose();
        return ValueTask.CompletedTask;
    }
    #endregion
    #region 删除
    public void Delete()
    {
        Zip.Dispose();
        Stream.Dispose();
        if (Path is { } path)
            CreateIO.IO(path)?.Delete();
    }
    #endregion
    #region 重写ToString
    public override string ToString()
        => Path ?? "这个压缩包尚未保存到文件中";
    #endregion
    #endregion
    #region 内部成员
    #region 检查文件路径的扩展名
    protected override bool CheckExtensionName(string extensionName)
        => extensionName is "zip";
    #endregion
    #region 默认格式
    protected override string DefaultFormat => "zip";
    #endregion
    #region 压缩包对象
    /// <summary>
    /// 获取封装的压缩包对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ZipArchive Zip { get; }
    #endregion
    #endregion
    #region 构造函数
    #region 使用路径
    /// <summary>
    /// 使用指定的路径初始化对象
    /// </summary>
    /// <param name="path">压缩包所在的路径，
    /// 如果不存在，则创建一个新的压缩包</param>
    public CompressedPackage(string path)
        : base(path)
    {
        Stream = CreateIO.FileStream(path);
        Zip = new(Stream, ZipArchiveMode.Update, true, Encoding.UTF8);
        RootDirectory = new CompressionDirectoryRoot(this);
    }
    #endregion
    #region 使用流
    /// <summary>
    /// 使用指定的流初始化对象
    /// </summary>
    /// <param name="stream">一个用来读取压缩包的流</param>
    public CompressedPackage(Stream stream)
        : base(null)
    {
        Stream = stream;
        Zip = new(Stream, ZipArchiveMode.Update, true, Encoding.UTF8);
        RootDirectory = new CompressionDirectoryRoot(this);
    }
    #endregion
    #endregion
}
