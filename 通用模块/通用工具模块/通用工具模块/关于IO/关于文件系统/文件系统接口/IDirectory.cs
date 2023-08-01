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
    #region 创建文件或目录时触发的事件
    /// <summary>
    /// 当在该目录下创建文件或目录时，触发这个事件，
    /// 事件的参数就是新创建的文件或目录
    /// </summary>
    event Action<IIO>? OnCreate;
    #endregion
}
