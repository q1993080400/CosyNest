using System.IO.Compression;
using System.IOFrancis.BaseFileSystem;
using System.IOFrancis.FileSystem;
using System.Maths.Tree;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 该类型是<see cref="IDirectoryBase"/>的实现，
/// 它是压缩包目录的基类
/// </summary>
abstract class CompressionDirectoryBase : CompressionItem, ICompressionDirectory
{
    #region 压缩包
    /// <summary>
    /// 获取封装的压缩包对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected ZipArchive Zip { get; }
    #endregion
    #region 创建文件目录
    public IO Create<IO>(string? name = null)
        where IO : IIOBase
    {
        var type = typeof(IO);
        name = System.IO.Path.Combine(Path, IDirectoryBase.GetUniqueName(this, name));
        if (type == typeof(ICompressionFile))
            return new CompressionFile(Zip.CreateEntry(name), this).To<IO>();
        if (type == typeof(ICompressionDirectory))
        {
            var create = Zip.CreateEntry(name + "/");
            return new CompressionDirectory(create.FullName, this).To<IO>();
        }
        throw new NotSupportedException($"参数必须是{nameof(ICompressionFile)}或{nameof(ICompressionDirectory)}，而不是{type}");
    }
    #endregion
    #region 添加文件或目录
    public ICompressionItem Add(IIO item)
    {
        var name = item.NameFull;
        switch (item)
        {
            case IFile:
                var path = IO.Path.Combine(Path, name);
                var compression = Zip.CreateEntryFromFile(item.Path, path);
                return new CompressionFile(compression, this);
            case IDirectory directory:
                var cd = Create<ICompressionDirectory>(name);
                directory.Son.ForEach(x => cd.Add(x));
                return cd;
            default:
                throw new NotSupportedException($"{item.GetType()}既不是文件，也不是目录");
        }
    }
    #endregion
    #region 尝试直达文件或目录
    public IO? Direct<IO>(string relativelyPath)
        where IO : IIOBase
        => throw new NotImplementedException();
    #endregion
    #region 子节点
    public override IEnumerable<INode> Son
    {
        get
        {
            var regex =/*language=regex*/@$"^{Path}/?[^/]+/?".Op().Regex();
            var items = Zip.Entries.Select(x => regex.MatcheSingle(x.FullName)).
                Where(x => x is { }).Select(x => x!.Match).Distinct().ToArray();
            foreach (var item in items)
            {
                var entry = Zip.GetEntry(item)!;
                yield return entry switch
                {
                    null or { Name: "" } => new CompressionDirectory(item, this),
                    var e => new CompressionFile(e, this)
                };
            }
        }
    }
    #endregion
    #region 构造函数
    /// <param name="zip">压缩包对象</param>
    /// <inheritdoc cref="CompressionItem(INode)"/>
    public CompressionDirectoryBase(INode father, ZipArchive zip)
        : base(father)
    {
        Zip = zip;
    }
    #endregion
}
