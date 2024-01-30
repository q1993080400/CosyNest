using System.Design;
using System.Design.Direct;
using System.NetFrancis.Http;
using System.Text.Json;

namespace System.NetFrancis.Api.Baidupan;

/// <summary>
/// 本类型封装了百度云盘API
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="accessToken">访问令牌，它用于验证身份</param>
/// <param name="refreshToken">刷新令牌，当访问令牌失效后，通过它刷新访问令牌</param>
/// <param name="clientId">应用ID，它对应AppKey</param>
/// <param name="clientSecret">应用密钥，它对应SecretKey</param>
/// <param name="saveToken">该委托用于保存刷新后的令牌，
/// 它的第一个参数是访问令牌，第二个参数是刷新令牌</param>
/// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
public sealed class BaidupanAPI(string accessToken, string refreshToken,
    string clientId, string clientSecret,
    Func<string, string, Task> saveToken, Func<IHttpClient>? httpClientProvide = null) : WebApi(httpClientProvide)
{
    #region 公开成员
    #region 获取文件或目录列表
    /// <summary>
    /// 获取百度云文件或目录的列表
    /// </summary>
    /// <param name="dir">指定获取哪个目录的文件，
    /// 如果这个路径指向一个文件，则返回它父目录的所有文件（包括它本身）</param>
    /// <returns>一个元组，它的项分别是返回的文件或目录的列表，
    /// 以及一个布尔值，它表示这个路径是否指向一个单一文件，
    /// 通过对比这个文件的路径，就可以找到该单一文件</returns>
    public (IAsyncEnumerable<BaidupanFD> Element, bool IsFile) GetFD(string dir = "/")
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
            => new("https://pan.baidu.com/rest/2.0/xpan/file",
                new[] { ("method", "list"), ("access_token", accessToken), ("dir", newDir) }!);
        #endregion
        return (GetFDBase(Fun), isSingleFile);
    }
    #endregion
    #region 搜索文件或目录
    /// <summary>
    /// 搜索文件或者目录
    /// </summary>
    /// <param name="search">要搜索的文件或目录的名字</param>
    /// <param name="dir">指示应该在什么目录下搜索，如果不指定，默认为搜索全部目录</param>
    /// <returns></returns>
    public IAsyncEnumerable<BaidupanFD> SearchFD(string search, string dir = "/")
    {
        #region 本地函数
        HttpRequestRecording Fun(string accessToken)
            => new("https://pan.baidu.com/rest/2.0/xpan/file",
            new[] { ("method", "search"), ("access_token", accessToken),
            ("key", search), ("dir", dir), ("recursion", "1")}!);
        #endregion
        return GetFDBase(Fun);
    }
    #endregion
    #region 获取媒体播放的M3U8文件
    /// <summary>
    /// 获取一个M3U8文件的文本，
    /// 它可以用于播放媒体
    /// </summary>
    /// <param name="mediumPath">媒体文件的路径</param>
    /// <returns></returns>
    public async Task<string> GetMediumM3U8(string mediumPath)
    {
        var request = new HttpRequestRecording()
        {
            Uri = new()
            {
                UriHost = "https://pan.baidu.com",
                UriExtend = "rest/2.0/xpan/file",
                UriParameter = new(new[]
                {
                    ("method","streaming"),
                    ("access_token",accessToken),
                    ("path",mediumPath),
                    ("type","M3U8_HLS_MP3_128")
                })
            },
            Header = new()
            {
                Host = "pan.baidu.com",
                UserAgent = $"xpanvideo;netdisk;iPhone14;ios-iphone;17;ts"
            },
            ThrowIfNotSuccess = false
        };
        while (true)
        {
            var response = await HttpClientProvide().Request(request);
            var text = await response.Content.ToText();
            #region 抛出异常的本地函数
            string Fun()
                => throw new APIException($"""
                在请求百度云播放媒体接口时出现错误，响应体如下：
                {text}
                """);
            #endregion
            switch ((int)response.Status)
            {
                case 200:
                    return text;
                case >= 400 and < 500:
                    var json = JsonSerializer.Deserialize<IDirect>(text, CreateDesign.JsonCommonOptions);
                    var errno = json?["errno"]?.To<int>() ?? 0;
                    if (errno is not 31341)
                        return Fun();
                    await Task.Delay(2000);
                    break;
                default:
                    return Fun();
            }
        }
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
        var response = await request(accessToken);
        switch (response.GetValue<int>("errno"))
        {
            case 0:
                return response;
            case -6:
                var refresh = await (await HttpClientProvide().Request("https://openapi.baidu.com/oauth/2.0/token", new[]
                {
                    ("grant_type", "refresh_token"),
                    ("refresh_token", refreshToken),
                    ("client_id", clientId),
                    ("client_secret", clientSecret)
                }!)).Content.ToObject();
                accessToken = refresh!.GetValue<string>("access_token")!;
                refreshToken = refresh.GetValue<string>("refresh_token")!;
                await saveToken(accessToken, refreshToken);
                return await request(accessToken);
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
        var response = await Base(async accessToken => (await HttpClientProvide().Request(createRequest(accessToken)).Read(x => x.ToObject()))!);
        var (directory, file) = GetList(response).Split(x => x.GetValue<int>("isdir") is 1);
        foreach (var item in directory)
        {
            yield return new BaidupanDirectory(item);
        }
        var ids = file.Select(x => x.GetValue<long>("fs_id")).ToArray();
        if (ids.Length is > 0)
        {
            #region 本地函数
            HttpRequestRecording GetHttpRequest(string accessToken)
                => new("https://pan.baidu.com/rest/2.0/xpan/multimedia",
                new[] { ("method", "filemetas"), ("access_token", accessToken),
                    ("fsids", $"[{ids.Join(",")}]"), ("dlink", "1") }!);
            #endregion
            var responseInfo = GetList(await Base(async accessToken => (await HttpClientProvide().Request(GetHttpRequest(accessToken)).Read(x => x.ToObject()))!));
            var infoDictionary = responseInfo!.ToDictionary(GetPath, x => x);
            foreach (var item in file)
            {
                yield return new BaidupanFile(item, infoDictionary[GetPath(item)], accessToken, HttpClientProvide);
            }
        }
    }
    #endregion
    #endregion
}
