using System.IOFrancis.BaseFileSystem;
using System.IOFrancis.FileSystem;
using System.Maths;
using System.Maths.Tree;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 该类型是<see cref="ICompressionItem"/>的实现，
/// 可以视为一个压缩包中的项
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="father">父节点</param>
abstract class CompressionItem(INode father) : ICompressionItem
{
    #region 关于文件系统树
    #region 父节点
    private readonly INode FatherField = father;

    INode? INode.Father => FatherField;
    #endregion
    #region 父目录
    public IDirectoryBase? Father
    {
        get => this.To<INode>().Father as IDirectoryBase;
        set => throw new NotImplementedException();
    }
    #endregion
    #region 子节点
    public abstract IEnumerable<INode> Son { get; }
    #endregion
    #endregion
    #region 关于路径与名称
    #region 路径
    public abstract string Path { get; set; }
    #endregion
    #region 完整名称
    public string NameFull
    {
        get => IO.Path.GetFileName(Path.TrimEnd('/'));
        set => throw new NotImplementedException();
    }
    #endregion
    #endregion 
    #region 删除
    public abstract void Delete();
    #endregion
    #region 文件目录大小（未实现）
    public IUnit<IUTStorage> Size => throw new NotImplementedException();
    #endregion
    #region 解压到指定目录
    public abstract Task Decompress(PathText path, bool cover = false);
    #endregion
    #region 重写ToString
    public override string ToString()
        => Path;

    #endregion
}
