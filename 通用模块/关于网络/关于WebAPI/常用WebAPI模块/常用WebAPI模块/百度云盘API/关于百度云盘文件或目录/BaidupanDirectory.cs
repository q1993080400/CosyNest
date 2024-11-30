using System.Design.Direct;

namespace System.NetFrancis.Api;

/// <summary>
/// 这个类型表示百度云盘上的一个目录
/// </summary>
public sealed record BaidupanDirectory : BaidupanFD
{
    #region 静态方法：创建对象
    /// <summary>
    /// 从百度网盘API响应中读取信息，
    /// 并创建对象
    /// </summary>
    /// <param name="fileData">文件数据，它包含文件的基本信息</param>
    /// <returns></returns>
    public static BaidupanDirectory Create(IDirect fileData)
        => new()
        {
            ID = fileData.GetValue<long>("fs_id"),
            Path = fileData.GetValue<string>("path")!,
        };
    #endregion
}
