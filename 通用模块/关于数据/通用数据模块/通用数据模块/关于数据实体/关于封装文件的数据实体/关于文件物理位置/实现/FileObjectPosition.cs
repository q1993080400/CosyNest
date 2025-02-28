using System.IOFrancis.FileSystem;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IFileObjectPosition"/>的实现，
/// 可以视为一个文件物理位置，
/// 并且它具有一个可以关联数据库对象的ID
/// </summary>
sealed record FileObjectPosition : IFileObjectPosition
{
    #region ID
    public required Guid ID { get; init; }
    #endregion
    #region 本体路径
    public required FilePathInfo Path { get; init; }
    #endregion
    #region 封面路径
    public required FilePathInfo? CoverPath { get; init; }
    #endregion
}
