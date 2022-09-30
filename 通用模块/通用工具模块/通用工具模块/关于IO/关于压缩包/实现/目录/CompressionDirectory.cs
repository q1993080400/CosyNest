using System.IO.Compression;
using System.IOFrancis.BaseFileSystem;
using System.IOFrancis.FileSystem;
using System.Maths.Tree;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 该类型是<see cref="ICompressionDirectory"/>的实现，
/// 可以视为一个压缩包中的目录
/// </summary>
sealed class CompressionDirectory : CompressionDirectoryBase
{
    #region 压缩包项
    /// <summary>
    /// 获取封装的压缩包项，由于不知名的原因，
    /// 含有未知编码的目录名没有压缩包项
    /// </summary>
    private ZipArchiveEntry? Entry => Zip.GetEntry(Path);
    #endregion
    #region 路径
    private readonly string PathField;

    public override string Path
    {
        get => PathField;
        set => throw new NotImplementedException();
    }
    #endregion
    #region 删除目录
    public override void Delete()
    {
        Son.Cast<ICompressionItem>().ForEach(x => x.Delete());
        Entry?.Delete();
    }
    #endregion
    #region 解压到指定目录
    public override async Task Decompress(PathText path, bool cover = false)
    {
        var directory = IO.Path.Combine(path, this.To<IDirectoryBase>().NameFull);
        Directory.CreateDirectory(directory);
        foreach (ICompressionItem item in Son)
        {
            await item.Decompress(directory, cover);
        }
    }
    #endregion
    #region 构造函数
    /// <param name="path">压缩包中的路径</param>
    /// <inheritdoc cref="CompressionDirectoryBase(INode,ZipArchive)"/>
    public CompressionDirectory(string path, INode father)
        : base(father, father.Ancestors.To<CompressedPackage>().Zip)
    {
        PathField = path;
    }
    #endregion
}
