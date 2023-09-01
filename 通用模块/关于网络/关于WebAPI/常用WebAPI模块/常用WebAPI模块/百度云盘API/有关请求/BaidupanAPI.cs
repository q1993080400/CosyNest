using System.Design.Direct;
using System.NetFrancis.Http;

namespace System.NetFrancis.Api.Baidupan;

/// <summary>
/// 本类型封装了百度云盘API
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="accessToken">访问令牌，它用于验证身份</param>
/// <param name="refreshToken">刷新令牌，当访问令牌失效后，通过它刷新访问令牌</param>
/// <param name="clientId">应用ID</param>
/// <param name="clientSecret">应用密钥</param>
/// <param name="saveToken">该委托用于保存刷新后的令牌，
/// 它的第一个参数是访问令牌，第二个参数是刷新令牌</param>
/// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
public sealed class BaidupanAPI(string accessToken, string refreshToken,
    string clientId, string clientSecret,
    Action<string, string> saveToken, Func<IHttpClient>? httpClientProvide = null) : WebApi(httpClientProvide)
{
    #region 公开成员
    #region 获取文件或目录列表
    /// <summary>
    /// 获取百度云文件或目录的列表
    /// </summary>
    /// <param name="dir">指定获取哪个目录的文件</param>
    /// <param name="search">指定搜索条件，
    /// 如果为<see langword="null"/>，表示不搜索</param>
    /// <returns></returns>
    public async IAsyncEnumerable<BaidupanFD> GetFD(string dir = "/", string? search = null)
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
        var (request, isSingleFile) = CreateRequest(dir, search);
        var response = await Base(async () => (await HttpClientProvide().Request(request).Read(x => x.ToObject()))!);
        var (directory, file) = GetList(response).Split(x => x.GetValue<int>("isdir") is 1);
        file = file.Where(x => !isSingleFile || GetPath(x) == dir);
        if (!isSingleFile)
        {
            foreach (var item in directory)
            {
                yield return new BaidupanDirectory(item);
            }
        }
        var ids = file.Select(x => x.GetValue<long>("fs_id"));
        var requestInfo = new HttpRequestRecording("http://pan.baidu.com/rest/2.0/xpan/multimedia",
         new[] {("method", "filemetas"), ("access_token", AccessToken),
           ("fsids", $"[{ids.Join(",")}]"), ("dlink", "1") }!);
        var responseInfo = GetList(await Base(async () => (await HttpClientProvide().Request(requestInfo).Read(x => x.ToObject()))!));
        var infoDictionary = responseInfo!.ToDictionary(GetPath, x => x);
        foreach (var item in file)
        {
            yield return new BaidupanFile(item, infoDictionary[GetPath(item)], AccessToken, HttpClientProvide);
        }
    }
    #endregion
    #endregion
    #region 内部成员
    #region 访问令牌
    /// <summary>
    /// 获取访问令牌，它用于验证身份
    /// </summary>
    internal string AccessToken { get; private set; } = accessToken;
    #endregion
    #region 刷新令牌
    /// <summary>
    /// 获取刷新令牌，当访问令牌失效后，
    /// 通过它刷新访问令牌
    /// </summary>
    private string RefreshToken { get; set; } = refreshToken;
    #endregion
    #region 应用ID
    /// <summary>
    /// 获取应用ID
    /// </summary>
    private string ClientId { get; } = clientId;
    #endregion
    #region 应用密钥
    /// <summary>
    /// 获取应用密钥
    /// </summary>
    private string ClientSecret { get; } = clientSecret;
    #endregion
    #region 保存令牌的委托
    /// <summary>
    /// 该委托用于保存刷新后的令牌，
    /// 它的第一个参数是访问令牌，第二个参数是刷新令牌
    /// </summary>
    private Action<string, string> SaveToken { get; } = saveToken;
    #endregion
    #region 辅助方法
    #region 基础方法
    /// <summary>
    /// 基础方法，在出现-6错误时，它尝试刷新令牌并重试请求，
    /// 在出现其他错误时，它抛出异常
    /// </summary>
    /// <param name="request">执行请求的委托</param>
    /// <returns></returns>
    private async Task<IDirect> Base(Func<Task<IDirect>> request)
    {
        var response = await request();
        switch (response.GetValue<int>("errno"))
        {
            case 0:
                return response;
            case -6:
                var refresh = await (await HttpClientProvide().Request("https://openapi.baidu.com/oauth/2.0/token", new[]
                 {
                  ("grant_type", "refresh_token"),
                  ("refresh_token", RefreshToken),
                  ("client_id", ClientId),
                  ("client_secret", ClientSecret)
                 }!)).Content.ToObject();
                AccessToken = refresh!.GetValue<string>("access_token")!;
                RefreshToken = refresh.GetValue<string>("refresh_token")!;
                SaveToken(AccessToken, RefreshToken);
                return await request();
            case var e:
                if (response.GetValue<string>("errmsg", false) is { } msg)
                    throw new APIException($"在请求百度云接口时，出现以下错误：{msg}，错误码：{e}")
                    {
                        HResult = e
                    };
                throw new APIException($"在请求百度云接口时，出现未知错误，错误码：{e}");
        }
    }
    #endregion
    #region 创建请求对象
    /// <summary>
    /// 创建用来向百度云请求数据的Http请求对象
    /// </summary>
    /// <returns>一个元组，它的项分别是创造的请求对象，
    /// 以及这个Uri是否为单个文件，如果是单个文件，需要进行特殊处理</returns>
    /// <inheritdoc cref="GetFD(string, string?)"/>
    private (HttpRequestRecording Request, bool IsSingleFile) CreateRequest(string dir, string? search)
    {
        if (search is null)
        {
            var isSingleFile = !Path.GetExtension(dir).IsVoid();
            dir = isSingleFile ? Path.GetDirectoryName(dir)!.Op().ToUriPath() : dir;
            return (new HttpRequestRecording("https://pan.baidu.com/rest/2.0/xpan/file",
                new[] { ("method", "list"), ("access_token", AccessToken), ("dir", dir) }!), isSingleFile);
        }
        return (new HttpRequestRecording("http://pan.baidu.com/rest/2.0/xpan/file",
            new[] { ("method", "search"), ("access_token", AccessToken),
            ("key", search), ("dir", dir), ("recursion", "1")}!), false);
    }

    #endregion
    #endregion
    #endregion
}
