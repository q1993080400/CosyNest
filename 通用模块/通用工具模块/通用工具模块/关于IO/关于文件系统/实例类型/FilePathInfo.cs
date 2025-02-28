namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个记录封装了文件的完整路径，
/// 以及关于文件名的信息
/// </summary>
public sealed record FilePathInfo
{
    #region 路径
    /// <summary>
    /// 获取文件的路径
    /// </summary>
    public string Path { get; }
    #endregion
    #region 文件名信息
    /// <summary>
    /// 获取关于文件名的信息
    /// </summary>
    public FileNameInfo FileName { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="path">文件的完整路径</param>
    public FilePathInfo(string path)
    {
        this.Path = path;
        FileName = FileNameInfo.FromPath(path);
    }
    #endregion
}
