using System.IO.Compression;
using System.MathFrancis.Tree;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 该类型是<see cref="ICompressionDirectory"/>的实现，
/// 可以视为一个压缩包中的根目录
/// </summary>
/// <param name="package">根目录所在的压缩包</param>
/// <inheritdoc cref="CompressionDirectoryBase(INode,ZipArchive)"/>
sealed class CompressionDirectoryRoot(CompressedPackage package) : CompressionDirectoryBase(package, package.Zip), INode
{
    #region 路径
    public override string Path
    {
        get => "";
        set => throw new NotImplementedException();
    }
    #endregion
    #region 删除目录
    public override void Delete()
        => this.To<ICompressionItem>().Ancestors.Delete();
    #endregion
    #region 解压到指定目录
    public override Task Decompress(string path, bool cover = false)
    {
        Zip.ExtractToDirectory(path, cover);
        return Task.CompletedTask;
    }
    #endregion
}
