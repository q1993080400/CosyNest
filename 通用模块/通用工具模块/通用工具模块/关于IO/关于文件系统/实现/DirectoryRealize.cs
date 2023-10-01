﻿using System.IOFrancis.BaseFileSystem;
using System.MathFrancis;
using System.MathFrancis.Tree;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个类型是<see cref="IDirectory"/>的实现，
/// 可以被视为一个目录
/// </summary>
sealed class DirectoryRealize : IORealize, IDirectory
{
    #region 封装的对象
    #region 获取目录对象
    protected override DirectoryInfo PackFS => new(Path);
    #endregion
    #region 封装本对象的接口形式
    /// <summary>
    /// 返回本对象的接口形式，
    /// 它使本对象的成员可以访问显式接口实现的成员
    /// </summary>
    private IDirectory Directory
        => this;
    #endregion
    #endregion
    #region 关于目录的信息
    #region 枚举子目录
    public override IEnumerable<INode> Son
        => PackFS.EnumerateFileSystemInfos().
        Where(x =>
        {
            #region 说明文档
            /*问：这个地方为什么有一个奇怪的过滤？
              答：这是因为EnumerateFileSystemInfos的逻辑非常的古怪，
              如果PackFS是一个目录，而且以\结尾，
              则EnumerateFileSystemInfos的第一项就是这个目录本身，这会引起无限递归Bug
              而且更可气的是，这个目录的FullName以一个隐形的字符开头，作者排查它花费了不少精力*/
            #endregion
            if (x is DirectoryInfo { FullName: var path })
            {
                path = ToolPath.Trim(path);
                return path != Path && path != (Path + "\\");
            }
            return true;
        }).Select(x => x switch
         {
             FileInfo a => (INode)new FileRealize(a.FullName),
             DirectoryInfo a => new DirectoryRealize(a.FullName),
             _ => throw new Exception("无法识别的类型")
         });
    #endregion
    #region 获取目录的大小
    public override IUnit<IUTStorage> Size
        => this.To<INode>().Father is IDrive d ?
        d.SizeUsed :
        Directory.Son.Select(x => x.Size).Sum();
    #endregion
    #endregion
    #region 关于对目录的操作
    #region 复制
    public override IIO Copy(IDirectory? target, string? newName = null, Func<string, int, string>? rename = null)
    {
        target ??= Directory.Father ?? throw new ArgumentException("父目录不能为null");
        newName ??= Directory.NameFull;
        if (rename is { })
            newName = ToolPath.Distinct(target, newName, rename);
        var newPosition = new DirectoryRealize(IO.Path.Combine(target.Path, newName), false);                //如果父目录不存在，则会自动创建
        Directory.Son.ForEach(x => x.Copy(newPosition));
        return newPosition;
    }
    #endregion
    #region 创建文件或目录
    public IO Create<IO>(string? name = null)
        where IO : IIOBase
    {
        var type = typeof(IO);
        var newName = IDirectoryBase.GetUniqueName(this, name);
        var path = System.IO.Path.Combine(Path, newName);
        if (type == typeof(IFile) || type == typeof(IFileBase))
            return (IO)CreateIO.File(path, false);
        if (type == typeof(IDirectory) || type == typeof(IDirectoryBase))
            return (IO)CreateIO.Directory(path, false);
        throw new NotSupportedException($"泛型参数必须是{nameof(IFile)}或{nameof(IDirectory)}，但是传入了{type}");
    }
    #endregion
    #region 删除目录
    public override void Delete()
    {
        PackFS.Delete(true);
        WatcherStop();
    }
    #endregion
    #endregion
    #region 尝试直达文件或目录
    public IO? Direct<IO>(string relativelyPath)
        where IO : IIOBase
    {
        var path = System.IO.Path.Combine(Path, relativelyPath);
        return CreateIO.IO(path).To<IO>(false);
    }
    #endregion
    #region 创建文件或目录时触发的事件
    public event Action<IIO>? OnCreate
    {
        add => Watcher.Created +=
            (_, e) => value!(CreateIO.IO(e.FullPath)!);
        remove => throw CreateException.NotSupportedEventRemove;
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 用指定的路径初始化目录对象
    /// </summary>
    /// <param name="path">指定的路径</param>
    /// <param name="checkExist">在路径不存在的时候，如果这个值为<see langword="true"/>，会抛出一个异常，
    /// 如果为<see langword="false"/>，则不会抛出异常，而是会创建这个目录</param>
    public DirectoryRealize(PathText path, bool checkExist = true)
        : base(path)
    {
        if (!PackFS.Exists)
        {
            if (checkExist)
                throw IOExceptionFrancis.BecauseExist(path.Path ?? "null");
            PackFS.Create();
        }
    }
    #endregion
}
