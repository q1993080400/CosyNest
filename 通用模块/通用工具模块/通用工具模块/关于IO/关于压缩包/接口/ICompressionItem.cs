using System.IOFrancis.BaseFileSystem;
using System.IOFrancis.FileSystem;
using System.MathFrancis.Tree;

namespace System.IOFrancis.Compressed;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个压缩包中的项，
/// 它可以是文件或目录
/// </summary>
public interface ICompressionItem : IIOBase
{
    #region 返回压缩包
    /// <summary>
    /// 返回这个项所在的压缩包，
    /// 它是所有压缩包项的根节点
    /// </summary>
    new ICompressedPackage Ancestors
         => this.To<INode>().Ancestors.To<ICompressedPackage>();
    #endregion
    #region 解压到指定目录
    /// <summary>
    /// 解压到指定目录
    /// </summary>
    /// <param name="path">解压的目标目录</param>
    /// <param name="cover">如果为<see langword="true"/>，
    /// 解压时会覆盖旧文件</param>
    /// <returns></returns>
    Task Decompress(PathText path, bool cover = true);
    #endregion
}
