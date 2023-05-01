using System.IO.Compression;
using System.IOFrancis.BaseFileSystem;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.Maths.Tree;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 该类型是<see cref="ICompressionFile"/>的实现，
/// 可以视为一个压缩包中的文件
/// </summary>
sealed class CompressionFile : CompressionItem, ICompressionFile
{
    #region 封装的对象
    /// <summary>
    /// 获取封装的Zip项，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private ZipArchiveEntry ZipEntry { get; }
    #endregion
    #region 关于路径与名称
    #region 路径
    public override string Path
    {
        get => ZipEntry.FullName;
        set => throw new NotImplementedException();
    }
    #endregion
    #region 简称
    public string NameSimple
    {
        get => ToolPath.SplitPathFile(ZipEntry.Name).Simple;
        set => throw new NotImplementedException();
    }
    #endregion
    #region 扩展名
    public string NameExtension
    {
        get => ToolPath.SplitPathFile(ZipEntry.Name).Extended ?? "";
        set => throw new NotImplementedException();
    }
    #endregion
    #endregion
    #region 子节点
    public override IEnumerable<INode> Son => Array.Empty<INode>();
    #endregion
    #region 删除文件
    public override void Delete()
        => ZipEntry.Delete();
    #endregion
    #region 读写文件
    public IFullDuplex GetBitPipe()
        => ZipEntry.Open().ToBitPipe(NameExtension);
    #endregion
    #region 关于解压缩
    #region 返回用于解压缩的管道
    public IBitRead Decompress()
        => ZipEntry.Open().ToBitPipe().Read;
    #endregion
    #region 解压到指定目录
    public override async Task Decompress(PathText path, bool cover = false)
    {
        using var read = Decompress();
        using var file = CreateIO.FileStream(IO.Path.Combine(path, this.To<IFileBase>().NameFull));
        await read.CopyTo(file);
    }
    #endregion
    #endregion 
    #region 构造函数
    /// <param name="zip">压缩包中的文件项</param>
    /// <inheritdoc cref="CompressionItem(INode)"/>
    public CompressionFile(ZipArchiveEntry zip, INode father)
        : base(father)
    {
        ZipEntry = zip;
    }
    #endregion
}
