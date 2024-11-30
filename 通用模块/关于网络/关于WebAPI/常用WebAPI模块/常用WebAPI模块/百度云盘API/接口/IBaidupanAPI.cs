namespace System.NetFrancis.Api;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个百度网盘API
/// </summary>
public interface IBaidupanAPI
{
    #region 获取文件或目录列表
    /// <summary>
    /// 获取百度云文件或目录的列表
    /// </summary>
    /// <param name="dir">指定获取哪个目录的文件，
    /// 如果这个路径指向一个文件，则返回它父目录的所有文件（包括它本身）</param>
    /// <returns></returns>
    Task<BaidupanFDResult> GetFD(string dir = "/");
    #endregion
    #region 搜索文件或目录
    /// <summary>
    /// 搜索文件或者目录
    /// </summary>
    /// <param name="search">要搜索的文件或目录的名字</param>
    /// <param name="dir">指示应该在什么目录下搜索，如果不指定，默认为搜索全部目录</param>
    /// <returns></returns>
    Task<BaidupanFD[]> SearchFD(string search, string dir = "/");
    #endregion
}
