using System.IOFrancis.BaseFileSystem;
using System.IOFrancis.Bit;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个压缩包中的文件
/// </summary>
public interface ICompressionFile : ICompressionItem, IFileBase
{
    #region 返回用于解压缩的管道
    /// <summary>
    /// 返回一个用于解压缩的管道
    /// </summary>
    /// <returns></returns>
    IBitRead Decompress();
    #endregion
}
