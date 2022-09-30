using System.IOFrancis.BaseFileSystem;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以视为一个目录
/// </summary>
public interface IDirectory : IIO, IDirectoryBase
{
    #region 创建文件或目录时触发的事件
    /// <summary>
    /// 当在该目录下创建文件或目录时，触发这个事件，
    /// 事件的参数就是新创建的文件或目录
    /// </summary>
    event Action<IIO>? OnCreate;
    #endregion
}
