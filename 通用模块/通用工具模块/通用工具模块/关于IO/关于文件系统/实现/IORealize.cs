﻿using System.IOFrancis.BaseFileSystem;
using System.MathFrancis.Tree;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个类型是文件和目录接口实现的共同基类
/// </summary>
/// <remarks>
/// 使用指定的路径初始化对象
/// </remarks>
/// <param name="path">文件或目录所在的路径</param>
abstract class IORealize(string path) : IIO
{
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
    public string Path
    {
        get => path;
        set
        {
            if (value == path)
                return;
            var father = IO.Path.GetDirectoryName(value);
            if (father.IsVoid())
                throw new NotSupportedException($"新的路径{value}的父目录无效");
            var newName = IO.Path.GetFileName(value);
            Copy(father, newName);
            Delete();
            path = value;
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
    #region 传入目录
    public abstract IIO Copy(IDirectory? target, string? newName = null, Func<string, int, string>? rename = null);
    #endregion
    #region 传入目标路径
    public IIO Copy(string? target, string? newName = null, Func<string, int, string>? rename = null)
         => Copy(CreateIO.Directory(target ?? Father?.Path ?? Path, false), newName, rename);
    #endregion
    #endregion
    #endregion
    #region 用来监视文件系统的对象
    public IFileSystemWatcher FileSystemWatcher
        => throw new NotImplementedException("不支持这个API");
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
}
