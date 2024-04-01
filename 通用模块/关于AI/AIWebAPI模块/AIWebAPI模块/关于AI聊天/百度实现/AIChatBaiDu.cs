using System.NetFrancis.Api;
using System.NetFrancis.Http;

namespace System.AI;

/// <summary>
/// 使用百度千帆ERNIE 3.5实现的AI聊天接口
/// </summary>
/// <param name="appKey">应用的APPKey</param>
/// <param name="secretKey">应用的SecretKey</param>
/// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
sealed class AIChatBaiDu(string appKey, string secretKey, Func<IHttpClient>? httpClientProvide) :
    WebApi(httpClientProvide), IAIChat
{
    #region 说明文档
    /*如果有问题，请查看文档
      https://cloud.baidu.com/doc/WENXINWORKSHOP/s/llsr6hjxo
     */
    #endregion
    #region 公开成员
    #region 创建上下文
    public async Task<IAIChatContext> Create()
    {
        var httpClient = HttpClientProvide();
        var uri = new UriComplete("https://aip.baidubce.com/oauth/2.0/token")
        {
            UriParameter = new([
                ("grant_type","client_credentials"),
                ("client_id",appKey),
                ("client_secret",secretKey)
                ])
        };
        var response = await httpClient.RequestPost(uri, new()).Read(x => x.ToObject());
        return response.GetValueRecursion<string>("access_token", false) is { } accessToken ?
            new AIChatContextBaiDu(accessToken, HttpClientProvide) :
            throw new APIException($"""
                创建百度AI聊天接口时出现异常，错误码：{response["error"]}，消息：
                {response["error_description"]}
                """);
    }
    #endregion
    #endregion
}
