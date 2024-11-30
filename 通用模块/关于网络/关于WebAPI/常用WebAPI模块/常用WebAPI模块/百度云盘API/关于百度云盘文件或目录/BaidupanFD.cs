using System.IOFrancis.FileSystem;
using System.Text.Json.Serialization;

namespace System.NetFrancis.Api;

/// <summary>
/// 这个类型表示百度云盘中的一个文件或目录
/// </summary>
[JsonDerivedType(typeof(BaidupanFile))]
[JsonDerivedType(typeof(BaidupanDirectory))]
public abstract record BaidupanFD
{
    #region 文件路径
    /// <summary>
    /// 获取文件路径
    /// </summary>
    public required string Path { get; init; }
    #endregion
    #region 文件信息
    /// <summary>
    /// 获取一个元组，它的项分别是文件或目录的简称，
    /// 扩展名（如果存在，不带点号），以及全称
    /// </summary>
    public (string Simple, string? Extended, string FullName) Info
        => ToolPath.SplitFilePath(Path);
    #endregion
    #region 文件ID
    /// <summary>
    /// 获取文件的ID
    /// </summary>
    public required long ID { get; init; }
    #endregion
    #region 重写ToString
    public sealed override string ToString()
        => Path;
    #endregion
}
