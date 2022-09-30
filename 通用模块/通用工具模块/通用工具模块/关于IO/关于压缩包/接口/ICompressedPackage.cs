using System.IOFrancis.FileSystem;
using System.Maths.Tree;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个压缩包
/// </summary>
public interface ICompressedPackage : IFromIO, INode
{
    #region 静态属性
    #region 返回Zip文件类型
    /// <summary>
    /// 返回Zip压缩文件的类型
    /// </summary>
    public static IFileType FileTypeZip { get; }
        = CreateIO.FileType("zip压缩文件", "zip");
    #endregion
    #endregion
    #region 返回压缩包根目录
    /// <summary>
    /// 返回压缩包的根目录
    /// </summary>
    ICompressionDirectory RootDirectory { get; }
    #endregion
}
