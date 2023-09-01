using System.Design.Direct;
using System.NetFrancis.Http;
using System.IOFrancis.Bit;

namespace System.NetFrancis.Api.Bilibili;

/// <summary>
/// 本类型封装了哔哩哔哩API
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
public sealed class BilibiliAPI(Func<IHttpClient>? httpClientProvide = null) : WebApi(httpClientProvide)
{
    #region 通过视频地址获取AID和CID
    /// <summary>
    /// 通过视频地址获取aid和cid
    /// </summary>
    /// <param name="uri">视频地址</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    public async Task<(string AID, string CID)> GetID(string uri, CancellationToken cancellationToken = default)
    {
        var match =/*language=regex*/"video/(?<bv>BV[^/?]+)".Op().Regex().MatcheSingle(uri) ?? throw new ArgumentException(@"这个地址不是B站视频地址");
        var json = await (await HttpClientProvide().Request($"http://api.bilibili.com/x/web-interface/view?bvid={match["bv"].Match}", cancellationToken: cancellationToken)).Content.ToObject();
        var data = json!.GetValue<IDirect>("data")!;
        return (data["aid"]!.ToString()!, data["cid"]!.ToString()!);
    }
    #endregion
    #region 下载B站视频
    #region 传入CID和AID
    /// <summary>
    /// 下载B站视频
    /// </summary>
    /// <param name="aid">视频的aid，如果是下载番剧，可以不要</param>
    /// <param name="cid">视频的cid</param>
    /// <param name="clear">视频的清晰度</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns>一个元组，它的项分别是下载视频的管道和下载音频的管道</returns>
    public async Task<(IBitRead Video, IBitRead Audio)> DownloadVideoFromID(string? aid, string cid,
        BiliBiliVideoClear clear = BiliBiliVideoClear.P1080,
        CancellationToken cancellationToken = default)
    {
        var uri = aid is null ?
            $"https://api.bilibili.com/pgc/player/web/playurl?fnval=80&cid={cid}" :
            $"https://api.bilibili.com/x/player/playurl?fnval=80&avid={aid}&cid={cid}";
        var request = new HttpRequestRecording(uri)
        {
            Header = new(new Dictionary<string, IEnumerable<string>>()
            {
                ["Referer"] = new[] { "https://www.bilibili.com" },
                ["User-Agent"] = new[] { "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.93 Safari/537.36" }
            })
        };
        var id = clear switch
        {
            BiliBiliVideoClear.P360 => 16,
            BiliBiliVideoClear.P480 => 32,
            BiliBiliVideoClear.P720 => 64,
            BiliBiliVideoClear.P1080 => 80,
            var c => throw new NotSupportedException($"不支持的值：{c}")
        };
        var http = HttpClientProvide();
        var json = await http.Request(request, cancellationToken: cancellationToken).Read(x => x.ToObject());
        var dash = json!.GetValueRecursion<IDirect>("data.dash")!;
        var audio = dash.GetValueRecursion<string>("audio[0].base_url")!;
        var video = dash.GetValue<object[]>("video")!.Cast<IDirect>().FirstOrDefault(x => Equals(x["id"], id)) ??
            throw new NotSupportedException($"找不到清晰度为{clear}的视频");
        return (await http.RequestDownload(request with
        {
            Uri = new(video["base_url"]!.ToString()!)
        }, cancellationToken: cancellationToken),
        await http.RequestDownload(request with
        {
            Uri = new(audio)
        }, cancellationToken: cancellationToken));
    }
    #endregion
    #region 传入Uri
    /// <inheritdoc cref="DownloadVideoFromID(string?, string, BiliBiliVideoClear, CancellationToken)"/>
    /// <inheritdoc cref="GetID(string, CancellationToken)"/>
    public async Task<(IBitRead Video, IBitRead Audio)> DownloadVideo(string uri,
        BiliBiliVideoClear clear = BiliBiliVideoClear.P1080,
        CancellationToken cancellationToken = default)
    {
        var (aid, cid) = await GetID(uri, cancellationToken);
        return await DownloadVideoFromID(aid, cid, clear, cancellationToken);
    }

    #endregion
    #endregion
}
