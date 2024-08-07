﻿using System.NetFrancis.Http;

namespace System.AI;

/// <summary>
/// 这个静态类可以用来创建和AI有关的WebAPI
/// </summary>
public static class CreateAIAPI
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
    #region 创建AI聊天接口
    #region 百度接口
    /// <summary>
    /// 创建百度API聊天接口，
    /// 如果因为各种原因无法创建，会引发异常
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="AI.AIChatBaiDu(string, string, string, Func{IHttpClient}?)"/>
    public static IAIChat AIChatBaiDu(string appKey, string secretKey, string modelUri, Func<IHttpClient>? httpClientProvide = null)
        => new AIChatBaiDu(appKey, secretKey, modelUri, httpClientProvide);
    #endregion
    #endregion
    #region 创建OCR接口
    #region 阿里接口
    /// <summary>
    /// 创建一个由阿里API实现的<see cref="IOCR"/>接口
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="OCRAliYun.OCRAliYun(string, Func{IHttpClient}?)"/>
    public static IOCR OCRAliYun(string appCode, Func<IHttpClient>? httpClientProvide = null)
        => new OCRAliYun(appCode, httpClientProvide);
    #endregion
    #endregion
}
