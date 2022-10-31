using System.NetFrancis.Api;
using System.NetFrancis.Http;

using TencentCloud.Common.Profile;
using TencentCloud.Common;
using TencentCloud.Nlp.V20190408.Models;
using TencentCloud.Nlp.V20190408;

namespace System.AI.NaturalLanguage;

/// <summary>
/// 这个类型是使用腾讯WebAPI实现的自然语言处理接口
/// </summary>
sealed class NaturalLanguageTencent : WebApi, INaturalLanguage
{
    #region 公开成员
    #region 人机对话
    public async Task<string> Dialogue(string problem)
    {
        var cred = new Credential
        {
            SecretId = SecretId,
            SecretKey = SecretKey
        };
        var clientProfile = new ClientProfile();
        var httpProfile = new HttpProfile
        {
            Endpoint = "nlp.tencentcloudapi.com"
        };
        clientProfile.HttpProfile = httpProfile;
        var client = new NlpClient(cred, "ap-guangzhou", clientProfile);
        var req = new ChatBotRequest
        {
            Query = problem
        };
        var resp = await client.ChatBot(req);
        return resp.Reply;
    }
    #endregion
    #endregion
    #region 内部成员
    #region SecretId
    /// <summary>
    /// 获取应用ID，它用于验证身份
    /// </summary>
    private string SecretId { get; }
    #endregion
    #region SecretKey
    /// <summary>
    /// 获取应用Key，它用于验证身份
    /// </summary>
    private string SecretKey { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="secretId">应用ID，它用于验证身份</param>
    /// <param name="secretKey">应用Key，它用于验证身份</param>
    /// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
    public NaturalLanguageTencent(string secretId, string secretKey, Func<IHttpClient>? httpClientProvide)
        : base(httpClientProvide)
    {
        SecretId = secretId;
        SecretKey = secretKey;
    }
    #endregion
}
