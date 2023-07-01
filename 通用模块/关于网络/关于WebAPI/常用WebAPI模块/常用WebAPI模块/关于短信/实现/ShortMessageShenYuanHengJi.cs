using System.Net.Http.Headers;
using System.NetFrancis.Http;
using System.Underlying.Phone;

namespace System.NetFrancis.Api.ShortMessage;

/// <summary>
/// 本类型是使用深源恒际实现的发短信接口
/// </summary>
sealed class ShortMessageShenYuanHengJi : WebApi, IShortMessageManage
{
    #region 说明文档
    /*本接口的阿里云页面为https://market.aliyun.com/products/57124001/cmapi00037170.html?spm=5176.730005.result.53.2ba03524Vx9Obn&innerSource=search_%E7%9F%AD%E4%BF%A1#sku=yuncode3117000001
      如有疑问，可以在这里查看*/
    #endregion
    #region App代码
    /// <summary>
    /// 获取App代码，它用于身份验证
    /// </summary>
    private string AppCode { get; }
    #endregion 
    #region 发送短信
    public async Task Send(string message, IEnumerable<string> addressee, CancellationToken cancellation)
    {
        foreach (var mobile in addressee)
        {
            var split = message.Split(Environment.NewLine);
            var httpClient = HttpClientProvide();
            var content = split[1].Replace("=", "%3A");
            var body = $"content={content}&phone_number={mobile}&template_id={split[0]}";
            var request = new HttpRequestRecording("http://dfsns.market.alicloudapi.com/data/send_sms")
            {
                Header = new()
                {
                    Authorization = new("APPCODE", AppCode),
                },
                HttpMethod = HttpMethod.Post,
                Content = new HttpContentRecording()
                {
                    Content = body.ToBytes().ToBitRead(),
                    Header = new()
                    {
                        ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8")
                    }
                }
            };
            var result = await httpClient.Request(request, cancellationToken: cancellation).Read(x => x.ToObject());
            if (result!["status"] is not "OK")
                throw new APIException($"发送到{mobile}的短信失败，错误信息：{result["reason"]}");
        }
    }

    /*问：如何使用本方法的message参数表示短信模板变量？
      答：按照约定，模板ID和模板变量使用换行符分隔，
      然后模板变量使用key=value的形式，用,分隔，
      例如，假设template_id是123，
      content是code%3A1234，
      则message应为：
      
      123
      code=1234*/
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="appCode">App代码，它用于身份验证</param>
    /// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
    public ShortMessageShenYuanHengJi(string appCode, Func<IHttpClient>? httpClientProvide = null)
        : base(httpClientProvide)
    {
        AppCode = appCode;
    }
    #endregion
}
