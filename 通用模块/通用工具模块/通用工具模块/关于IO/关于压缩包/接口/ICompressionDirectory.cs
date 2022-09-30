using System.IOFrancis.BaseFileSystem;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个压缩包中的目录
/// </summary>
public interface ICompressionDirectory : ICompressionItem, IDirectoryBase
{
    #region 添加文件或目录
    /// <summary>
    /// 将文件或目录添加到这个压缩包，
    /// 并返回新添加的项
    /// </summary>
    /// <param name="item">待添加的文件或目录</param>
    /// <returns></returns>
    ICompressionItem Add(IIO item);
    #endregion
    #region 添加管道
    /// <summary>
    /// 读取一个管道的内容，并将它作为文件添加到压缩包，
    /// 然后返回新添加的压缩文件
    /// </summary>
    /// <param name="name">文件的全名，含扩展名</param>
    /// <param name="read">用来读取内容的管道</param>
    /// <returns></returns>
    ICompressionFile Add(string name, IBitRead read)
    {
        var file = Create<ICompressionFile>(name);
        using var write = file.GetBitPipe();
        read.CopyTo(write.Write).Wait();
        return file;
    }
    #endregion
}
