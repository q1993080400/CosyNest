using System.Design.Direct;
using System.IOFrancis.FileSystem;

namespace System.NetFrancis.Api.Baidupan;

/// <summary>
/// 这个类型表示百度云盘中的一个文件或目录
/// </summary>
public abstract record BaidupanFD
{
    #region 公开成员
    #region 文件路径
    /// <summary>
    /// 获取文件路径
    /// </summary>
    public string Path { get; }
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
    public long ID { get; }
    #endregion
    #region 重写ToString
    public sealed override string ToString()
        => Path;
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的数据初始化对象
    /// </summary>
    /// <param name="data">包含对象字段的弱类型数据</param>
    internal BaidupanFD(IDirect data)
    {
        Path = data.GetValue<string>("path")!;
        ID = data.GetValue<long>("fs_id");
    }
    #endregion
}
