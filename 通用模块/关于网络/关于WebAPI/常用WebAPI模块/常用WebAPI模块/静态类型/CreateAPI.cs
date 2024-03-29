﻿using System.Net;
using System.NetFrancis.Api.Bing.Image;
using System.NetFrancis.Api.ShortMessage;
using System.NetFrancis.Http;
using System.Underlying.Phone;

namespace System.NetFrancis.Api;

/// <summary>
/// 这个静态类可以用来帮助创建WebApi
/// </summary>
public static class CreateAPI
{
    #region 创建短信接口
    #region 创建国阳短信接口
    #region 测试模板ID
    /// <summary>
    /// 获取国阳云短信接口的测试模板ID
    /// </summary>
    public const string SMSGuoYangeTemplateIdTest = "908e94ccf08b4476ba6c876d13f084ad";
    #endregion
    #region 正式方法
    /// <summary>
    /// 创建国阳短信接口
    /// </summary>
    /// <param name="smsSignId">签名ID，它用于身份验证，
    /// 如果不指定，则使用一个测试ID</param>
    /// <inheritdoc cref="ShortMessageGuoYang(string, string, Func{IHttpClient}?)"/>
    public static IShortMessageManage SMSGuoYange(string appCode, string smsSignId = "2e65b1bb3d054466b82f0c9d125465e2", Func<IHttpClient>? httpClientProvide = null)
        => new ShortMessageGuoYang(appCode, smsSignId, httpClientProvide);
    #endregion
    #endregion
    #region 创建深源恒际接口
    /// <summary>
    /// 创建深源恒际短信接口
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ShortMessageShenYuanHengJi(string, Func{IHttpClient}?)"/>
    public static IShortMessageManage SMSShenYuanHengJi(string appCode, Func<IHttpClient>? httpClientProvide = null)
        => new ShortMessageShenYuanHengJi(appCode, httpClientProvide);
    #endregion
    #endregion
    #region 创建一个必应图片API接口
    /// <summary>
    /// 创建一个必应图片API接口
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="BingImageAPI(Func{IHttpClient}?)"/>
    public static IBingImageAPI ImageAPI(Func<IHttpClient>? httpClientProvide = null)
        => new BingImageAPI(httpClientProvide);
    #endregion
    #region 创建IP查询接口
    /// <summary>
    /// 创建一个可以用来查询公网IP的接口
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="BingImageAPI(Func{IHttpClient}?)"/>
    public static Func<Task<IPAddress>> PublicNetworkIPAPI(Func<IHttpClient>? httpClientProvide = null)
    {
        httpClientProvide ??= () => CreateNet.HttpClientShared;
        return async () =>
        {
            var uri = "https://api.ipify.org/?format=json";
            var request = await httpClientProvide().Request(uri).Read(x => x.ToObject());
            var ip = request["ip"]!.ToString()!;
            return IPAddress.Parse(ip);
        };
    }
    #endregion
}
