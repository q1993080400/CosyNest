using System.IOFrancis.BaseFileSystem;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 所有实现这个接口的类型，都可以视为一个文件
/// </summary>
public interface IFile : IIO, IFileBase
{
    #region 复制文件
    #region 传入目录
    /// <summary>
    /// 复制文件
    /// </summary>
    /// <param name="newSimple">新文件的名称，不带扩展名，如果为<see langword="null"/>，代表不修改</param>
    /// <param name="newExtension">新文件的扩展名，如果为<see langword="null"/>，代表不修改</param>
    /// <returns>复制后的新文件</returns>
    /// <inheritdoc cref="IIO.Copy(IDirectory?, string?, Func{string, int, string}?)"/>
    IFile Copy(IDirectory? target, string? newSimple, string? newExtension, Func<string, int, string>? rename = null)
    {
        if ((newSimple, newExtension) is (null, null))
            return (IFile)Copy(target, null, rename);
        var fullName = new FileNameInfo(newSimple ?? NameSimple, newExtension ?? NameExtension).FullName;
        return (IFile)Copy(target, fullName, rename);
    }
    #endregion
    #region 传入目录路径
    /// <inheritdoc cref="Copy(IDirectory?, string?, string?, Func{string, int, string}?)"/>
    IFile Copy(string? target, string? newSimple, string? newExtension, Func<string, int, string>? rename = null)
    {
        if (target is null)
            return Copy((IDirectory?)null, newSimple, newExtension, rename);
        var directory = CreateIO.Directory(Father!.Path);
        return Copy(directory, newSimple, newExtension, rename);
    }
    #endregion
    #endregion
}
