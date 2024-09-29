using System.Diagnostics;
using System.IOFrancis.BaseFileSystem;
using System.MathFrancis.Tree;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个文件或目录
/// </summary>
public interface IIO : IIOBase
{
    #region 说明文档
    /*在实现本接口时，请遵循以下规范：
      #在初始化本对象的时候， 如果文件或目录不存在，则应当抛出异常，
      禁止初始化不存在的文件或目录，这是本接口的一个约定

      #在返回子文件和子目录的时候，应该使用迭代器延迟返回，
      如果需要遍历整个文件系统，这样可以大大改善性能*/
    #endregion
    #region 关于文件系统树
    #region 返回根节点
    /// <summary>
    /// 返回文件系统，
    /// 它是所有文件，目录和驱动器的根
    /// </summary>
    new IFileSystem Ancestors
         => (IFileSystem)this.To<INode>().Ancestors;
    #endregion
    #region 读写文件的父目录
    /// <inheritdoc cref="IIOBase.Father"/>
    new IDirectory? Father
    {
        get => this.To<IIOBase>().Father.To<IDirectory>();
        set => this.To<IIOBase>().Father = value;
    }

    /*实现本API请遵循以下规范：
      如果该对象是目录，而且是所在驱动器的根目录，则这个属性应返回null，
      但是在这种情况下,INode.Father应返回该目录的驱动器，
      这是因为驱动器不是IDirectory，但仍然是文件系统中的一个节点*/
    #endregion
    #region 获取文件或目录的驱动器
    /// <summary>
    /// 获取这个文件或目录所在的驱动器
    /// </summary>
    IDrive Drive { get; }
    #endregion
    #region 枚举直接子文件或目录
    /// <inheritdoc cref="IIOBase.Son"/>
    new IEnumerable<IIO> Son
        => this.To<INode>().Son.Cast<IIO>();
    #endregion
    #endregion
    #region 用来监视文件系统的对象
    /// <summary>
    /// 这个对象可以用来监视文件系统的更改，
    /// 如果本对象是文件，它作用于单个文件，
    /// 是目录，作用于所有子目录和孙目录
    /// </summary>
    IFileSystemWatcher FileSystemWatcher { get; }
    #endregion
    #region 是否隐藏文件或目录
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表该文件或目录为隐藏，否则代表正常显示
    /// </summary>
    bool Hide { get; set; }
    #endregion
    #region 文件或目录的创建时间
    /// <summary>
    /// 获取或设置文件或目录的创建时间
    /// </summary>
    DateTimeOffset DateCreate { get; set; }
    #endregion
    #region 对文件或目录的操作
    #region 复制
    #region 传入目录
    /// <summary>
    /// 将这个文件或目录复制到新目录，
    /// 如果这个对象是目录，还会递归复制所有子目录
    /// </summary>
    /// <param name="target">复制的目标目录，
    /// 如果为<see langword="null"/>，则默认复制到当前目录</param>
    /// <param name="newName">这个参数允许在复制的同时修改文件或目录的名称（指全名），
    /// 如果为<see langword="null"/>，代表不改名</param>
    /// <param name="rename">当复制的目标目录存在同名文件或目录时，
    /// 如果这个值为<see langword="null"/>，则覆盖同名文件或合并同名目录，
    /// 否则执行该委托赋予一个新的名称，委托的第一个参数是原始名称，
    /// 第二个参数是尝试重命名失败的次数，从2开始，以上名称均不带扩展名</param>
    /// <returns>复制后的新文件或目录</returns>
    IIO Copy(IDirectory? target, string? newName = null, Func<string, int, string>? rename = null);
    #endregion
    #region 传入目录路径
    /// <inheritdoc cref="Copy(IDirectory?, string?, Func{string, int, string}?)"/>
    IIO Copy(string? target, string? newName = null, Func<string, int, string>? rename = null);
    #endregion
    #endregion
    #region 打开
    /// <summary>
    /// 打开这个文件或目录
    /// </summary>
    /// <returns>打开文件或目录所创建的新进程</returns>
    Process Open()
       => ToolThread.Open(Path);
    #endregion
    #endregion
}
