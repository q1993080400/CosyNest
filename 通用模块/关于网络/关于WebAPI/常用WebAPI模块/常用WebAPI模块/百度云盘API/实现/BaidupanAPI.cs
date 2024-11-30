using System.Design.Direct;
using System.NetFrancis.Http;

namespace System.NetFrancis.Api;

/// <summary>
/// 本类型封装了百度云盘API
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="info">用于创建百度云盘API的参数</param>
public sealed class BaidupanAPI(BaidupanAPIInfo info) : WebApi(info.ServiceProvider), IBaidupanAPI
{
    #region 公开成员
    #region 获取文件或目录列表
    public async Task<BaidupanFDResult> GetFD(string dir = "/")
    {
        var isSingleFile = !Path.GetExtension(dir).IsVoid();
        var newDir = isSingleFile switch
        {
            true => Path.GetDirectoryName(dir) switch
            {
                null or "" => "/",
                { } t => t.Op().ToUriPath()
            },
            false => dir
        };
        #region 本地函数
        HttpRequestRecording Fun(string accessToken)
        {
            var uri = new UriComplete("https://pan.baidu.com/rest/2.0/xpan/file",
               [("method", "list"), ("dir", newDir),
                ("access_token", info.AccessToken)]!);
            return new()
            {
                Uri = uri,
            };
        }
        #endregion
        var dictionary = await GetFDBase(Fun).ToDictionaryAwaitAsync(x => ValueTask.FromResult(x.Path));
        return new()
        {
            Element = dictionary,
            Path = newDir
        };
    }
    #endregion
    #region 搜索文件或目录
    public async Task<BaidupanFD[]> SearchFD(string search, string dir = "/")
    {
        #region 本地函数
        HttpRequestRecording Fun(string accessToken)
        {
            var uri = new UriComplete("https://pan.baidu.com/rest/2.0/xpan/file",
                [("method", "search"), ("access_token", accessToken),
                ("key", search), ("dir", dir), ("recursion", "1")]);
            return new()
            {
                Uri = uri
            };
        }
        #endregion
        return await GetFDBase(Fun).ToArrayAsync();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 发起请求并自动刷新
    /// <summary>
    /// 基础方法，在出现-6错误时，它尝试刷新令牌并重试请求，
    /// 在出现其他错误时，它抛出异常
    /// </summary>
    /// <param name="request">执行请求的委托，它的参数是访问令牌</param>
    /// <returns></returns>
    private async Task<IDirect> Base(Func<string, Task<IDirect>> request)
    {
        var response = await request(info.AccessToken);
        switch (response.GetValue<int>("errno"))
        {
            case 0:
                return response;
            case -6:
                var uri = new UriComplete("https://openapi.baidu.com/oauth/2.0/token",
                    [("grant_type", "refresh_token"),
                    ("refresh_token", info.RefreshToken),
                    ("client_id", info.ClientId),
                    ("client_secret", info.ClientSecret)]);
                var refresh = await HttpClient.RequestJsonGet(uri);
                info = info with
                {
                    AccessToken = refresh!.GetValue<string>("access_token")!,
                    RefreshToken = refresh.GetValue<string>("refresh_token")!
                };
                await info.SaveToken(info);
                return await request(info.AccessToken);
            case var e:
                var message = response.GetValue<string>("errmsg", false) is { } msg ?
                    $"在请求百度云接口时，出现以下错误：{msg}，错误码：{e}" :
                    $"在请求百度云接口时，出现未知错误，错误码：{e}";
                throw new APIException(message)
                {
                    HResult = e
                };
        }
    }
    #endregion
    #region 获取文件或目录的基础方法
    /// <summary>
    /// 获取文件或目录的基础方法
    /// </summary>
    /// <param name="createRequest">这个委托创建请求对象，
    /// 它的参数是访问令牌</param>
    /// <returns></returns>
    private async IAsyncEnumerable<BaidupanFD> GetFDBase(Func<string, HttpRequestRecording> createRequest)
    {
        #region 本地函数
        #region 获取百度云API响应主体
        static IDirect[] GetList(IDirect data)
            => data.GetValue<object[]>("list")!.Cast<IDirect>().ToArray();
        #endregion
        #region 获取百度云文件或目录的路径
        static string GetPath(IDirect data)
            => data.GetValue<string>("path")!;
        #endregion
        #endregion
        var response = await Base(async accessToken => await HttpClient.RequestJson(createRequest(accessToken)));
        var (directory, file) = GetList(response).Split(x => x.GetValue<int>("isdir") is 1);
        foreach (var item in directory)
        {
            yield return BaidupanDirectory.Create(item);
        }
        var ids = file.Select(x => x.GetValue<long>("fs_id")).ToArray();
        if (ids.Length is > 0)
        {
            #region 本地函数
            HttpRequestRecording GetHttpRequest(string accessToken)
            {
                var uri = new UriComplete("https://pan.baidu.com/rest/2.0/xpan/multimedia",
                    [("method", "filemetas"), ("access_token", accessToken),
                    ("fsids", $"[{ids.Join(",")}]"), ("dlink", "1")]!);
                return new()
                {
                    Uri = uri
                };
            }
            #endregion
            var data = await Base(async accessToken => await HttpClient.RequestJson(GetHttpRequest(accessToken)));
            var responseInfo = GetList(data);
            var infoDictionary = responseInfo!.ToDictionary(GetPath, x => x);
            foreach (var item in file)
            {
                yield return BaidupanFile.Create(item, infoDictionary[GetPath(item)], info.AccessToken);
            }
        }
    }
    #endregion
    #endregion
}
