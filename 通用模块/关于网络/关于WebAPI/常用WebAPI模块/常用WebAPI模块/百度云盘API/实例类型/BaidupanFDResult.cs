namespace System.NetFrancis.Api;

/// <summary>
/// 这个记录是获取百度云盘文件或目录的结果
/// </summary>
public sealed record BaidupanFDResult
{
    #region 文件或目录的列表
    /// <summary>
    /// 这个字典按路径索引百度云盘文件或目录的列表
    /// </summary>
    public required IReadOnlyDictionary<string, BaidupanFD> Element { get; init; }
    #endregion
    #region 文件或目录的路径
    /// <summary>
    /// 获取文件或目录的路径
    /// </summary>
    public required string Path { get; init; }
    #endregion
    #region 单一文件
    /// <summary>
    /// 当<see cref="Path"/>指向一个文件时，
    /// <see cref="Element"/>返回和这个文件同级的所有文件或目录，
    /// 本属性则返回这个文件本身，
    /// 如果<see cref="Path"/>指向一个目录，
    /// 则本属性为<see langword="null"/>
    /// </summary>
    public BaidupanFile? SingleFile
        => Element.GetValueOrDefault(Path) as BaidupanFile;
    #endregion
}
