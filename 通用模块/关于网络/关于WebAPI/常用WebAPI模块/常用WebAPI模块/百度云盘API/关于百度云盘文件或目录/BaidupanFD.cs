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
    /// 获取这个文件的信息
    /// </summary>
    public FileNameInfo Info
        => FileNameInfo.FromPath(Path);
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
