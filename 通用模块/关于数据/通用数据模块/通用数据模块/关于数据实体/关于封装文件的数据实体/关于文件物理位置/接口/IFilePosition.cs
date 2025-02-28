using System.IOFrancis.FileSystem;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个文件的物理位置，
/// 它指示这个文件实际存储的地方
/// </summary>
public interface IFilePosition
{
    #region 本体路径
    /// <summary>
    /// 指示文件的本体路径
    /// </summary>
    FilePathInfo Path { get; }
    #endregion
    #region 封面路径
    /// <summary>
    /// 指示文件的封面路径
    /// </summary>
    FilePathInfo? CoverPath { get; }
    #endregion
}
