using System.IO.Compression;
using System.IOFrancis.BaseFileSystem;
using System.IOFrancis.FileSystem;
using System.MathFrancis.Tree;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 该类型是<see cref="ICompressionDirectory"/>的实现，
/// 可以视为一个压缩包中的目录
/// </summary>
/// <param name="path">压缩包中的路径</param>
/// <inheritdoc cref="CompressionDirectoryBase(INode,ZipArchive)"/>
sealed class CompressionDirectory(string path, INode father) : CompressionDirectoryBase(father, father.Ancestors.To<CompressedPackage>().Zip)
{
    #region 压缩包项
    /// <summary>
    /// 获取封装的压缩包项，由于不知名的原因，
    /// 含有未知编码的目录名没有压缩包项
    /// </summary>
    private ZipArchiveEntry? Entry => Zip.GetEntry(Path);
    #endregion
    #region 路径
    private readonly string PathField = path;

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
        foreach (ICompressionItem item in Son.Cast<ICompressionItem>())
        {
            await item.Decompress(directory, cover);
        }
    }
    #endregion
}
