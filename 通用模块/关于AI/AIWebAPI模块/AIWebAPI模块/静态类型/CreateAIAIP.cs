using System.AI.FaceRecognition;
using System.AI.NaturalLanguage;
using System.NetFrancis.Http;

namespace System.AI;

/// <summary>
/// 这个静态类可以用来创建和AI有关的WebAPI
/// </summary>
public static class CreateAIAIP
{
    #region 创建人脸识别接口
    #region 百度接口
    /// <summary>
    /// 创建一个由百度实现的人脸识别接口
    /// </summary>
    /// <inheritdoc cref="FaceRecognitionBaiDu.FaceRecognitionBaiDu(string, string, Func{IHttpClient}?)"/>
    public static IFaceRecognition FaceRecognitionBaiDu(string apiKey, string secretKey, Func<IHttpClient>? httpClientProvide = null)
        => new FaceRecognitionBaiDu(apiKey, secretKey, httpClientProvide);
    #endregion
    #endregion
    #region 创建自然语言处理接口
    #region 腾讯接口
    /// <summary>
    /// 创建一个由腾讯WebAPI实现的自然语言处理接口
    /// </summary>
    /// <inheritdoc cref="NaturalLanguageTencent.NaturalLanguageTencent(string, string, Func{IHttpClient}?)"/>
    public static INaturalLanguage NaturalLanguageTencent(string secretId,
        string secretKey, Func<IHttpClient>? httpClientProvide = null)
        => new NaturalLanguageTencent(secretId, secretKey, httpClientProvide);
    #endregion
    #endregion
}
