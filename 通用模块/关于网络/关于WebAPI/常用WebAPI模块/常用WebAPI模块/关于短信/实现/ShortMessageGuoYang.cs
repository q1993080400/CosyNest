using System.NetFrancis.Http;
using System.Underlying.Phone;

namespace System.NetFrancis.Api.ShortMessage;

/// <summary>
/// 本类型是使用国阳云实现的发短信接口
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="smsSignId">签名ID，它用于身份验证</param>
/// <param name="appCode">APP代码</param>
/// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
sealed class ShortMessageGuoYang(string appCode, string smsSignId, Func<IHttpClient>? httpClientProvide = null) : WebApi(httpClientProvide), IShortMessageManage
{
    #region 说明文档
    /*本接口的阿里云页面为https://market.aliyun.com/products/57126001/cmapi00037415.html?spm=5176.730005.result.1.22123524DuLhpQ&innerSource=search_%E7%9F%AD%E4%BF%A1#sku=yuncode31415000012，
      如有问题，可以在这里查看*/
    #endregion
    #region 私有成员
    #region 签名ID
    /// <summary>
    /// 签名ID，它用于身份验证
    /// </summary>
    private string SMSSignId { get; } = smsSignId;
    #endregion
    #region APP代码
    /// <summary>
    /// 获取APP代码
    /// </summary>
    private string AppCode { get; } = appCode;
    #endregion
    #endregion
    #region 发送短信
    public async Task Send(string message, IEnumerable<string> addressee, CancellationToken cancellation)
    {
        foreach (var mobile in addressee)
        {
            var split = message.Split(Environment.NewLine);
            var uri = $"https://gyytz.market.alicloudapi.com/sms/smsSend?mobile={mobile}&smsSignId={SMSSignId}&templateId={split[0]}";
            if (split.Length > 1)
                uri += $"&param={split[1]}";
            var httpClient = HttpClientProvide();
            var request = new HttpRequestRecording(uri)
            {
                Header = new()
                {
                    Authorization = new("APPCODE", AppCode)
                },
                HttpMethod = HttpMethod.Post
            };
            var result = await httpClient.Request(request, cancellationToken: cancellation).Read(x => x.ToObject());
            if (result?.GetValue<int>("code") is not 0)
                throw new APIException($"发送到{mobile}的短信失败，错误信息：{result!["msg"]}");
        }
    }

    #endregion
}
