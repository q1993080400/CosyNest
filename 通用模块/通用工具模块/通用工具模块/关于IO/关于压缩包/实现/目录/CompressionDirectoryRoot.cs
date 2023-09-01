using System.IO.Compression;
using System.IOFrancis.FileSystem;
using System.Maths.Tree;

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
        => throw new NotSupportedException("不能删除压缩包的根文件夹，你可以直接删除整个压缩包");
    #endregion
    #region 解压到指定目录
    public override Task Decompress(PathText path, bool cover = false)
        => Task.Run(() => Zip.ExtractToDirectory(path, cover));

    #endregion
}
