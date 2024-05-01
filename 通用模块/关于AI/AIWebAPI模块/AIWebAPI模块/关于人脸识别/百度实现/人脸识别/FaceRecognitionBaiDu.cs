using System.Design.Direct;
using System.NetFrancis.Api;
using System.NetFrancis.Http;

namespace System.AI;

/// <summary>
/// 这个类型是使用百度实现的人脸识别接口
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="apiKey">应用的ApiKey，它用于身份验证</param>
/// <param name="secretKey">应用的SecretKey，它用于身份验证</param>
/// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
sealed class FaceRecognitionBaiDu(string apiKey, string secretKey, Func<IHttpClient>? httpClientProvide)
    : WebApi(httpClientProvide), IFaceRecognition
{
    #region 公开成员
    #region 执行人脸检测
    public async Task<IFaceRecognitionInfo> Recognition(string imageBase64, CancellationToken cancellation)
    {
        var accessToken = await GetAccessToken();
        var response = await HttpClientProvide().RequestPost("https://aip.baidubce.com/rest/2.0/face/v3/detect",
            new
            {
                image = imageBase64,
                image_type = "BASE64"
            }, null, new[]
            {
                ("access_token", accessToken)
            }!, cancellationToken: cancellation).Read(x => x.ToObject());
        Check(response);
        return new FaceRecognitionInfo()
        {
            FaceCount = response.GetValue<object>("result.face_num").To<int>(false)
        };
    }
    #endregion
    #endregion
    #region 内部成员
    #region 获取访问令牌
    /// <summary>
    /// 获取访问令牌
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetAccessToken()
    {
        var response = await HttpClientProvide().RequestPost("https://aip.baidubce.com/oauth/2.0/token",
            new { }, null,
            new[]
            {
                ("grant_type","client_credentials"),
                ("client_id",apiKey),
                ("client_secret",secretKey)
            }!).Read(x => x.ToObject());
        return Check(response)["access_token"]!.ToString()!;
    }
    #endregion
    #region 检查API返回值
    /// <summary>
    /// 检查返回值，如果存在错误信息，则抛出异常，
    /// 否则将其原路返回
    /// </summary>
    /// <param name="response">待检查的返回值</param>
    /// <returns></returns>
    private static IDirect Check(IDirect response)
    {
        if (response.TryGetValue("error") is (true, var value))
            throw new APIException($"""
                请求百度人脸识别API遇到以下错误：
                {value}
                {response["error_description"]}
                """);
        return response;
    }
    #endregion
    #endregion
}
