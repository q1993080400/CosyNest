﻿using System.NetFrancis;
using System.NetFrancis.Api;

namespace System.AI;

/// <summary>
/// 使用百度千帆ERNIE-Speed-128K实现的AI聊天接口
/// </summary>
/// <param name="appKey">应用的APPKey</param>
/// <param name="secretKey">应用的SecretKey</param>
/// <param name="modelUri">获取请求模型的Uri，它可以用来区分使用哪个模型</param>
/// <inheritdoc cref="WebApi(IServiceProvider)"/>
sealed class AIChatBaiDu(string appKey, string secretKey, string modelUri, IServiceProvider serviceProvider) :
    WebApi(serviceProvider), IAIChat
{
    #region 说明文档
    /*如果有问题，请查看文档
      https://cloud.baidu.com/doc/WENXINWORKSHOP/s/6ltgkzya5
     */
    #endregion
    #region 公开成员
    #region 创建上下文
    public async Task<IAIChatContext> Create()
    {
        var httpClient = HttpClient;
        var uri = new UriComplete("https://aip.baidubce.com/oauth/2.0/token")
        {
            UriParameter = new([
                ("grant_type","client_credentials"),
                ("client_id",appKey),
                ("client_secret",secretKey)
                ])
        };
        var response = await httpClient.RequestJsonPost(uri, new object());
        return response.GetValue<string>("access_token", false) is { } accessToken ?
            new AIChatContextBaiDu(accessToken, modelUri, httpClient) :
            throw new APIException($"""
                创建百度AI聊天接口时出现异常，错误码：{response["error"]}，消息：
                {response["error_description"]}
                """);
    }
    #endregion
    #endregion
}
