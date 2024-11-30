using System.Design.Direct;
using System.NetFrancis.Http;

namespace System.NetFrancis.Api;

/// <summary>
/// 这个类型表示百度云盘上的一个文件
/// </summary>
public sealed record BaidupanFile : BaidupanFD
{
    #region 文件下载地址
    /// <summary>
    /// 获取文件下载地址，
    /// 它的参数中已经加入了身份验证令牌，
    /// 可以通过这个地址直接下载
    /// </summary>
    public required string Download { get; init; }
    #endregion
    #region 文件类型
    /// <summary>
    /// 获取文件类型
    /// </summary>
    public required BaidupanFileType Type { get; init; }
    #endregion
    #region 下载文件
    /// <summary>
    /// 下载这个百度云文件
    /// </summary>
    /// <param name="path">保存文件的地址</param>
    /// <param name="httpClient">用来发起Http请求的对象</param>
    /// <param name="cancellationToken">一个用于取消操作的令牌</param>
    /// <returns></returns>
    public async Task DownloadFile(string path, IHttpClient httpClient, CancellationToken cancellationToken = default)
    {
        using var read = await httpClient.RequestStream(Download, true, cancellationToken);
        await read.SaveToFile(path, cancellationToken);
    }
    #endregion
    #region 静态方法：创建对象
    /// <summary>
    /// 从百度网盘API响应中读取信息，
    /// 并创建对象
    /// </summary>
    /// <param name="fileData">文件数据，它包含文件的基本信息</param>
    /// <param name="infoData">元数据数据，它包含文件的扩展信息，如下载地址等</param>
    /// <param name="accessToken">百度云盘访问令牌</param>
    /// <returns></returns>
    public static BaidupanFile Create(IDirect fileData, IDirect infoData, string accessToken)
        => new()
        {
            ID = fileData.GetValue<long>("fs_id"),
            Path = fileData.GetValue<string>("path")!,
            Download = infoData.GetValue<string>("dlink")! + $"&access_token={accessToken}",
            Type = fileData.GetValue<BaidupanFileType>("category"),
        };
    #endregion
}
