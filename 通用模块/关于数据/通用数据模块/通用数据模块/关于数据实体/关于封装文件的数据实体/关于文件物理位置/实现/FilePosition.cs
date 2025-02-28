using System.IOFrancis.FileSystem;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IFilePosition"/>的实现，
/// 可以视为一个文件物理位置
/// </summary>
sealed record class FilePosition : IFilePosition
{
    #region 本体路径
    public required FilePathInfo Path { get; init; }
    #endregion
    #region 封面路径
    public required FilePathInfo? CoverPath { get; init; }
    #endregion
}
