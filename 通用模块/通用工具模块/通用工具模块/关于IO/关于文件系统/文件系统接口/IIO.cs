using System.Diagnostics;
using System.IOFrancis.BaseFileSystem;
using System.Maths.Tree;

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
    IIO Copy(string? target, string? newName = null, Func<string, int, string>? rename = null)
        => Copy(CreateIO.Directory(target ?? Father?.Path ?? Path, false), newName, rename);
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
    #region 对文件或目录执行原子操作
    #region 说明文档
    /*问：文件或目录的原子操作是什么意思？
       答：指类似数据库事务的模式，
       如果在操作过程中出现了异常，
       则不会对文件或目录产生修改

       问：原子操作有性能损失吗？
       答：由于可能会涉及创建备份之类的操作，
       因此对于大文件，原子操作有明显的性能损失，
       建议仅在必要时使用*/
    #endregion
    #region 有返回值
    /// <summary>
    /// 对文件或目录执行原子操作
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="delegate">对文件或目录进行原子操作的委托，
    /// 参数就是这个<see cref="IIO"/>对象本身</param>
    /// <returns>执行<paramref name="delegate"/>所获取的返回值</returns>
    Ret Atomic<Ret>(Func<IIO, Ret> @delegate);
    #endregion
    #region 无返回值
    /// <summary>
    /// 对文件或目录执行原子操作
    /// </summary>
    /// <param name="delegate">对文件或目录进行原子操作的委托，
    /// 参数就是这个<see cref="IIO"/>对象本身</param>
    void Atomic(Action<IIO> @delegate)
        => Atomic<object?>(x =>
        {
            @delegate(x);
            return null;
        });
    #endregion
    #endregion
    #endregion
    #region 有关事件
    #region 说明文档
    /*实现这些事件时，请遵循以下规范：
      #由于仅在少数情况下需要对文件目录进行监听，
      因此，这些事件仅在第一次被注册时初始化

      #在文件或目录被删除，移动时，自动停止监听*/
    #endregion
    #region 停止监听所有事件
    /// <summary>
    /// 停止监听文件或目录，
    /// 所有有关事件都将不再触发
    /// </summary>
    void WatcherStop();
    #endregion
    #region 在被删除时触发
    /// <summary>
    /// 当该文件，或该目录的子文件目录被删除后执行这个事件，
    /// 事件的参数就是被删除的路径
    /// </summary>
    event Action<string>? OnDelete;
    #endregion
    #region 在被修改时触发
    /// <summary>
    /// 当该文件，或该目录下的子文件被修改后执行这个事件，
    /// 修改的项目包括名称，写入，最后访问时间，
    /// 事件的参数就是被修改的文件
    /// </summary>
    event Action<IFile>? OnFileChange;

    /*说明文档
      由于修改项目包括最后访问时间，
      所以它同样可以监听文件被访问，但是这种监听是不完美的，体现在：
      
      #最后访问时间似乎并不是指的文件被打开的时间，
      它有这方面的成分，但是具体含义不明，如果有人精通Win32，麻烦向我赐教下
    
      #出于性能考虑，Windows默认不会自动更新最后访问时间，
      如果想要开启这个功能，需要在CMD中输入以下命令：
     
      fsutil behavior set disablelastaccess 0*/
    #endregion
    #endregion
}
