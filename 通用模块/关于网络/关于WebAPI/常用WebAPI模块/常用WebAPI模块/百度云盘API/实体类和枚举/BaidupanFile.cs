using System.Design.Direct;
using System.IOFrancis.FileSystem;
using System.NetFrancis.Http;

namespace System.NetFrancis.Api.Baidupan;

/// <summary>
/// 这个类型表示百度云盘上的一个文件
/// </summary>
public sealed record BaidupanFile : BaidupanFD
{
    #region 公开成员
    #region 文件下载地址
    /// <summary>
    /// 获取文件下载地址，
    /// 它的参数中已经加入了身份验证令牌，
    /// 可以通过这个地址直接下载
    /// </summary>
    public string Download { get; }
    #endregion
    #region 下载文件
    /// <summary>
    /// 下载这个百度云文件
    /// </summary>
    /// <param name="path">保存文件的地址</param>
    /// <param name="cancellationToken">一个用于取消操作的令牌</param>
    /// <returns></returns>
    public async Task DownloadFile(PathText path, CancellationToken cancellationToken = default)
    {
        var read = await HttpClientProvide().RequestDownload(Download, cancellationToken: cancellationToken);
        await read.SaveToFile(path, cancellation: cancellationToken);
    }
    #endregion
    #endregion
    #region 内部成员
    #region Http工厂
    /// <summary>
    /// 获取提供Http客户端的工厂
    /// </summary>
    private Func<IHttpClient> HttpClientProvide { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="fileData">文件数据，它包含文件的基本信息</param>
    /// <param name="infoData">元数据数据，它包含文件的扩展信息，如下载地址等</param>
    /// <param name="accessToken">百度云盘访问令牌</param>
    /// <param name="httpClientProvide">用来提供Http客户端的工厂</param>
    internal BaidupanFile(IDirect fileData, IDirect infoData, string accessToken, Func<IHttpClient> httpClientProvide)
        : base(fileData)
    {
        this.HttpClientProvide = httpClientProvide;
        Download = infoData.GetValue<string>("dlink")! + $"&access_token={accessToken}";
    }
    #endregion 
}
