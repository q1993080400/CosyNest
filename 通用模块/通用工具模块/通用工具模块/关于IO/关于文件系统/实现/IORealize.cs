using System.ComponentModel;
using System.IOFrancis.BaseFileSystem;
using System.MathFrancis;
using System.MathFrancis.Tree;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个类型是文件和目录接口实现的共同基类
/// </summary>
abstract class IORealize : IIO
{
#pragma warning disable CA2208, CA1816

    #region 获取文件目录对象
    /// <summary>
    /// 获取封装的文件目录对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected abstract FileSystemInfo PackFS { get; }
    #endregion
    #region 关于文件系统树
    #region 返回根节点
    INode INode.Ancestors
        => CreateIO.FileSystem;
    #endregion
    #region 获取父目录
    #region INode版本
    INode? INode.Father
        => Father ?? (INode)Drive;
    #endregion
    #region IIO版本
    public IDirectoryBase? Father
    {
        get
        {
            var father = IO.Path.GetDirectoryName(Path)!;
            return father.IsVoid() ? null : CreateIO.Directory(father);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value is not IDirectory)
                throw new ArgumentException($"{value.GetType()}不是{nameof(IDirectory)}，无法写入");
            Path = IO.Path.Combine(value.Path, this.To<IIO>().NameFull);
        }
    }
    #endregion
    #endregion
    #region 枚举直接子文件或子目录
    public abstract IEnumerable<INode> Son { get; }
    #endregion
    #region 获取文件或目录的驱动器
    public IDrive Drive
        => CreateIO.FileSystem[Path[0].ToString()]!;
    #endregion
    #endregion
    #region 文件或目录的信息
    #region 完整路径
    private string PathField;

    public string Path
    {
        get => PathField;
        set
        {
            File.Delete(value);
            PackFS.To<dynamic>().MoveTo(value);
            PathField = value;
            WatcherStop();
        }
    }
    #endregion
    #region 读写完整名称
    public string NameFull
    {
        get => IO.Path.GetFileName(Path);
        set
        {
            var father = IO.Path.GetDirectoryName(Path)!;
            Path = IO.Path.Combine(father, value ?? throw new ArgumentNullException(nameof(NameFull)));
        }
    }
    #endregion
    #region 获取文件或目录的大小
    public abstract IUnit<IUTStorage> Size { get; }
    #endregion
    #region 是否隐藏文件或目录
    public bool Hide
    {
        get => PackFS.Attributes.HasFlag(FileAttributes.Hidden);
        set => PackFS.Attributes = value ?
            PackFS.Attributes | FileAttributes.Hidden :
            PackFS.Attributes.RemoveFlag(FileAttributes.Hidden);
    }
    #endregion
    #region 文件或目录的创建时间
    public DateTimeOffset DateCreate
    {
        get => new(PackFS.CreationTime, PackFS.CreationTime - PackFS.CreationTimeUtc);
        set => PackFS.CreationTimeUtc = value.UtcDateTime;
    }
    #endregion
    #endregion
    #region 对文件或目录的操作
    #region 删除
    public abstract void Delete();
    #endregion
    #region 复制
    public abstract IIO Copy(IDirectory? target, string? newName = null, Func<string, int, string>? rename = null);
    #endregion
    #region 对文件或目录执行原子操作
    public Ret Atomic<Ret>(Func<IIO, Ret> @delegate)
    {
        IIO io = this;
        var backup = Copy(io.Father!, Guid.NewGuid().ToString());
        Ret r;
        try
        {
            r = @delegate(this);                                  //对原始文件，而不是备份执行操作
        }
        catch (Exception)
        {
            Delete();                                           //如果出现异常，则删除自身，并将备份转正
            backup.Path = Path;
            throw;
        }
        backup.Delete();                        //如果没有出现异常，则删除备份
        return r;
    }
    #endregion
    #endregion
    #region 有关事件
    #region 有关监听
    #region 获取监听对象
    private FileSystemWatcher? WatcherField;

    /// <summary>
    /// 获取一个监听对象，
    /// 它可以为文件目录发生更改的事件提供支持
    /// </summary>
    protected FileSystemWatcher Watcher
    {
        get
        {
            if (WatcherField is null)
            {
                WatcherField = this is IFile f ?
                new(f.Father!.Path, f.NameFull) : new(Path)
                {
                    IncludeSubdirectories = true
                };
                Watcher.NotifyFilter =
                    NotifyFilters.LastWrite | NotifyFilters.LastAccess |
                    NotifyFilters.DirectoryName | NotifyFilters.FileName;
                WatcherField.Error += (_, e) =>
                {
                    var ex = e.GetException();
                    if (ex is Win32Exception { ErrorCode: -2147467259 } && this is IDirectory)      //当引发这个异常时，表示文件或目录不存在
                        OnDeleteMirroring?.Invoke(Path);
                    else throw ex;
                };
                WatcherField.EnableRaisingEvents = true;
                GC.ReRegisterForFinalize(this);
            }
            return WatcherField;
        }
    }
    #endregion
    #region 停止监听所有事件
    public void WatcherStop()
    {
        if (WatcherField is { })
        {
            WatcherField.Dispose();
            GC.SuppressFinalize(this);
        }
        OnDeleteMirroring = null;
    }
    #endregion
    #endregion
    #region 在被删除时触发
    #region 说明文档
    /*问：本事件似乎会把注册的委托复制两份，
      分别注册到两个不同的事件中，请问为什么要执行这种怪异的行为？
      答：这是由于FileSystemWatcher的奇怪行为而导致的，
      假设路径p是一个目录，那么在它被删除时，监听它的FileSystemWatcher会发生以下行为：

      1.如果直接监听p，则会引发一个异常，而不是触发Deleted事件
      2.如果监听p的父目录f，然后将p作为文件监听，
      则只会监听p和p的子目录，不会递归监听子目录的子目录

      这与OnDelete标准的行为不符，因此需要声明一个OnDelete的镜像OnDeleteMirroring，
      当p被删除的时候，不是抛出异常，而是调用OnDeleteMirroring，这样一来，
      在外界看来似乎是OnDelete被触发了，具有一致的行为

      重要说明：该设计存在以下隐患：
      当被监听的目录被删除时，所引发的异常信息是“拒绝访问”，
      但是作者对Win32异常不了解，不知道该异常（状态码-2147467259）会不会在没有权限的时候也会触发，
      如果会，这会导致在权限不足时，错误地触发删除事件*/
    #endregion
    #region 正式事件
    private Action<string>? OnDeleteMirroring { get; set; }

    public event Action<string>? OnDelete
    {
        add
        {
            Watcher.Deleted += (_, e) => value!(e.FullPath);
            OnDeleteMirroring += value;
        }
        remove => throw CreateException.NotSupportedEventRemove;
    }
    #endregion
    #endregion
    #region 在被修改时触发
    public event Action<IFile>? OnFileChange
    {
        add => Watcher.Changed += (_, e) =>
        {
            if (File.Exists(e.FullPath))
                value!(CreateIO.File(e.FullPath));
        };
        remove => throw CreateException.NotSupportedEventRemove;
    }
    #endregion
    #endregion
    #region 重写的方法
    #region 重写的GetHashCode
    public override int GetHashCode()
        => Path.GetHashCode();
    #endregion
    #region 重写Equals
    public override bool Equals(object? obj)
        => obj is IIO i &&
        Path == i.Path;
    #endregion
    #region 重写ToString
    public override string ToString()
        => Path;
    #endregion
    #endregion
    #region 析构函数
    ~IORealize()
    {
        WatcherStop();
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的路径初始化对象
    /// </summary>
    /// <param name="path">文件或目录所在的路径</param>
    public IORealize(string path)
    {
        PathField = path;
        GC.SuppressFinalize(this);
    }
    #endregion
}
