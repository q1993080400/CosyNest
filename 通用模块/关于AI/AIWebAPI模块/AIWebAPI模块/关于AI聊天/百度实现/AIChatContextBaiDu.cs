using System.Data;
using System.NetFrancis.Api;
using System.NetFrancis.Http;

namespace System.AI;

/// <summary>
/// 使用百度千帆ERNIE 3.5实现的AI聊天接口上下文
/// </summary>
/// <param name="accessToken">访问令牌</param>
/// <inheritdoc cref="AIChatBaiDu(string, string, Func{IHttpClient}?)"/>
sealed class AIChatContextBaiDu(string accessToken, Func<IHttpClient> httpClientProvide) : IAIChatContext
{
    #region 公开成员
    #region 历史消息
    public IReadOnlyList<AIChatMessage> History
        => HistoryCache;
    #endregion
    #region AI聊天
    public async Task<string> Dialogue(string problem)
    {
        var enquire = new AIChatMessage()
        {
            IsHuman = true,
            Message = problem
        };
        var body = History.Append(enquire).Select(x => new
        {
            role = x.IsHuman ? "user" : "assistant",
            content = x.Message
        }).ToArray();
        var uri = new UriComplete("https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/ernie-speed-128k")
        {
            UriParameter = new([("access_token", accessToken)])
        };
        var response = await httpClientProvide().
             RequestPost(uri, new
             {
                 messages = body
             }).Read(x => x.ToObject());
        var resultText = response.GetValue<string>("result", false) is { } result ?
            result :
            throw new APIException($"""
                请求百度AI聊天接口时出现异常，错误码：{response["error_code"]}，消息：
                {response["error_msg"]}
                """);
        HistoryCache.Add(enquire);
        HistoryCache.Add(new()
        {
            IsHuman = false,
            Message = resultText
        });
        return resultText;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 历史消息缓存
    /// <summary>
    /// 获取历史消息缓存
    /// </summary>
    private List<AIChatMessage> HistoryCache { get; } = [];
    #endregion
    #endregion
}
