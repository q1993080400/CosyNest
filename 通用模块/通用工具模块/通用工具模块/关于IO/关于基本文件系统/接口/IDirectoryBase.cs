using System.IOFrancis.FileSystem;

namespace System.IOFrancis.BaseFileSystem;

/// <summary>
/// 该接口为基本文件系统中的目录提供统一抽象
/// </summary>
public interface IDirectoryBase : IIOBase
{
    #region 清除目录
    /// <summary>
    /// 清除目录中的所有子文件和目录，
    /// 但是不将目录本身删除
    /// </summary>
    void Clear()
        => Son.ForEach(x => x.Delete());
    #endregion
    #region 搜索文件或目录
    /// <summary>
    /// 从目录下搜索具有指定名称的文件或目录
    /// </summary>
    /// <typeparam name="IO">要搜索的文件或目录的类型</typeparam>
    /// <param name="name">要搜索的文件或目录全名</param>
    /// <param name="isRecursive">如果这个值为<see langword="true"/>，
    /// 则执行递归搜索，否则只搜索直接子节点</param>
    /// <returns>搜索到的文件或目录，如果没有找到，则返回默认值</returns>
    IO? Find<IO>(string name, bool isRecursive = false)
        where IO : IIOBase
        => (isRecursive ? SonAll : Son).OfType<IO>().FirstOrDefault(x => x.NameFull == name);
    #endregion
    #region 直达子文件或目录
    /// <summary>
    /// 尝试直达子文件或目录
    /// </summary>
    /// <typeparam name="IO">要直达的文件或目录的类型</typeparam>
    /// <param name="relativelyPath">子文件或目录的相对路径，
    /// 它相对于本目录</param>
    /// <returns>直达结果，如果指定的地方不存在文件或目录，则返回默认值</returns>
    IO? Direct<IO>(string relativelyPath)
        where IO : IIOBase;
    #endregion
    #region 创建文件或目录
    #region 辅助方法：获取不重复的名称
    /// <summary>
    /// 实现<see cref="Create{IO}(string?)"/>的辅助方法，获取一个在目录下绝对不会重复的名称
    /// </summary>
    /// <param name="directory">要获取不重复名称的目录</param>
    /// <inheritdoc cref="Create{IO}(string?)"/>
    protected static string GetUniqueName(IDirectoryBase directory, string? name)
    {
        if (name is null)
            return Guid.NewGuid().ToString();
        var names = directory.Son.Cast<IIOBase>().Select(x => x.NameFull);
        return names.Distinct(name, (n, i) =>
        {
            var (simple, extended, _) = ToolPath.SplitPathFile(n, true);
            return $"{simple}({i}){extended}";
        });
    }
    #endregion
    #region 创建文件或目录
    /// <summary>
    /// 创建一个文件或目录，并返回
    /// </summary>
    /// <typeparam name="IO">要创建的文件或目录的全名</typeparam>
    /// <param name="name">新文件或目录的名称，
    /// 如果为<see langword="null"/>，则给予一个不重复的随机名称，
    /// 如果不为<see langword="null"/>但是名称重复，则自动对名称进行重命名</param>
    /// <returns></returns>
    IO Create<IO>(string? name = null)
        where IO : IIOBase;
    #endregion
    #region 创建文件
    /// <summary>
    /// 创建一个文件
    /// </summary>
    /// <typeparam name="File">要创建的文件的类型</typeparam>
    /// <param name="nameSimple">文件的简称，不带扩展名</param>
    /// <param name="nameExtended">文件的扩展名，不带点号</param>
    /// <returns>新创建的文件，如果文件的名称不指定，则分配一个不重复的随机名称，
    /// 如果指定，但是名称重复，则自动对名称进行重命名</returns>
    File CreateFile<File>(string? nameSimple = null, string? nameExtended = null)
        where File : IFileBase
    {
        var name = nameSimple ?? Guid.NewGuid().ToString() + (nameExtended is null ? null : $".{nameExtended}");
        return Create<File>(name);
    }
    #endregion
    #endregion
}
