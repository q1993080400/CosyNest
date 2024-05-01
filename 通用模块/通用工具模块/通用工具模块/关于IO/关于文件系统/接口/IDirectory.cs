using System.IOFrancis.BaseFileSystem;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以视为一个目录
/// </summary>
public interface IDirectory : IIO, IDirectoryBase
{
    #region 复制子文件目录
    /// <summary>
    /// 将子文件目录复制到其他位置，
    /// 但是不复制这个目录本身
    /// </summary>
    /// <param name="father">新的父目录</param>
    void CopySon(IDirectory father)
    {
        foreach (var item in Son)
        {
            item.Copy(father);
        }
    }
    #endregion
    #region 搜索文件或目录
    #region 模式匹配
    /// <summary>
    /// 搜索此目录下的所有文件或目录，
    /// 并返回结果
    /// </summary>
    /// <typeparam name="IO">返回值类型，它可以是文件或目录</typeparam>
    /// <param name="condition">搜索条件，以文件或目录的名称为准，
    /// 它可以使用通配符，*匹配零个或多个字符，?匹配零个或一个字符</param>
    /// <param name="isRecursion">如果这个值为<see langword="true"/>，
    /// 则递归搜索所有子文件或目录，否则仅搜索直接子文件或目录</param>
    /// <returns></returns>
    IEnumerable<IO> Search<IO>(string condition, bool isRecursion = false)
        where IO : IIO;
    #endregion
    #region 按照名称搜索
    /// <summary>
    /// 按照名称搜索文件或目录，
    /// 如果没有找到，返回<see langword="null"/>
    /// </summary>
    /// <param name="name">文件或目录的名称</param>
    /// <returns></returns>
    /// <inheritdoc cref="Search{IO}(string, bool)"/>
    IO? SearchFromName<IO>(string name, bool isRecursion = false)
        where IO : IIO
        => Search<IO>(name, isRecursion).SingleOrDefaultSecure();
    #endregion
    #endregion
}
